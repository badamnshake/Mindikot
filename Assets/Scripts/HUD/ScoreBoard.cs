using System.Collections.Generic;
using DataStructs;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard : NetworkBehaviour
{
    [Header("Add in order Spade-Heart-Diamond-Club")] [SerializeField]
    private Image[] cardsCapturedDisplay;

    [SerializeField] private Color blueTeamColor;
    [SerializeField] private Color orangeTeamColor;

    [SerializeField] private Text tBlueCountDisplay;
    [SerializeField] private Text tOrangeCountDisplay;

    // 0- none 1 - blue  2 - orange has the 10 card 
    [SyncVar(hook = nameof(OnChangeIsCaptured))]
    private List<int> _isCaptured;


    [SyncVar(hook = nameof(SetTeamBlueHandCountDisplay))]
    private int _tBlueHandsCount;

    [SyncVar(hook = nameof(SetTeamOrangeHandCountDisplay))]
    private int _tOrangeHandsCount;

    private int _tBlueMindiCount;
    private int _tOrangeMindiCount;
    
    
    // initializes starting values
    [Server]
    public void ResetValues()
    {
        _isCaptured = new List<int>() {0, 0, 0, 0};
        _tBlueHandsCount = 0;
        _tOrangeHandsCount = 0;
        _tBlueMindiCount = 0;
        _tOrangeMindiCount = 0;
    }

    // Public methods :invoked by GameHandler

    [Server]
    public void MindiIsCaptured(Suit suit, int team)
    {
        _isCaptured[(int) suit - 1] = team;
        if (team == 1) _tBlueMindiCount++;
        else _tOrangeMindiCount++;
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
        if (_tOrangeMindiCount > 2 || (_tOrangeMindiCount == 2 && _tBlueHandsCount >= 7))
            return 2;
        return 0;
    }
    
    
    // ClientSide
    // Hooks that display values on clients onChange of values
    [Client]
    void SetTeamOrangeHandCountDisplay(int oldValue, int newValue) =>
        tBlueCountDisplay.text = _tBlueHandsCount.ToString();

    [Client]
    void SetTeamBlueHandCountDisplay(int oldValue, int newValue) =>
        tOrangeCountDisplay.text = _tOrangeHandsCount.ToString();

    [Client]
    void OnChangeIsCaptured(List<int> oldValue, List<int> newValue)
    {
        for (int i = 0; i < _isCaptured.Count; i++)
        {
            cardsCapturedDisplay[i].color = _isCaptured[i] switch
            {
                1 => blueTeamColor,
                2 => orangeTeamColor,
                _ => cardsCapturedDisplay[i].color
            };
        }
    }
}