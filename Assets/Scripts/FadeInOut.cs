using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour
{
    [SerializeField] Image fade;
    
    void Start ()
    {
        fade.gameObject.SetActive(true);
        fadeOut();
    }

    // not using now
    void fadeIn ()
    {
        fade.canvasRenderer.SetAlpha(0f);
        fade.CrossFadeAlpha(1, 2, false);
    }

    void fadeOut ()
    {
        fade.canvasRenderer.SetAlpha(1.0f);
        fade.CrossFadeAlpha(0, 2, false);
    }
}