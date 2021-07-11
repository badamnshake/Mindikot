using System;
using UnityEngine;


[CreateAssetMenu(fileName = "", menuName = "MindiKot/CardPrefabs", order = 0)]
public class ScriptableCardPrefabs : ScriptableObject
{
    public GameObject[] Spades;
    public GameObject[] Hearts;
    public GameObject[] Diamonds;
    public GameObject[] Clubs;

    public GameObject GetCardPrefab(Tuple<int, int> card)
    {
        switch (card.Item1)
        {
            case 1:
                return Spades[card.Item2 - 1];
            case 2:
                return Hearts[card.Item2 - 1];
            case 3:
                return Diamonds[card.Item2 - 1];
            case 4:
                return Clubs[card.Item2 - 1];
            default:
                return new GameObject("cream");
        }
    }
}
