using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameUIManager : MonoBehaviour
{
    // Start Panel
    [SerializeField] GameObject startPanel;
    [SerializeField] Image startPanel_PlayerIcon;
    [SerializeField] TMPro.TextMeshProUGUI playerName;
    [SerializeField] Image startPanel_RivalIcon;
    [SerializeField] GameObject startPanel_playerOrder;
    [SerializeField] GameObject startPanel_card1;
    [SerializeField] GameObject startPanel_card2;
    [SerializeField] GameObject startPanel_resultCard;
    [SerializeField] GameObject startPanel_resultCard_BackSide;
    [SerializeField] TMPro.TextMeshProUGUI startPanel_resultCardText;
    [SpaceAttribute]

    // Game Interacting Area objects
    [SerializeField] GameObject blankCardPrefab;
    [SpaceAttribute]

    // Deck
    [SerializeField] TMPro.TextMeshProUGUI PlayerDeckCountText;
    [SerializeField] GameObject PlayerDummyCard;
    [SerializeField] GameObject RivalDummyCard;
    [SpaceAttribute]

    // Timer
    [SerializeField] TMPro.TextMeshProUGUI timerText;
    [SpaceAttribute]
    
    // Turn Button
    [SerializeField] GameObject RivalTurnMask;
    [SpaceAttribute]
    
    // CardInfoWindow
    [SerializeField] GameObject CardInfoWindow;
    [SerializeField] CardController CardToShow;
    [SpaceAttribute]

    // Turn Panel
    [SerializeField] GameObject TurnPanel;
    [SerializeField] TMPro.TextMeshProUGUI TurnPanelText;
    [SpaceAttribute]

    // Result Panel
    [SerializeField] GameObject resultPanel;
    [SerializeField] TMPro.TextMeshProUGUI resultText;

    public bool cardInfoWindow_Opened { get; private set; }

    void Awake ()
    {
        cardInfoWindow_Opened = false;
        StartPanelSetUP();
    }

    void Start ()
    {
        playerName.text = UserData.userName;
        startPanel.SetActive(true);
        startPanel_resultCard.SetActive(false);
        startPanel_playerOrder.SetActive(false);
        PlayerDummyCard.SetActive(true);
        RivalDummyCard.SetActive(true);
        CardInfoWindow.SetActive(true); // need to be active at first to use, haven't figured out why
        CardInfoWindow.SetActive(false);
        TurnPanel.SetActive(false);
        resultPanel.SetActive(false);
    }

    public void StartPanelSetUP ()
    {
        startPanel_PlayerIcon.sprite = Resources.Load<Sprite>("Images/CharacterIcon/MCIcon");
        startPanel_RivalIcon.sprite = Resources.Load<Sprite>("Images/CharacterIcon/badguyIcon");
    }

    // show the who goes first at the beginning of the game
    public IEnumerator ShowPlayOrder (bool playerFirst)
    {
        yield return new WaitForSeconds(1f);
        startPanel_playerOrder.SetActive(true);

        // Shuffle Animation 
        startPanel_card1.transform.DOLocalMoveX(170, 0.1f);
        startPanel_card2.transform.DOLocalMoveX(-170, 0.1f);
        FindObjectOfType<AudioManager>().Play("CardSE2");
        yield return new WaitForSeconds(0.1f);
        startPanel_card2.transform.SetAsFirstSibling();
        startPanel_card1.transform.DOLocalMoveX(0, 0.1f);
        startPanel_card2.transform.DOLocalMoveX(0, 0.1f);
        yield return new WaitForSeconds(0.1f);

        startPanel_card2.transform.DOLocalMoveX(170, 0.1f);
        startPanel_card1.transform.DOLocalMoveX(-170, 0.1f);
        FindObjectOfType<AudioManager>().Play("CardSE2");
        yield return new WaitForSeconds(0.1f);
        startPanel_card1.transform.SetAsFirstSibling();
        startPanel_card1.transform.DOLocalMoveX(0, 0.1f);
        startPanel_card2.transform.DOLocalMoveX(0, 0.1f);
        yield return new WaitForSeconds(0.2f);

        // result
        startPanel_resultCard.gameObject.SetActive(true);
        if (playerFirst)
        {
            startPanel_resultCardText.text = "Play\nFirst";
        }
        else
        {
            startPanel_resultCardText.text = "Draw\nFirst";
        }
        startPanel_resultCard.transform.DOScaleX(0, 0.1f);
        FindObjectOfType<AudioManager>().Play("CardSE2");
        yield return new WaitForSeconds(0.1f);
        startPanel_resultCard_BackSide.gameObject.SetActive(false);
        startPanel_resultCard.transform.DOScaleX(2.6743f, 0.1f);
        yield return new WaitForSeconds(0.1f);
        yield return new WaitForSeconds(1f);
    }

    public void StartPanelOut ()
    {
        // simple inactive for now
        startPanel.SetActive(false);
    }

    // Add a space when card is moving 
    public GameObject AddBlankCard (Transform area)
    {
        return Instantiate(blankCardPrefab, area, false);
    }

    // Remove the blank card after the card is at place
    public void RemoveBlankCard (GameObject blankCard)
    {
        // put out
        blankCard.transform.SetParent(blankCard.transform.parent.parent);
        // destroy the blank card
        Destroy(blankCard);
    }

    // Change view accroding to the # (cards in deck)
    public void DeckDisplay (GameManager.GamePlayerSide side, int num)
    {
        // disable dummy card (show death card) if no card in deck
        switch (side)
        {
            case (GameManager.GamePlayerSide.Player):
                if (num == 0)
                {
                    PlayerDummyCard.SetActive(false);
                }
                else if (!PlayerDummyCard.activeSelf)
                {
                    PlayerDummyCard.SetActive(true);
                }
                PlayerDeckCountText.text = num.ToString();
                break;
            case (GameManager.GamePlayerSide.Rival):
                if (num == 0)
                {
                    RivalDummyCard.SetActive(false);
                }
                else if (!RivalDummyCard.activeSelf)
                {
                    RivalDummyCard.SetActive(true);
                }
                break;
            default:
                break;
        }
    }


    public void UpdateTimer (int timer)
    {
        timerText.text = timer.ToString();
        if (timer <= 10)
        {
            timerText.color = Color.red;
        }
        else
        {
            timerText.color = Color.white;
        }
    }

    public IEnumerator PlayerTurnUI ()
    {
        // remove mask while player's turn
        RivalTurnMask.SetActive(false);

        // Show turn cut in
        TurnPanelText.text = "YOUR TURN";
        TurnPanel.SetActive(true);
        
        yield return new WaitForSeconds(1f);

        // Close turn cut in
        TurnPanel.SetActive(false);
    }

    public IEnumerator RivalTurnUI ()
    {
        // put mask on turn change button while rival's turn
        RivalTurnMask.SetActive(true);

        // Show turn cut in
        TurnPanelText.text = "RIVAL'S TURN";
        TurnPanel.SetActive(true);

        yield return new WaitForSeconds(1f);

        // Close turn cut in
        TurnPanel.SetActive(false);
    }

    public void ShowCardInfoWindow (bool show, CardController card)
    {
        if (show)
        {
            // for convenience, set as rival's card
            CardToShow.InitCard(card.ID, GameManager.GamePlayerSide.Rival);
        }
        CardInfoWindow.SetActive(show);
        cardInfoWindow_Opened = show;
    }

    public void ShowResult (GameManager.GameResult gameResult)
    {
        string BGM = "";
        switch (gameResult)
        {
            case (GameManager.GameResult.Win) :
                resultText.text = "WIN";
                BGM = "WinningBGM";
                break;
            case (GameManager.GameResult.Lose) :
                resultText.text = "LOSE";
                BGM = "LosingBGM";
                break;
            case (GameManager.GameResult.Draw) :
                resultText.text = "DRAW";
                break;
            default:
                break;
        }
        resultPanel.SetActive(true);
        FindObjectOfType<AudioManager>().Stop("BattleBGM");
        FindObjectOfType<AudioManager>().Play(BGM);
    }
}
