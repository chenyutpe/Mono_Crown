using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBuying : MonoBehaviour
{
    // money to buy 1 pack
    const int moneyPerPack = 100;

    [SerializeField] GameObject cannotBuyWindow;
    [SerializeField] GameObject cardsGetWindow;

    void Awake ()
    {
        // need to do, haven't figured out why
        cardsGetWindow.SetActive(true);
        cardsGetWindow.SetActive(false);
    }

    public void Buy ()
    {
        if (UserData.userMoney < moneyPerPack)
        {
            // not enough money
            cannotBuyWindow.SetActive(true);
        }
        else
        {
            // use money
            UserData.UserUseMoney(moneyPerPack);

            // get dummy cards
            CardController[] CardsGetDisplay = cardsGetWindow.GetComponentsInChildren<CardController>();
            
            // 6 cards in a pack
            for (int i = 0; i < 6; ++i)
            {
                // simple random card, 10 is the total types for cards now
                int cardGet = Random.Range(1, 11);
                {
                    UserData.UserGetCard(cardGet); // Add to players owning cards
                }
                // for convenience, set as rival's card
                CardsGetDisplay[i].InitCard(cardGet, GameManager.GamePlayerSide.Rival);
            }

            cardsGetWindow.SetActive(true);
        }
    }
}
