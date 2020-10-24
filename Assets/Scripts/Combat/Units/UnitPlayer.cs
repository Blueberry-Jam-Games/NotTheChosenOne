using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPlayer : CombatUnit
{
    public override string GetDialogueChoiceTitle()
    {
        return "ChooseAction";
    }

    public override CombatAction ResolveAction(string question, int selection, CombatManager cmRef)
    {
        Debug.Log("Resolving player action");
        if (question == "action")
        {
            switch (selection)
            {
                case 0:
                    return null; //Null because sub-menue accessed
                case 1:
                    Debug.LogError("Inventorty accessed");
                    return new ActionNothing(this, cmRef); //Prevents soft lock
                case 2:
                    Debug.Log("Advance");
                    return new ActionAdvance(this, 100, cmRef); //TODO Speed
                case 3:
                    Debug.Log("Withdraw");
                    return new ActionWithdraw(this, 100, cmRef); //TODO Speed
                default:
                    Debug.LogError("Invalid choice for player");
                    return new ActionNothing(this, cmRef); //Failsafe to prevent softlock
            }
        }
        else if (question == "skill")
        {
            switch (selection)
            {
                case 0:
                    //return new ActionAttack(this, 150, cmRef); //TODO Speed
                    //Targeting
                    if (cmRef.enemy.Count > 1)
                    {
                        DisplayActionTargeting(cmRef);
                        return null;
                    } 
                    else
                    {
                        return new ActionAttack(this, 0, 150, cmRef);
                    }
                case 1:
                    return new ActionGuard(this, 150, cmRef); //TODO Speed
                case 2:
                    return new ActionPray(this, 150, cmRef); //TODO Speed
                case 3:
                    return null; //Sub-menue returns null
                default:
                    Debug.LogError("Invalid choice for player");
                    return new ActionNothing(this, cmRef); //Failsafe to prevent softlock
            }
        }
        else if (question == "target")
        {
            return new ActionAttack(this, selection, 150, cmRef); //TODO Speed
        }
        else
        {
            Debug.LogError("Class processing incorrect choice");
            return new ActionNothing(this, cmRef); //Failsafe to prevent softlock
        }
    }

    public override CombatAction AIResolveAction(CombatManager cmRef)
    {
        Debug.LogError("Player AI function called unexpectedly");
        return null;
    }
}
