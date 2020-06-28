using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropCardArea : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        // can't put card if already put
        if (GetComponentInChildren<CardController>() == null)
        {
            CardController card = eventData.pointerDrag.GetComponent<CardController>();
            if (card != null)
            {
                // from hand
                if (card.cardPlace == CardController.CardPlace.Hand && card.canUse)
                {
                    // stick to this area
                    card.cardMovement.defaultParent = this.transform;
                    FindObjectOfType<AudioManager>().Play("CardSE1");
                    
                    // used card
                    GameManager.instance.UseCost(card.cost);
                    card.Used();

                    // ability checking
                    if (card.abilities != null)
                    {
                        foreach (Ability ability in card.abilities)
                        {
                            if (ability == Ability.Charge)
                            {
                                card.SetCanAttack();
                            }
                        }
                    }
                }
            }
        }
    }
}
