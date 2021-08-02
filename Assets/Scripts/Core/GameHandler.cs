using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataStructs;
using Mirror;
using UnityEngine;

public class GameHandler : NetworkBehaviour
{
    private GameState gameState;
    private int[][] _distributedCards;

    private List<NetworkGamePlayer> _playerList = new List<NetworkGamePlayer>();

    private List<int> currentHand = new List<int>(4);
    private int currentTurn = 0;

    [SerializeField] private GameObject scoreBoardPrefab;

    private ScoreBoard _scoreBoard;


    [Server]
    public void StartGame()
    {
        if (!isServer) return;

        gameState = GameState.Start;

        GameObject scoreboardInstance = Instantiate(scoreBoardPrefab, transform);
        _scoreBoard = scoreboardInstance.GetComponent<ScoreBoard>();

        NetworkServer.Spawn(scoreboardInstance);
        _scoreBoard.ResetValues();


        _distributedCards = DeckManager.DistributeCards();

        DrawCards();
        gameState = GameState.Play;
        // StartCoroutine(GameLoop());
    }


    [Server]
    public void DrawCards()
    {
        for (var i = 0; i < _playerList.Count; i++)
        {
            var gamePlayer = _playerList[i];
            gamePlayer.SetMyCards(_distributedCards[i].ToList());
        }

        StartCoroutine(GameLoop());
    }

    [Server]
    IEnumerator GameLoop()
    {
        currentTurn = 0;
        Team winnerTeam; // blue team wins -1 orange -2
        Suit currentTurnSuit = Suit.None;
        Suit rullingSuit = Suit.None;
        int wasMindiHand;

        for (int i = 0; i < 13; i++)
        {
            currentHand.Clear();
            
            for (int j = 0; j < 4; j++)
            {
                var playerInTurn = _playerList[currentTurn];
                playerInTurn.StartTurn();

                yield return new WaitUntil(() => playerInTurn.cardToPlay > -1);
                if (currentTurnSuit == Suit.None) currentTurnSuit = DeckManager.GetCardSuit(playerInTurn.cardToPlay);

                currentHand[currentTurn] = playerInTurn.cardToPlay;
                currentHand.Add(playerInTurn.cardToPlay);
                RotateTurn();
            }

            int winIndex = DeckManager.DeclareWinnerCard(currentHand, currentTurnSuit, rullingSuit, out wasMindiHand);

            currentTurn = winIndex;

            if (winIndex == 0 || winIndex == 2)
                winnerTeam = Team.Blue;
            else
                winnerTeam = Team.Orange;
            _scoreBoard.IncreaseTeamHandCount(winnerTeam);

            if (wasMindiHand != -1) _scoreBoard.MindiIsCaptured(DeckManager.GetCardSuit(wasMindiHand), winnerTeam);
        }
    }


    [Server]
    void RotateTurn() => currentTurn = currentTurn + 1 == _playerList.Count ? 0 : currentTurn + 1;


    public void SetPlayers(List<NetworkGamePlayer> gamePlayers) => _playerList = gamePlayers;
}