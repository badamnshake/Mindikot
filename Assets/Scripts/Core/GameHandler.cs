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
            gamePlayer.StartTurn();
        }
    }

    // [Server]
    // // IEnumerator GameLoop()
    // {
    //     yield return new WaitUntil(() => _playerList[0].cardToPlay > -1);
    //     for (int i = 0; i < 13; i++)
    //     {
    //         // yield return TurnLoop();
    //     }
    // }
    //
    // IEnumerator TurnLoop()
    // {
    //     currentHand.Clear();
    //     for (int i = 0; i < 4; i++)
    //     {
    //         yield return new WaitForSeconds(7f);
    //         currentHand.Add(_playerList[currentTurn].cardToPlay);
    //         RotateTurn();
    //     }
    //
    //     yield return new WaitForSeconds(1f);
    //
    //     print("one hand played");
    // }

    [Server]
    void RotateTurn() => currentTurn = currentTurn + 1 == _playerList.Count ? 0 : currentTurn + 1;


    public void SetPlayers(List<NetworkGamePlayer> gamePlayers) => _playerList = gamePlayers;
}