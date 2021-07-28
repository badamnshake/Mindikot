using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using DataStructs;

public static class DeckManager
{
    // Deck has 52 ints each correspond to a card : 1: spade 14: heart 27: diamond 40: club
    // deck starts from 2 = 1..... q, king , ace = 13
    private static List<int> _deck
        = Enumerable.Range(1, 52).ToList();

    // each player will have 13 cards to play


    // shuffles the deck
    static void ShuffleDeck()
    {
        List<int> newDeck = _deck;
        int currentIndex = newDeck.Count - 1;

        while (0 != currentIndex)
        {
            // Knuth shuffle style array 

            var randomIndex = Random.Range(0, currentIndex);

            // swap index with randomOne
            var temp = newDeck[currentIndex];
            newDeck[currentIndex] = newDeck[randomIndex];
            newDeck[randomIndex] = temp;

            currentIndex--;
        }

        _deck = newDeck;
    }

    public static void DistributeCards(out List<int> p1, out List<int> p2, out List<int> p3, out List<int> p4)
    {
        ShuffleDeck();
        p1 = new List<int>();
        p2 = new List<int>();
        p3 = new List<int>();
        p4 = new List<int>();
        for (int i = 0; i < 13; i++)
        {
            p1.Add(_deck[i]);
        }

        for (int i = 13; i < 26; i++)
        {
            p2.Add(_deck[i]);
        }

        for (int i = 26; i < 39; i++)
        {
            p3.Add(_deck[i]);
        }

        for (int i = 39; i < 52; i++)
        {
            p4.Add(_deck[i]);
        }
    }

    /// <summary>
    /// from cards Array returns the value that wins in a face off in mindikot
    /// </summary>
    public static int DeclareWinnerCard(List<int> cards, Suit turnSuit, Suit rulingSuit)
    {
        List<int> cardShortList = new List<int>();


        if (turnSuit == Suit.None)
        {
            Debug.LogError("passing turn suit as none");
            return -1;
        }

        // if there isn't a ruling class then take turn as ruling
        if (rulingSuit == Suit.None)
            rulingSuit = turnSuit;


        foreach (var card in cards)
        {
            if (IsInSuit(rulingSuit, card))
            {
                cardShortList.Add(card);
            }
        }

        // if there were none ruling cards then take turn as ruling
        if (cardShortList.Count == 0)
        {
            foreach (var card in cards)
            {
                if (IsInSuit(turnSuit, card))
                {
                    cardShortList.Add(card);
                }
            }
        }

        return cards.IndexOf(cardShortList.Max());
    }


    static bool IsInSuit(Suit suit, int card) =>
        card > (int) suit * 13 - 13 && card <= (int) suit * 13;

    static Suit GetCardSuit(int card) =>
        card <= 13 ? Suit.Spade : card <= 26 ? Suit.Heart : card <= 39 ? Suit.Diamond : Suit.Club;
}