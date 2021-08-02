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

    public static int[][] DistributeCards()
    {
        ShuffleDeck();
        int[][] distCards = new int[4][];
        distCards[0] = new int[13];
        distCards[1] = new int[13];
        distCards[2] = new int[13];
        distCards[3] = new int[13];


        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 13; j++)
            {
                int index = (i * 13) + j;
                distCards[i][j] = _deck[index];
            }
        }

        return distCards;
    }

    /// <summary>
    /// from cards Array returns the value that wins in a face off in mindikot if not mindi in hand returns -1 otherwise index
    /// </summary>
    public static int DeclareWinnerCard(List<int> cards, Suit turnSuit, Suit rulingSuit, out int mindiHand)
    {
        mindiHand = -1;
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
            if (MindiCheck(card)) mindiHand = card;
            if (IsInSuit(rulingSuit, card)) cardShortList.Add(card);
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

    public static Suit GetCardSuit(int card) =>
        card <= 13 ? Suit.Spade : card <= 26 ? Suit.Heart : card <= 39 ? Suit.Diamond : Suit.Club;

    static bool MindiCheck(int card)
        => card == 8 || card == 21 || card == 34 || card == 47;
}