using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gallery : MonoBehaviour
{
    CardController[] CardsDisplay;

    void Start()
    {
        CardsDisplay = GetComponentsInChildren<CardController>();
        ShowAllCards();
    }

    void ShowAllCards ()
    {
        for (int i = 0; i < CardsDisplay.Length; ++i)
        {
            // for convenience, set as rival's card
            CardsDisplay[i].InitCard(i+1, GameManager.GamePlayerSide.Rival);
        }
    }
}
