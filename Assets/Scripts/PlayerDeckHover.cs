using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerDeckHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] GameObject PlayerDeckWindow;

    bool pointerEntering;
    float timer;

    public void OnPointerEnter(PointerEventData eventData)
    {
        pointerEntering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        pointerEntering = false;
        // close Window
        PlayerDeckWindow.SetActive(false);
    }

    void Update ()
    {
        // If pointer is on the card, start the timer
        if (pointerEntering)
        {
            timer += Time.deltaTime;

            // stay on the card longer than 0.5 second and not dragging, only need open the info window once
            if (timer >= 0.5f && !PlayerDeckWindow.activeSelf)
            {
                PlayerDeckWindow.SetActive(true);
            }
        }
        else
        {
            // reset timer if exitted
            timer = 0;
        }
    }
}
