using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// a temporary method(Drag attacker card onto Defender card), want to change later
public class AttackableCard : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        // Dragging the attacker
        CardController attacker = eventData.pointerDrag.GetComponent<CardController>();
        
        CardController defender = GetComponent<CardController>();
        
        if (attacker == null || defender == null)
        {
            return;
        }

        // can't attack ally
        if (attacker.Owner == defender.Owner)
        {
            return;
        }

        if (attacker.canAttack)
        {
            GameManager.instance.Battle(attacker, defender);
        }
    }
}
