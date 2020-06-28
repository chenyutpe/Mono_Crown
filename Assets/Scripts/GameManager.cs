using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // helpers to organize the game
    public GameUIManager UIManager;
    // public GameAI EnemyAI; // will add in future update
    [SpaceAttribute]
    
    // elements in the game
    [SerializeField] GamePlayerController playerController;
    [SerializeField] GamePlayerController rivalController;
    [SpaceAttribute]
    [SerializeField] Transform playerDeckZone;
    [SerializeField] Transform rivalDeckZone;
    [SpaceAttribute]
    [SerializeField] Transform playerHand;
    [SerializeField] Transform rivalHand;
    [SerializeField] Transform playerField;
    DropCardArea[] playerFieldSlot;
    [SerializeField] Transform rivalField;
    DropCardArea[] rivalFieldSlot;
    [SpaceAttribute]
    [SerializeField] CardController cardPrefab;
    
    // game status
    public bool isPlayerTurn { get; private set; }
    bool gameSet;
    const int timeLimitInTurn = 60; // 60 second limit

    // for convenience
    public enum GamePlayerSide
    {
        Player, Rival
    }

    public enum GameResult
    {
        Win, Lose, Draw
    }

    // singleton for convenience
    public static GameManager instance;

    void Awake ()
    {
        if (instance == null)
        {
            instance = this;
        }
        // initialize
        isPlayerTurn = true;
        gameSet = false;

        // get the slots to place card
        playerFieldSlot = playerField.GetComponentsInChildren<DropCardArea>();
        rivalFieldSlot = rivalField.GetComponentsInChildren<DropCardArea>();
    }

    void Start ()
    {
        StartCoroutine(GameStart());
    }

    IEnumerator GameStart ()
    {
        // temporary method for character icons
        playerController.SetIcon(GamePlayerSide.Player);
        rivalController.SetIcon(GamePlayerSide.Rival);

        // shuffle the decks at first
        playerController.ShuffleDeck();
        rivalController.ShuffleDeck();

        // decide who goes first
        TurnDetermine();
        StartCoroutine(UIManager.ShowPlayOrder(isPlayerTurn));
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < 7; ++i)
        {
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(1f);
        // Start Game
        
        // Close the start panel
        UIManager.StartPanelOut();

        // give starting cards to hands
        StartCoroutine(FirstHand());
        yield return new WaitForSeconds(1.8f);
        // start the game
        TurnHandler();
    }

    public void Retire ()
    {
        StopAllCoroutines();
        UIManager.ShowResult(GameResult.Lose);
        gameSet = true;
    }

    public void CheckGameSet ()
    {
        if (playerController.Lose())
        {
            StopAllCoroutines();
            if (rivalController.Lose())
            {
                UIManager.ShowResult(GameResult.Draw);
            }
            else
            {
                UIManager.ShowResult(GameResult.Lose);
            }
            gameSet = true;
        }
        else if (rivalController.Lose())
        {
            StopAllCoroutines();
            UIManager.ShowResult(GameResult.Win);
            gameSet = true;
        }
    }

    // Card related processes

    CardController GenerateCard (int cardID, GamePlayerSide owner, Transform place)
    {
        CardController card = Instantiate(cardPrefab, place, false);
        // be sure on the right place
        card.transform.position = card.transform.parent.position;
        // init card data
        card.InitCard(cardID, owner);
        
        // for possible needs
        return card;
    }

    IEnumerator DrawCard (GamePlayerSide gameplayer)
    {
        int drawed; // to store card id
        switch (gameplayer)
        {
            case (GamePlayerSide.Player):
                drawed = playerController.Draw();
                if (drawed == -1) // no card in deck
                {
                    // lose the game 
                    playerController.SetHP(0);
                }
                else
                {
                    CardController card = GenerateCard(drawed, gameplayer, playerDeckZone);
                    
                    // check # (cards in deck)
                    UIManager.DeckDisplay(GamePlayerSide.Player, playerController.CardsInDeck());
                    StartCoroutine(card.cardMovement.MoveToHand(playerHand));
                    yield return new WaitForSeconds(0.3f);
                }
                break;
            case (GamePlayerSide.Rival):
                drawed = rivalController.Draw();
                if (drawed == -1) // no card in deck
                {
                    // lose the game
                    rivalController.SetHP(0);
                }
                else
                {
                    CardController card = GenerateCard(drawed, gameplayer, rivalDeckZone);
                    
                    // check # (cards in deck)
                    UIManager.DeckDisplay(GamePlayerSide.Rival, rivalController.CardsInDeck());
                    StartCoroutine(card.cardMovement.MoveToHand(rivalHand));
                    yield return new WaitForSeconds(0.3f);
                }
                break;
            default:
                drawed = -2; // error case
                break;
        }
    }

    IEnumerator FirstHand()
    {
        // draw 3 cards each
        for (int i = 0; i < 3; ++i)
        {
            StartCoroutine(DrawCard(GamePlayerSide.Player));
            StartCoroutine(DrawCard(GamePlayerSide.Rival));
            yield return new WaitForSeconds(0.3f);
        }
    }
    
    public void UseCost(int cost)
    {
        if (isPlayerTurn)
        {
            playerController.UseCost(cost);

            // see cards in hand still can use or not
            CardController[] playerHandCard = playerHand.GetComponentsInChildren<CardController>();
            foreach (CardController card in playerHandCard)
            {
                card.UpdateCanUse(playerController.usableCost);
            }
        }
        else
        {
            rivalController.UseCost(cost);

            CardController[] rivalHandCard = rivalHand.GetComponentsInChildren<CardController>();
            foreach (CardController card in rivalHandCard)
            {
                card.UpdateCanUse(rivalController.usableCost);
            }
        }
    }
    // Card related processes end
    
    // Turn processes, maybe will make a new class to handle.

    void TurnDetermine ()
    {
        // coin tossing
        isPlayerTurn = (Random.value > 0.5f);
    }

    void TurnChange ()
    {
        // reverse
        isPlayerTurn = !isPlayerTurn;
        TurnHandler();
    }

    public void TurnEnd ()
    {
        // set cards to inactive when turn ended
        if (isPlayerTurn)
        {
            CardController[] playerFieldCard = playerField.GetComponentsInChildren<CardController>();
            foreach (CardController card in playerFieldCard)
            {
                card.SetInactive();
            }

            // Cards in hand with usable cost can use
            CardController[] playerHandCard = playerHand.GetComponentsInChildren<CardController>();
            foreach (CardController card in playerHandCard)
            {
                card.SetInactive();
            }
        }
        else
        {
            CardController[] rivalFieldCard = rivalField.GetComponentsInChildren<CardController>();
            foreach (CardController card in rivalFieldCard)
            {
                card.SetInactive();
            }

            // Cards in hand with usable cost can use
            CardController[] rivalHandCard = rivalHand.GetComponentsInChildren<CardController>();
            foreach (CardController card in rivalHandCard)
            {
                card.SetInactive();
            }
        }
        TurnChange();
    }

    void TurnHandler ()
    {
        StopAllCoroutines();
        if (isPlayerTurn)
        {
            StartCoroutine(playerTurn());
        }
        else
        {
            StartCoroutine(rivalTurn());
        }
    }
    IEnumerator Timer()
    {
        int timeCount = timeLimitInTurn;
        UIManager.UpdateTimer(timeCount);

        while (timeCount > 0)
        {
            yield return new WaitForSeconds(1);
            --timeCount;
            UIManager.UpdateTimer(timeCount);
        }
        // force to change turn
        TurnEnd();
    }

    IEnumerator playerTurn ()
    {
        Debug.Log("Player Turn");
        StartCoroutine(UIManager.PlayerTurnUI());
        yield return new WaitForSeconds(1f);
        playerController.NewTurn();
        StartCoroutine(DrawCard(GamePlayerSide.Player));
        yield return new WaitForSeconds(0.31f);
        CheckGameSet();
        StartCoroutine(Timer());

        if (!gameSet)
        {
            // Check card abilities and skills
            CardController[] playerFieldCard = playerField.GetComponentsInChildren<CardController>();
            foreach (CardController card in playerFieldCard)
            {
               if (card.skills.Count > 0)
               {
                   foreach (Skill skill in card.skills)
                   {
                       if (skill.skillTiming == SkillTiming.BeginningOfTurn)
                       {
                           SkillManager.UseSkill(card, skill, playerController, playerField, rivalController, rivalField);
                       }
                   }
               }
            }

            // Cards on field become attackable
            playerFieldCard = playerField.GetComponentsInChildren<CardController>();
            foreach (CardController card in playerFieldCard)
            {
                // only monsters can attack
                if (card.type == CardType.Monster)
                {
                    card.SetCanAttack();
                }
            }

            // Cards in hand with usable cost can use
            CardController[] playerHandCard = playerHand.GetComponentsInChildren<CardController>();
            foreach (CardController card in playerHandCard)
            {
                card.UpdateCanUse(playerController.usableCost);
            }
        }
    }

    IEnumerator rivalTurn ()
    {
        Debug.Log("Rival Turn");
        StartCoroutine(UIManager.RivalTurnUI());
        yield return new WaitForSeconds(1f);
        rivalController.NewTurn();
        StartCoroutine(DrawCard(GamePlayerSide.Rival));
        yield return new WaitForSeconds(0.3f);
        CheckGameSet();
        StartCoroutine(Timer());

        if (!gameSet)
        {
            // Check card abilities and skills
            CardController[] rivalFieldCard = rivalField.GetComponentsInChildren<CardController>();
            foreach (CardController card in rivalFieldCard)
            {
               if (card.skills.Count > 0)
               {
                   foreach (Skill skill in card.skills)
                   {
                       if (skill.skillTiming == SkillTiming.BeginningOfTurn)
                       {
                           SkillManager.UseSkill(card, skill, rivalController, rivalField, playerController, playerField);
                       }
                   }
               }
            }

            // get cards on field
            rivalFieldCard = rivalField.GetComponentsInChildren<CardController>();
            foreach (CardController card in rivalFieldCard)
            {
                // only monsters can attack
                if (card.type == CardType.Monster)
                {
                    card.SetCanAttack();
                }
            }
            
            // get cards in hand
            CardController[] rivalHandCard = rivalHand.GetComponentsInChildren<CardController>();
            
            foreach (CardController card in rivalHandCard)
            {
                card.UpdateCanUse(rivalController.usableCost);
            }

            yield return new WaitForSeconds(1);
            
            foreach (CardController useCard in rivalHandCard)
            {
                if (useCard.canUse)
                {
                    // set card to field
                    foreach (DropCardArea slot in rivalFieldSlot)
                    {
                        // empty slot
                        if (slot.GetComponentInChildren<CardController>() == null)
                        {
                            // Card to set on fields
                            if (useCard.type == CardType.Monster || useCard.type == CardType.Artifact)
                            {
                                StartCoroutine(useCard.cardMovement.MoveToField(slot.transform));
                                UseCost(useCard.cost);
                                useCard.Used();
                                yield return new WaitForSeconds(1);

                                // ability checking
                                if (useCard.abilities != null)
                                {
                                    foreach (Ability ability in useCard.abilities)
                                    {
                                        if (ability == Ability.Charge)
                                        {
                                            useCard.SetCanAttack();
                                        }
                                    }
                                }
                                
                                break;
                            }
                        }
                    }
                }
            }
            
            // Battle phase
            yield return new WaitForSeconds(1);
            // choose cards to attack
            rivalFieldCard = rivalField.GetComponentsInChildren<CardController>();
            if (rivalFieldCard.Length > 0)
            {
                foreach (CardController atker in rivalFieldCard)
                {
                    if (atker.canAttack)
                    {
                        yield return new WaitForSeconds(1);
                        CardController[] playerFieldCard = playerField.GetComponentsInChildren<CardController>();
                        if (playerFieldCard.Length > 0)
                        {
                            // choose left-most card and attack
                            CardController dfder = playerFieldCard[0];
                            StartCoroutine(atker.cardMovement.MoveToTarget(dfder.transform));
                            yield return new WaitForSeconds(0.41f);
                            Battle(atker, dfder);
                        }
                        else    // no cards on field
                        {
                            // Go face
                            StartCoroutine(atker.cardMovement.MoveToTarget(playerController.transform));
                            yield return new WaitForSeconds(0.41f);
                            Battle(atker, playerController);
                        }
                        // need to wait for some frame problem or the destroyed card will still be detected.
                        yield return new WaitForEndOfFrame();
                    }
                }
            }
            yield return new WaitForSeconds(1);
            TurnEnd();
        }
    }

    // Turn processes ends


    // Battle processes

    // overloading
    public void Battle (CardController attacker, CardController defender)
    {
        Debug.Log(attacker.cardName + " attacked "+ defender.cardName);
        attacker.Attack(defender);
        CheckGameSet();
    }

    public void Battle (CardController attacker, GamePlayerController gamePlayer)
    {
        Debug.Log(attacker.cardName + " attacked face");
        attacker.Attack(gamePlayer);
        CheckGameSet();
    }
    // Battle processes ends

}