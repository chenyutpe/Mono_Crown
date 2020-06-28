using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnderConstructionObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] GameObject HintWindow;

    bool pointerEntering;
    float timer;

    void Awake ()
    {
        HintWindow.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        pointerEntering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        pointerEntering = false;
        // close Window
        HintWindow.SetActive(false);
    }

    void Update ()
    {
        // If pointer is on the card, start the timer
        if (pointerEntering)
        {
            timer += Time.deltaTime;

            // stay on the object longer than 0.5 second and not dragging, only need open the window once
            if (timer >= 0.5f && !HintWindow.activeSelf)
            {
                HintWindow.SetActive(true);
            }
        }
        else
        {
            // reset timer if exitted
            timer = 0;
        }
    }
}