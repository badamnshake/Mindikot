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
    private int spadeCaptured = -1;

    [SyncVar(hook = nameof(OnHeartMindiCaptured))]
    private int heartCaptured = -1;

    [SyncVar(hook = nameof(OnDiamondMindiCaptured))]
    private int diamondCaptured = -1;

    [SyncVar(hook = nameof(OnClubMindiCaptured))]
    private int clubCaptured = -1;

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

        spadeCaptured = 0;
        heartCaptured = 0;
        diamondCaptured = 0;
        clubCaptured = 0;
    }

    // Public methods :invoked by GameHandler

    [Server]
    public void MindiIsCaptured(Suit suit, int team)
    {
        if (team == 1)
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
    public void IncreaseBlueTeamCount() => _tBlueHandsCount++;

    [Server]
    public void IncreaseOrangeTeamCount() => _tOrangeHandsCount++;

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

    void OnSpadeMindiCaptured(int oldValue, int newValue) =>
        cardsCapturedDisplay[0].color = newValue == 1 ? blueTeamColor :
            spadeCaptured == 2 ? orangeTeamColor : notCapturedColor;

    void OnHeartMindiCaptured(int oldValue, int newValue) =>
        cardsCapturedDisplay[1].color = newValue == 1 ? blueTeamColor :
            spadeCaptured == 2 ? orangeTeamColor : notCapturedColor;

    void OnDiamondMindiCaptured(int oldValue, int newValue) =>
        cardsCapturedDisplay[2].color = newValue == 1 ? blueTeamColor :
            spadeCaptured == 2 ? orangeTeamColor : notCapturedColor;

    void OnClubMindiCaptured(int oldValue, int newValue) =>
        cardsCapturedDisplay[3].color = newValue == 1 ? blueTeamColor :
            spadeCaptured == 2 ? orangeTeamColor : notCapturedColor;
}