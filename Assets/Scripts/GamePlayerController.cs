using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayerController : MonoBehaviour
{
    GamePlayerDisplay display;
    List<int> deck;

    public Sprite icon { get; private set; }
    public int HP { get; private set; }
    public int originalHP { get; private set; }
    
    public int usableCost { get; private set; }
    public const int maxCost = 9;
    int turnCount;

    void Awake ()
    {
        display = GetComponent<GamePlayerDisplay>();
        usableCost = 0;
        turnCount = 0;
    }

    void Start ()
    {
        Init ();
    }

    void Init ()
    {
        // Clone deck data from DeckManager
        deck = DeckManager.defaultDeck.ConvertAll(id => id);

        // Initialize the status
        originalHP = 20;
        HP = 20;
        // icon = ... no image now

        display.UpdateDisplay(this);
    }

    // temporary method, hope to create a entity for character in further update
    public void SetIcon(GameManager.GamePlayerSide playerSide)
    {
        switch (playerSide)
        {
            case (GameManager.GamePlayerSide.Player) :
                icon = Resources.Load<Sprite>("Images/CharacterIcon/MCIcon");
                break;
            case (GameManager.GamePlayerSide.Rival) :
                icon = Resources.Load<Sprite>("Images/CharacterIcon/badguyIcon");
                break;
            default:
                break;
        }
        display.UpdateDisplay(this);
    }

    // card related
    public int Draw ()
    {
        return DeckManager.DrawFrom(deck);
    }

    public void ShuffleDeck ()
    {
        DeckManager.Shuffle(deck);
    }

    public int CardsInDeck ()
    {
        return deck.Count;
    }
    // card related end

    // status related
    public void Damaged (int damage)
    {
        HP -= damage;
        // HP won't be negative
        HP = (HP < 0 ? 0 : HP);
        FindObjectOfType<AudioManager>().Play("DamagedSE1");
        UpdateStatus();
    }

    public void SetHP (int value)
    {
        HP = value;
        UpdateStatus();
    }

    public void UpdateStatus ()
    {
        display.UpdateDisplay(this);
    }

    public bool Lose ()
    {
        return (HP == 0);
    }
    // status related end

    // cost related
    void NewTurnCost ()
    {
        usableCost = (turnCount <= maxCost ? turnCount : maxCost);
        display.UpdateCost(this);
    }

    public void UseCost (int cost)
    {
        usableCost -= cost;
        display.UpdateCost(this);
    }
    // cost related end

    public void NewTurn ()
    {
        ++turnCount;
        NewTurnCost();
        UpdateStatus();
    }
}
