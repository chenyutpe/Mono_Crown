using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttackableGamePlayer : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        // Dragging the attacker
        CardController attacker = eventData.pointerDrag.GetComponent<CardController>();
        
        GamePlayerController defender = GetComponent<GamePlayerController>();
        
        if (attacker == null || defender == null)
        {
            return;
        }
        if (attacker.canAttack)
        {
            GameManager.instance.Battle(attacker, defender);
        }
    }
}
