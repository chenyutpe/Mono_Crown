using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New CardEntity", menuName = "Card Entity")]
public class CardEntity : ScriptableObject
{
    public int id;
    public CardType type;
    public CardColor color;
    public string cardName;
    public string description;
    public Sprite illust;
    public int cost;
    public int ATK;
    public int HP;
    public List<Ability> abilities;
    public List<Skill> skills;
}

public enum CardType
{
    Monster, Artifact, Spell
}

public enum CardColor
{
    Black, White
}

// Abilities are more about some fixed features
public enum Ability
{
    Charge
}

// Skills are more unique and specified features

public enum SkillType
{
    Damage, 
    Heal, 
    AddHP,
    AddATK, 
    CutHP, 
    CutATK
}

public enum SkillTiming
{
    Instant, BeginningOfTurn, EndOfTurn, OnSummon, OnDeath, Always
}

public enum SkillObject
{
    Self, Master, RivalMaster,
    AllCharacter, AllUnits, AllMonsters, AllArtifacts,
    AllFriendlyCharacter, AllFriendlyUnits, AllFriendlyMonsters, AllFriendlyArtifacts,
    AllEnemyCharacter, AllEnemyUnits, AllEnemyMonsters, AllEnemyArtifacts,
    RandomCharacter, RandomUnits, RandomMonsters, RandomArtifacts, 
    RandomFriendlyCharacter, RandomFriendlyUnits, RandomFriendlyMonsters, RandomFriendlyArtifacts, 
    RandomEnemyCharcter, RandomEnemyUnits, RandomEnemyMonsters, RandomEnemyArtifacts,
    SpecificCharacter, SpecificUnit, SpecificMonster, SpecificArtifact,
    AllBlackUnits, AllWhiteUnits, 
}

[System.Serializable]
public struct Skill
{
    public SkillType skillType;
    public SkillTiming skillTiming;
    public int amount; // for skills with amounts
    public SkillObject skillObject;
    public int numOfObjectives; // for skills needs it
}