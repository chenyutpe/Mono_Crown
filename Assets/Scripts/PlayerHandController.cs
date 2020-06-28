using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHandController : MonoBehaviour
{
    // only size control (for now)
    public void Expand ()
    {
        GetComponent<RectTransform>().sizeDelta = new Vector2(893.8f, 140.7f);
        transform.localPosition = new Vector3(0f, -289.6f, 0f);
        GetComponent<HorizontalLayoutGroup>().spacing = 20;
    }

    public void Close ()
    {
        GetComponent<RectTransform>().sizeDelta = new Vector2(300f, 140.7f);
        transform.localPosition = new Vector3(-296.5f, -289.6f, 0f);
        GetComponent<HorizontalLayoutGroup>().spacing = -70;
    }
}
