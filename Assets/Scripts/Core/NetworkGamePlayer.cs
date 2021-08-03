using System;
using System.Collections;
using System.Collections.Generic;
using DataStructs;
using Mirror;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class NetworkGamePlayer : NetworkBehaviour
{
    [Header("UI")] [SerializeField] private GameObject lobbyUI;
    [SerializeField] private CardImages cardImages;
    [SerializeField] private CardStyle currentCardStyle = CardStyle.Normal;

    [SerializeField] private Image[] cardImageDisplay;
    [SerializeField] private Card[] cards;

    // time indicator used for UI
    [SerializeField] private Color indicatorOffColor;
    [SerializeField] private Color indicatorOnColor;
    [SerializeField] private Color indicatorWarningColor;
    [SerializeField] private GameObject yourTurnIndicator;

    [SyncVar] private string _displayName = "Loading...";


    [SyncVar(hook = nameof(SetCards))] private List<int> _myCards = new List<int>();
    [SyncVar(hook = nameof(OnStartTurn))] private bool _isMyTurn = false;

    private Suit _currentSuit;
    private Suit _rulingSuit;
    private List<Card> _playableCards = new List<Card>();

    private bool _isSetRulingCards = false;

    public int cardToPlay;

    private NetManLobby _room;

    private NetManLobby Room
    {
        get
        {
            if (_room != null)
            {
                return _room;
            }

            return _room = NetworkManager.singleton as NetManLobby;
        }
    }

    public override void OnStartClient()
    {
        DontDestroyOnLoad(gameObject);

        Room.GamePlayers.Add(this);
    }

    public override void OnStopClient()
    {
        Room.GamePlayers.Remove(this);
    }

    [Server]
    public void SetDisplayName(string newName) => _displayName = newName;

    [Server]
    public void StartTurn(Suit currentSuit, Suit rulingSuit)
    {
        _isMyTurn = true;
        _currentSuit = currentSuit;
        _rulingSuit = rulingSuit;
    }

    public void OnStartTurn(bool oldValue, bool newValue)
    {
        if (!_isMyTurn) return;
        yourTurnIndicator.SetActive(true);

        _playableCards.Clear();
        bool foundCards = false;

        cardToPlay = -1;

        if (!_isSetRulingCards && _rulingSuit != Suit.None)
        {
            foreach (var card in cards)
            {
                if (!IsCardInSuit(_currentSuit, card.GetValue())) continue;
                card.SetCardRuling();
            }

            _isSetRulingCards = true;
            
        }

        if (_currentSuit == Suit.None)
        {
            SetAllCardsPlayable();
        }
        else
        {
            foreach (var card in cards)
            {
                if (!IsCardInSuit(_currentSuit, card.GetValue())) continue;
                if (!foundCards) foundCards = true;
                _playableCards.Add(card);
                card.SetInteractable(true);
            }

            if (!foundCards)
            {
                SetAllCardsPlayable();
                if (_rulingSuit == Suit.None) MessageShow(setRulingCard: true);
            }
        }

        StartCoroutine(TurnBehaviour());
    }

    IEnumerator TurnBehaviour()
    {
        yield return new WaitForSeconds(7f);
        // if player misses to play a card then play a random card
        cardToPlay = PlayRandomCard();
        EndTurn();
    }

    public void OnButtonClickPlayCard(Card card)
    {
        if (!_isMyTurn) return;
        cardToPlay = card.GetValue();
        card.gameObject.SetActive(false);
        // if a player plays a card then stop the coroutine
        StopCoroutine(TurnBehaviour());
        EndTurn();
    }

    public override void OnStartAuthority()
    {
        lobbyUI.SetActive(true);
    }

    void SetAllCardsPlayable()
    {
        foreach (var card in cards)
        {
            _playableCards.Add(card);
            card.SetInteractable(true);
            card.SetCardRuling();
        }
    }

    void EndTurn()
    {
        yourTurnIndicator.SetActive(false);
        foreach (var card in cards) card.SetInteractable(false);
        _isMyTurn = false;
    }


    // when its turn this will be played


    private int PlayRandomCard()
    {
        Card randomCard = _playableCards[Random.Range(0, _playableCards.Count)];
        foreach (Card card in cards)
        {
            if (card.Equals(randomCard))
            {
                card.gameObject.SetActive(false);
            }
        }

        return randomCard.GetValue();
    }


    [Server]
    public void SetMyCards(List<int> listCards) => _myCards = listCards;

    // syncVAr hook responding to card get set
    public void SetCards(List<int> oldValues, List<int> newCards)
    {
        // instantiating cards on the ui to play

        for (var i = 0; i < _myCards.Count; i++)
        {
            int value = _myCards[i];
            cards[i].SetValue(value);
            cardImageDisplay[i].sprite = cardImages.GetCardImage(value, currentCardStyle);
            cards[i].gameObject.SetActive(true);
        }
    }

    bool IsCardInSuit(Suit suit, int card) =>
        card > (int) suit * 13 - 13 && card <= (int) suit * 13;

    void MessageShow(bool setRulingCard = false, bool notYourTurn = false)
    {
        // TODO: add PopUpMessageSystem
        if (setRulingCard)
        {
            print("hooray define ruling card");
        }

        if (notYourTurn)
        {
            print("its not your turn");
        }
    }
}