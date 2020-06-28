using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    // too many cases to consider, just implement what we need right now
    public static void UseSkill(CardController card, Skill skill,
                         GamePlayerController player, Transform playerField, 
                         GamePlayerController rival, Transform rivalField)
    {
        switch (skill.skillType)
        {
            case (SkillType.Damage):
                
                switch (skill.skillObject)
                {
                    case (SkillObject.RandomEnemyCharcter):
                        CardController[] rivalFieldCard = rivalField.GetComponentsInChildren<CardController>();
                        
                        for (int i = 0 ; i < skill.numOfObjectives ; ++i)
                        {
                            int obj = Random.Range(0, rivalFieldCard.Length+1); // including rival player
                            Debug.Log("" + obj);
                            if (obj == rivalFieldCard.Length)
                            {
                                // Attack rival
                                rival.Damaged(skill.amount);
                            }
                            else
                            {
                                // Attack enemy units
                                rivalFieldCard[obj].Damaged(skill.amount);
                            }
                        }
                        break;
                    case (SkillObject.RivalMaster):
                        rival.Damaged(skill.amount);
                        break;
                    default:
                        break;
                }
                break;
            case (SkillType.Heal):
                switch (skill.skillObject)
                {
                    case (SkillObject.AllFriendlyUnits):
                        CardController[] playerFieldCard = playerField.GetComponentsInChildren<CardController>();
                        
                        foreach (CardController unit in playerFieldCard)
                        {
                            unit.Healed(skill.amount);
                        }
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }
    }
}
