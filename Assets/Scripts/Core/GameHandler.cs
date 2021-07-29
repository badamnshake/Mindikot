using System.Collections.Generic;
using DataStructs;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class GameHandler : NetworkBehaviour
{
    public GameObject cardPrefab;
    public GameObject playerArea;
    public CardImages cardImages;
    public CardStyle currentCardStyle = CardStyle.Normal;

    private List<int> p1Cards;
    private List<int> p2Cards;
    private List<int> p3Cards;
    private List<int> p4Cards;

    private List<int> currentHand = new List<int>(4);

    public bool signalGameStart;

    private void Update()
    {
        if (signalGameStart)
        {
            DrawCards();
            signalGameStart = false;
        }
    }

    public void DrawCards()
    {
        DeckManager.DistributeCards(out p1Cards, out p2Cards, out p3Cards, out p4Cards);
        p1Cards.Sort();

        foreach (var cardValue in p1Cards)
        {
            GameObject card = Instantiate(cardPrefab, Vector2.zero, Quaternion.identity);
            card.transform.SetParent(playerArea.transform, false);
            card.GetComponent<Image>().sprite = cardImages.GetCardImage(cardValue, currentCardStyle);
            card.GetComponent<Card>().SetValue(cardValue);
        }
    }

    
}