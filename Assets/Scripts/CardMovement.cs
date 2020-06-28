using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class CardMovement : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Transform defaultParent;
    int cardOrder;

    CardController card;    // this card
    RectTransform rt;   // of this card
    
    bool isDragable;
    bool isDragging;
    bool pointerEntering;
    float timer;

    void Awake ()
    {
        defaultParent = transform.parent;
        card = GetComponent<CardController>();
        rt = GetComponent<RectTransform>();
        isDragable = false;
        isDragging = false;
        pointerEntering = false;
        timer = 0;
    }

    // dragging-related
    public void OnBeginDrag(PointerEventData eventData)
    {
        // can't use rival's card
        if (card.Owner == GameManager.GamePlayerSide.Rival)
        {
            isDragable = false;
            return;
        }
        
        // not a card can use or attack
        if (!card.canUse && !card.canAttack)
        {
            isDragable = false;
            return;
        }
        
        isDragable = true;

        isDragging = true;

        // move out from the field and can go back if not dropped
        cardOrder = transform.GetSiblingIndex(); 
        transform.SetParent(defaultParent.parent.parent);

        // to sense things under the card
        GetComponent<CanvasGroup>().blocksRaycasts = false;

        // follow mouse
        SetDraggedPosition(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragable)
        {
            return;
        }

        // follow mouse
        SetDraggedPosition(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDragable)
        {
            return;
        }

        // go back if not dropped
        transform.SetParent(defaultParent);
        transform.SetSiblingIndex(cardOrder); 
    
        // reset raycast
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        
        isDragging = false;
    }

    // offical method
    void SetDraggedPosition (PointerEventData eventData)
    {
        Vector3 globalMousePos;
		if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, eventData.position, eventData.pressEventCamera, out globalMousePos))
		{
			rt.position = globalMousePos;
		}
    }
    // dragging-related end

    // hover event
    public void OnPointerEnter(PointerEventData eventData)
    {
        // can't look backside card
        if (card.isShowed())
        {
            pointerEntering = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        pointerEntering = false;
        // close CardInfoWindow
        GameManager.instance.UIManager.ShowCardInfoWindow(false, card);
    }

    void Update ()
    {
        // If pointer is on the card, start the timer
        if (pointerEntering)
        {
            timer += Time.deltaTime;

            // stay on the card longer than 0.5 second and not dragging, only need open the info window once
            if (timer >= 0.5f && !isDragging && !GameManager.instance.UIManager.cardInfoWindow_Opened)
            {
                card = GetComponent<CardController>();
                GameManager.instance.UIManager.ShowCardInfoWindow(true, card);
            }
        }
        else
        {
            // reset timer if exitted
            timer = 0;
        }
    }
    // hover event end

    // movement animation

    public IEnumerator MoveToHand (Transform hand)
    {
        // move out from deck
        transform.SetParent(defaultParent.parent);

        // blank card for the position
        GameObject blankCard = GameManager.instance.UIManager.AddBlankCard(hand);
        
        // moving with DOTween
        transform.DOMove(hand.position, 0.30f);
        FindObjectOfType<AudioManager>().Play("CardSE2");
        //transform.DOMove(blankCard.transform.position, 0.30f);
        
        // flip if is player's card
        if (card.Owner == GameManager.GamePlayerSide.Player)
        {
            transform.DOScaleX(0, 0.15f);
            yield return new WaitForSeconds(0.15f);
            card.ShowCard();
            transform.DOScaleX(1, 0.15f);
            yield return new WaitForSeconds(0.15f);
        }
        else
        {
            yield return new WaitForSeconds(0.30f);
        }

        // remove the blank card
        GameManager.instance.UIManager.RemoveBlankCard(blankCard);

        // go to hand
        defaultParent = hand;
        transform.SetParent(defaultParent);
    }

    public IEnumerator MoveToField (Transform field)
    {
        // move out from hand
        transform.SetParent(defaultParent.parent);
        
        // moving and flipping animation with DOTween
        transform.DOMove(field.position, 0.30f);
        transform.DOScaleX(0, 0.15f);
        yield return new WaitForSeconds(0.15f);
        card.ShowCard();
        transform.DOScaleX(1, 0.15f);
        yield return new WaitForSeconds(0.15f);
        
        // go on field
        defaultParent = field;
        transform.SetParent(defaultParent);
        FindObjectOfType<AudioManager>().Play("CardSE1");
    }

    public IEnumerator MoveToTarget (Transform target)
    {
        Vector3 originalPosition = transform.position;
        transform.SetParent(defaultParent.parent.parent);

        // moving animation with DOTween
        transform.DOMove(target.position, 0.15f);
        yield return new WaitForSeconds(0.15f);

        // going back, animation with DOTween
        transform.DOMove(originalPosition, 0.25f);
        yield return new WaitForSeconds(0.25f);

        transform.SetParent(defaultParent);
    }
    // movement animation end
}