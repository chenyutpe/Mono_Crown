using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class CardController : MonoBehaviour
{
    // card componets
    CardDisplay cardDisplay;
    public CardMovement cardMovement { get; private set; }
    
    // basic data
    public int ID { get; private set; }
    public CardType type { get; private set; }
    public CardColor color { get; private set; }
    public string cardName { get; private set; }
    public string description { get; private set; }
    public Sprite illust { get; private set; }
    public int cost { get; private set; }
    public int ATK { get; private set; }
    public int HP { get; private set; }
    public List<Ability> abilities { get; private set; }
    public List<Skill> skills { get; private set; }
    
    // to compare with
    public int originalATK { get; private set; }
    public int originalHP { get; private set; }

    // other status
    public GameManager.GamePlayerSide Owner { get; private set; }
    public bool canAttack { get; private set; }
    public bool canUse { get; private set; }    // from hand to field
    public enum CardPlace
    {
        Hand, Field
    }
    public CardPlace cardPlace { get; private set; }

    void Awake ()
    {
        cardDisplay = GetComponent<CardDisplay>();
        cardMovement = GetComponent<CardMovement>();
        canAttack = false;
        canUse = false;
        cardPlace = CardPlace.Hand; // should go to hand first
    }

    public void InitCard(int cardID, GameManager.GamePlayerSide owner)
    {
        CardEntity cardEntity = Resources.Load<CardEntity>("CardEntityDatabase/Card" + cardID);
        
        ID = cardEntity.id;
        type = cardEntity.type;
        color = cardEntity.color;
        cardName = cardEntity.cardName;
        description = cardEntity.description;
        illust = cardEntity.illust;
        cost = cardEntity.cost;
        ATK = cardEntity.ATK;
        HP = cardEntity.HP;

        abilities = cardEntity.abilities.ConvertAll(ability => ability);
        skills = cardEntity.skills.ConvertAll(skill => skill);
        
        originalATK = cardEntity.ATK;
        originalHP = cardEntity.HP;
        this.Owner = owner;
        
        // no need to cover if its player's
        if (owner == GameManager.GamePlayerSide.Player)
        {
            ShowCard();
        }

        cardDisplay.InitDisplay(this);
    }

    public void Attack (CardController defender)
    {
        canAttack = false;

        int counterDamage = defender.ATK;
        defender.Damaged(this.ATK);
        this.Damaged(counterDamage);
    }
    
    // overloading
    public void Attack (GamePlayerController gamePlayer)
    {
        canAttack = false;

        gamePlayer.Damaged(this.ATK);
        
        this.UpdateStatus();
    }

    public void Damaged (int damage)
    {
        HP -= damage;
        // HP won't be negative
        HP = (HP < 0 ? 0 : HP);
        FindObjectOfType<AudioManager>().Play("DamagedSE1");
        UpdateStatus();
    }

    public void Healed (int heal)
    {
        HP += heal;
        // can't heal over max HP
        HP = (HP > originalHP ? originalHP : HP);
        FindObjectOfType<AudioManager>().Play("healSE1");
        UpdateStatus();
    }

    void UpdateStatus ()
    {
        cardDisplay.UpdateDisplay(this);
        if (HP == 0)
        {
            // send to Graveyard, maybe will store this later.
            Destroy(this.gameObject);
        }
    }

    public void SetCanAttack ()
    {
        // become attackable
        canAttack = true;
        cardDisplay.DisplayCanAttack();
    }

    public void UpdateCanUse (int usableCost)
    {
        if (cost <= usableCost)
        {
            canUse = true;
        }
        else
        {
            canUse = false;
        }

        // no need to see rival's useable card
        if (Owner == GameManager.GamePlayerSide.Player)
        {
            cardDisplay.DisplayCanUse(canUse);
        }
    }

    public void Used ()
    {
        canUse = false;
        cardPlace = CardPlace.Field;
        cardDisplay.DisplayInactive();
    }

    public void SetInactive ()
    {
        canUse = false;
        canAttack = false;
        cardDisplay.DisplayInactive();
    }

    public void ShowCard ()
    {
        cardDisplay.ShowCard();
    }

    public bool isShowed ()
    {
        return cardDisplay.isShowed();
    }
}