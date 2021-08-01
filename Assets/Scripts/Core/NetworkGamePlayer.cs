using System;
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


    [SyncVar] private string _displayName = "Loading...";


    [SyncVar(hook = nameof(SetCards))] private List<int> _myCards = new List<int>();

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
    public void SetDisplayName(string newName)
    {
        _displayName = newName;
    }

    public override void OnStartAuthority()
    {
        lobbyUI.SetActive(true);
    }

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

    [Server]
    public void SetMyCards(List<int> listCards)
    {
        _myCards = listCards;
    }
}