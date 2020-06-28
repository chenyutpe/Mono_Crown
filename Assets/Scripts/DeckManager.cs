using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DeckManager
{
    // default deck for now
    public static List<int> defaultDeck = new List<int>{1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8,9 ,9, 10, 10};

    public static int DrawFrom (List<int> deck)
    {
        // no card in deck
        if (deck.Count == 0)
        {
            // some handling
            return -1;
        }
        
        // return the drawed card's ID
        int drawedCard = deck[0];
        deck.RemoveAt(0);
        return drawedCard;
    }

    public static void Shuffle (List<int> deck)
    {
        int deckSize = deck.Count;
        for (int i = 0; i < deckSize - 1; ++i)
        {
            int change = Random.Range(i, deckSize);
            // simple swapping
            int temp = deck[i];
            deck[i] = deck[change];
            deck[change] = temp;
        }
    }
}