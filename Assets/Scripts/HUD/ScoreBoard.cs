using System.Collections.Generic;
using System.Linq;
using DataStructs;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard : NetworkBehaviour
{
    [Header("Add in order Spade-Heart-Diamond-Club")] [SerializeField]
    private Image[] cardsCapturedDisplay;

    [SerializeField] private Color notCapturedColor;
    [SerializeField] private Color blueTeamColor;
    [SerializeField] private Color orangeTeamColor;

    [SerializeField] private Text tBlueCountDisplay;
    [SerializeField] private Text tOrangeCountDisplay;

    // 0- none 1 - blue  2 - orange has the 10 card 

    [SyncVar(hook = nameof(OnSpadeMindiCaptured))]
    private Team spadeCaptured;

    [SyncVar(hook = nameof(OnHeartMindiCaptured))]
    private Team heartCaptured;

    [SyncVar(hook = nameof(OnDiamondMindiCaptured))]
    private Team diamondCaptured;

    [SyncVar(hook = nameof(OnClubMindiCaptured))]
    private Team clubCaptured;

    [SyncVar(hook = nameof(SetTeamBlueHandCountDisplay))]
    private int _tBlueHandsCount = -1;

    [SyncVar(hook = nameof(SetTeamOrangeHandCountDisplay))]
    private int _tOrangeHandsCount = -1;

    private int _tBlueMindiCount = -1;
    private int _tOrangeMindiCount = -1;


    // initializes starting values
    [Server]
    public void ResetValues()
    {
        _tBlueHandsCount = 0;
        _tOrangeHandsCount = 0;

        _tBlueMindiCount = 0;
        _tOrangeMindiCount = 0;

        spadeCaptured = Team.None;
        heartCaptured = Team.None;
        diamondCaptured = Team.None;
        clubCaptured = Team.None;
    }

    // Public methods :invoked by GameHandler

    [Server]
    public void MindiIsCaptured(Suit suit, Team team)
    {
        if (team == Team.Blue)
            _tBlueMindiCount++;

        else
            _tOrangeMindiCount++;

        if (suit == Suit.Spade)
            spadeCaptured = team;
        else if (suit == Suit.Heart)
            heartCaptured = team;
        else if (suit == Suit.Diamond)
            diamondCaptured = team;
        else if (suit == Suit.Club) clubCaptured = team;
    }

    [Server]
    public void IncreaseTeamHandCount(Team team)
    {
        if (team == Team.Blue)
            _tBlueHandsCount++;
        else if (team == Team.Orange)
            _tOrangeHandsCount++;
        else
            Debug.Log("passinng team as none in scoreboard probably init error");
    }


    // 0 - none 1 - blue  2 - orange has won
    public int CheckForWin()
    {
        if (_tBlueMindiCount > 2 || (_tBlueMindiCount == 2 && _tBlueHandsCount >= 7))
            return 1;
        if (_tOrangeMindiCount > 2 || (_tOrangeMindiCount == 2 && _tOrangeHandsCount >= 7))
            return 2;
        return 0;
    }


    // ClientSide
    // Hooks that display values on clients onChange of values
    void SetTeamOrangeHandCountDisplay(int oldValue, int newValue) =>
        tOrangeCountDisplay.text = _tOrangeHandsCount.ToString();

    void SetTeamBlueHandCountDisplay(int oldValue, int newValue) =>
        tBlueCountDisplay.text = _tBlueHandsCount.ToString();

    void OnSpadeMindiCaptured(Team oldValue, Team newValue) => ChagneCardsCapturedDisplay(newValue, 0);

    void OnHeartMindiCaptured(Team oldValue, Team newValue) => ChagneCardsCapturedDisplay(newValue, 1);

    void OnDiamondMindiCaptured(Team oldValue, Team newValue) => ChagneCardsCapturedDisplay(newValue, 2);
    void OnClubMindiCaptured(Team oldValue, Team newValue) => ChagneCardsCapturedDisplay(newValue, 3);

    void ChagneCardsCapturedDisplay(Team team, int index)
    {
        if (team == Team.Blue)
            cardsCapturedDisplay[2].color = blueTeamColor;
        else if (team == Team.Orange)
            cardsCapturedDisplay[2].color = orangeTeamColor;
        else
            cardsCapturedDisplay[2].color = notCapturedColor;
    }
}