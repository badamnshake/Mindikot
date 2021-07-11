using System;
using System.Linq;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataStructs;

public class DeckManager : MonoBehaviour
{
    Dictionary<int, Tuple<int, int>> _deck;
    int _cardsPerHand;

    void Start()
    {
        _deck = new Dictionary<int, Tuple<int, int>>();
        SetUpVariables((int)NumOfPlayer.Four, (int)NumOfDeck.One);
    }

    public void ChangeSettings(NumOfPlayer numOfPlayer, NumOfDeck numOfDeck)
    {
        SetUpVariables((int)numOfPlayer, (int)numOfDeck);
    }

    public Dictionary<int, Tuple<int, int>> GetShuffledDeck()
    {
        ShuffleDeck(_deck);
        return _deck;
    }

    void SetUpVariables(int numOfPlayer, int numOfDecks)
    {
        _deck.Clear();
        if ((numOfPlayer == 4 && numOfDecks == 1) || (numOfPlayer == 8 && numOfDecks == 2))
        {
            FillDeck(1, numOfDecks);
        }
        else if (numOfPlayer == 6 && numOfDecks == 3 || (numOfPlayer == 4 && numOfDecks == 2))
        {
            FillDeck(6, numOfDecks);
        }
        else if (numOfPlayer == 8 && numOfDecks == 3)
        {
            FillDeck(4, numOfDecks);
        }
        else if (numOfPlayer == 4 && numOfDecks == 3)
        {
            FillDeck(8, numOfDecks);
        }
        else if (numOfPlayer == 6 && numOfDecks == 2)
        {
            FillDeck(5, numOfDecks);
        }
        else
        {
            Debug.Log("invalid values are passed in as deck & player numbers");
        }

        _cardsPerHand = _deck.Count / numOfPlayer;
        // foreach (var item in deck) print(item.Key + " -> " + item.Value.Item1 + " , " + item.Value.Item2);
    }

    void ShuffleDeck(Dictionary<int, Tuple<int, int>> deck)
    {
        Dictionary<int, Tuple<int, int>> newDeck = new Dictionary<int, Tuple<int, int>>();
        List<int> keys = Enumerable.Range(1, deck.Count).ToList();

        foreach (var item in deck)
        {
            int p = UnityEngine.Random.Range(0, keys.Count);
            newDeck.Add(keys[p], item.Value);
            keys.RemoveAt(p);
        }

        _deck = newDeck;
    }

    void FillDeck(int floor, int numOfDeck)
    {
        int index = 1;
        while (numOfDeck >= 1)
        {
            for (int suit = 1; suit <= 4; suit++)
            {
                for (int value = 13; value >= floor; value--)
                {
                    _deck[index] = new Tuple<int, int>(suit, value);
                    index++;
                }
            }
            numOfDeck--;
        }
    }

    public Dictionary<int, Tuple<int, int>> GetDeck() => _deck;
    public int GetCardsPerHand() => _cardsPerHand;
}
