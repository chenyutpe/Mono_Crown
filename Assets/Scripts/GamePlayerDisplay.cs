using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayerDisplay : MonoBehaviour
{
    // Character Icon
    [SerializeField] Image icon;

    // HP
    [SerializeField] TMPro.TextMeshProUGUI HPText;

    // Cost
    [SerializeField] GameObject CostArea;
    Image[] CostDiamonds;

    void Awake ()
    {
        CostDiamonds = CostArea.GetComponentsInChildren<Image>();
    }

    public void UpdateDisplay (GamePlayerController gamePlayer)
    {
        icon.sprite = gamePlayer.icon;
        HPText.text = gamePlayer.HP.ToString();
        
        // HP changed
        if (gamePlayer.HP < gamePlayer.originalHP)
        {
            // show decreased HP in red text
            HPText.color = Color.red;
        }
        else if  (gamePlayer.HP > gamePlayer.originalHP)
        {
            // show buffed HP in blue text
            HPText.color = Color.blue;
        }
        else
        {
            // show normal HP in white text
            HPText.color = Color.white;
        }
    }

    public void UpdateCost (GamePlayerController gamePlayer)
    {
        // iterater
        int i = 0;

        // Filled diamond if can use
        for (; i < gamePlayer.usableCost; ++i)
        {
            CostDiamonds[i].sprite = Resources.Load<Sprite>("Icon/diamond");
        }

        // Empty diamond if can't use
        for (; i < GamePlayerController.maxCost; ++i)
        {
            CostDiamonds[i].sprite = Resources.Load<Sprite>("Icon/diamond_frame");
        }
    }
}
