using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{   
    [SerializeField] Image movementHighlight;
    [SerializeField] Image BG_White;
    [SerializeField] TMPro.TextMeshProUGUI cardNameText;
    [SerializeField] TMPro.TextMeshProUGUI descriptionText;
    [SerializeField] Image illustImage;
    [SerializeField] TMPro.TextMeshProUGUI costText;
    [SerializeField] TMPro.TextMeshProUGUI ATKText;
    [SerializeField] TMPro.TextMeshProUGUI HPText;
    [SerializeField] Image BackSide;

    public void InitDisplay (CardController card)
    {
        // content display
        cardNameText.text = card.cardName;
        descriptionText.text = card.description;
        illustImage.sprite = card.illust;
        costText.text = card.cost.ToString();
        
        // card color
        if (card.color == CardColor.White)
        {
            BG_White.gameObject.SetActive(true);
            cardNameText.color = Color.black;
            descriptionText.color = Color.black;
            costText.color = Color.black;
            ATKText.color = Color.black;
            HPText.color = Color.black;
        }
        else
        {
            BG_White.gameObject.SetActive(false);
            cardNameText.color = Color.white;
            descriptionText.color = Color.white;
            costText.color = Color.white;
            ATKText.color = Color.white;
            HPText.color = Color.white;
        }

        // only monster card has ATK 
        if (card.type == CardType.Monster)
        {
            ATKText.text = card.ATK.ToString();
        }
        else
        {
            ATKText.text = "";
        }

        // only monster card and articraft card has HP
        if (card.type == CardType.Monster || card.type == CardType.Artifact)
        {
            HPText.text = card.HP.ToString();
        }
        else
        {
            HPText.text = "";
        }
    }

    public void UpdateDisplay (CardController card)
    {        
        // only monster card has ATK 
        if (card.type == CardType.Monster)
        {
            ATKText.text = card.ATK.ToString();
            // ATK changed
            if (card.ATK < card.originalATK)
            {
                // show decreased ATK in red text
                ATKText.color = Color.red;
            }
            else if  (card.ATK > card.originalATK)
            {
                // show buffed ATK in blue text
                ATKText.color = Color.blue;
            }
            else
            {
                // show normal ATK in white/black text
                if (card.color == CardColor.White)
                {
                    ATKText.color = Color.black;
                }
                else
                {
                    ATKText.color = Color.white;
                }
            }
        }
        else
        {
            ATKText.text = "";
        }

        // only monster card and articraft card has HP
        if (card.type == CardType.Monster || card.type == CardType.Artifact)
        {
            HPText.text = card.HP.ToString();
            // HP changed
            if (card.HP < card.originalHP)
            {
                // show decreased HP in red text
                HPText.color = Color.red;
            }
            else if  (card.HP > card.originalHP)
            {
                // show buffed HP in blue text
                HPText.color = Color.blue;
            }
            else
            {
                // show normal HP in white/black text
                if (card.color == CardColor.White)
                {
                    HPText.color = Color.black;
                }
                else
                {
                    HPText.color = Color.white;
                }
            }
        }
        else
        {
            HPText.text = "";
        }
        
        // attack finished
        if (!card.canAttack)
        {
            movementHighlight.gameObject.SetActive(false);
        }
    }

    public void DisplayCanAttack ()
    {
        // can attack
        movementHighlight.color = Color.green;
        movementHighlight.gameObject.SetActive(true);
    }

    public void DisplayCanNotAttack ()
    {
        // cannot attack
        movementHighlight.gameObject.SetActive(false);
    }

    public void DisplayCanUse (bool canUse)
    {
        if (canUse)
        {
            movementHighlight.color = Color.cyan;
            movementHighlight.gameObject.SetActive(true);
        }
        else
        {
            movementHighlight.gameObject.SetActive(false);
        }
    }

    public void DisplayInactive ()
    {
        movementHighlight.gameObject.SetActive(false);
    }

    public void ShowCard ()
    {
        BackSide.gameObject.SetActive(false);
    }

    public bool isShowed ()
    {
        return !(BackSide.gameObject.activeSelf);
    }
}