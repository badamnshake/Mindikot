using System;
using System.Collections;
using System.Collections.Generic;
using DataStructs;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class NetworkGamePlayer : NetworkBehaviour
{
    [Header("UI")] [SerializeField] private GameObject lobbyUI;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private GameObject playerArea;
    [SerializeField] private CardImages cardImages;
    [SerializeField] private CardStyle currentCardStyle = CardStyle.Normal;
    [SerializeField] private Image[] cardsDisplay;
    [SerializeField] private Sprite emptyCard;

    [SerializeField] private Image[] cardImageDisplay;
    [SerializeField] private Card[] cards;

    // time indicator used for UI
    [SerializeField] private Color indicatorOffColor;
    [SerializeField] private Color indicatorOnColor;
    [SerializeField] private Color indicatorWarningColor;

    [SyncVar] private string _displayName = "Loading...";


    [SyncVar(hook = nameof(SetCards))] private List<int> _myCards = new List<int>();
    [SyncVar(hook = nameof(PlayTurn))] private bool isMyTurn = false;

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
    public void StartTurn() => isMyTurn = true;

    public override void OnStartAuthority()
    {
        lobbyUI.SetActive(true);
    }


    // when its turn this will be played
    public void PlayTurn(bool oldValue, bool newValue) => StartCoroutine(MyTurn());

    IEnumerator MyTurn()
    {
        cardToPlay = -1;
        yield return new WaitForSeconds(7f);
        isMyTurn = false;

        if (cardToPlay == -1) cardToPlay = PlayRandomCard();

        _myCards.Remove(cardToPlay);
    }

    private int PlayRandomCard()
    {
        // TODO: add logic to play a card from appropriate suit
        return _myCards[0];
    }


    public void OnButtonClickPlayCard(Card card)
    {
        if (!isMyTurn) return;
        cardToPlay = card.GetValue();
        card.gameObject.SetActive(false);
        isMyTurn = false;
    }

    [Server]
    public void SetMyCards(List<int> listCards) => _myCards = listCards;

    // syncVAr hook responding to card get set
    public void SetCards(List<int> oldValues, List<int> newCards)
    {
        // instantiating cards on the ui to play

        for (var i = 0; i < newCards.Count; i++)
        {
            int value = newCards[i];
            cards[i].SetValue(value);
            cardImageDisplay[i].sprite = cardImages.GetCardImage(value, currentCardStyle);
            cards[i].gameObject.SetActive(true);
        }
    }
}