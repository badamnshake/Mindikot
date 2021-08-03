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

    private LinkedList<int> turnRotator = new LinkedList<int>(new int[] {0, 1, 2, 3});
    private LinkedListNode<int> _currentTurn;

    [SerializeField] private GameObject scoreBoardPrefab;

    private ScoreBoard _scoreBoard;


    [Server]
    public void StartGame()
    {
        if (!isServer) return;

        _currentTurn = turnRotator.First;

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
        Team winnerTeam; // blue team wins -1 orange -2
        Suit currentTurnSuit = Suit.None;
        Suit rulingSuit = Suit.None;
        int wasMindiHand;
        _currentTurn = turnRotator.First;

        for (int i = 0; i < 13; i++)
        {
            currentHand = new List<int>(4);
            var turnIndex = new List<int>(4);

            for (int j = 0; j < 4; j++)
            {
                var playerInTurn = _playerList[_currentTurn.Value];
                playerInTurn.StartTurn(currentTurnSuit, rulingSuit);

                yield return new WaitWhile(() => playerInTurn.cardToPlay == -1);
                print("current Turn was" + _currentTurn.Value);

                Suit playedCardSuit = DeckManager.GetCardSuit(playerInTurn.cardToPlay);
                if (currentTurnSuit == Suit.None)
                {
                    currentTurnSuit = playedCardSuit;
                }
                else if (playedCardSuit != currentTurnSuit)
                {
                    rulingSuit = playedCardSuit;
                }

                turnIndex.Add(_currentTurn.Value);
                currentHand.Add(playerInTurn.cardToPlay);
                RotateTurn();
            }

            int winIndex = DeckManager.DeclareWinnerCard(currentHand, currentTurnSuit, rulingSuit, out wasMindiHand);

            _currentTurn = turnRotator.Find(turnIndex[winIndex]);

            winnerTeam = turnIndex[winIndex] % 2 == 0 ? Team.Blue : Team.Orange;

            _scoreBoard.IncreaseTeamHandCount(winnerTeam);

            if (wasMindiHand != -1) _scoreBoard.MindiIsCaptured(DeckManager.GetCardSuit(wasMindiHand), winnerTeam);

            yield return new WaitForSeconds(2f);
        }
    }


    [Server]
    void RotateTurn() => _currentTurn = _currentTurn.Next ?? _currentTurn.List.First;


    public void SetPlayers(List<NetworkGamePlayer> gamePlayers) => _playerList = gamePlayers;
}