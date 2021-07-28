using DataStructs;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CardImageObj", order = 1)]
public class CardImages : ScriptableObject
{
    public Sprite[] simple;
    public Sprite[] ancient;
    public Sprite[] retro;

    public Sprite GetCardImage(int cardValue, CardStyle cardStyle)
    {
        switch (cardStyle)
        {
            case CardStyle.Ancient:
                return ancient[cardValue - 1];
            case CardStyle.Retro:
                return retro[cardValue - 1];
            default:
                return simple[cardValue - 1];
        }
    }
}