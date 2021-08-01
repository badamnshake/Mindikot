using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;

public class GameHandler : NetworkBehaviour
{
    private int[][] _distributedCards;

    private List<NetworkGamePlayer> _playerList = new List<NetworkGamePlayer>();
    private bool _signalGameStart;

    private List<int> currentHand = new List<int>(4);

    [SerializeField] private ScoreBoard _scoreBoard;


    private void Update()
    {
        if (!isServer) return;
        if (_signalGameStart)
        {
            _scoreBoard.ResetValues();
            _distributedCards = DeckManager.DistributeCards();
            DrawCards();
            _signalGameStart = false;
        }
    }

    [Server]
    public void DrawCards()
    {
        for (var i = 0; i < _playerList.Count; i++)
        {
            var gamePlayer = _playerList[i];
            gamePlayer.SetMyCards(_distributedCards[i].ToList());
        }
    }


    public void SetPlayers(List<NetworkGamePlayer> gamePlayers) => _playerList = gamePlayers;
    public void SetSignalGameStart(bool value) => _signalGameStart = value;
}