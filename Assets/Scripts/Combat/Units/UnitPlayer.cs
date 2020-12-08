using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPlayer : CombatUnitPlayable
{
    public GameObject arrowRef;

    public override string GetDialogueChoiceTitle()
    {
        return "ChooseAction";
    }

    public override CombatAction ResolveAction(string question, int selection)
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
                    return new ActionNothing(this, manager); //Prevents soft lock
                case 2:
                    Debug.Log("Advance");
                    return new ActionAdvance(this, 100, manager); //TODO Speed
                case 3:
                    Debug.Log("Withdraw");
                    return new ActionWithdraw(this, 100, manager); //TODO Speed
                default:
                    Debug.LogError("Invalid choice for player");
                    return new ActionNothing(this, manager); //Failsafe to prevent softlock
            }
        }
        else if (question == "skill")
        {
            switch (selection)
            {
                case 0:
                    //Targeting
                    if (manager.enemy.Count > 1)
                    {
                        DisplayActionTargeting();
                        return null;
                    } 
                    else
                    {
                        return new ActionAttack(this, 0, 150, manager);
                    }
                case 1:
                    return new ActionGuard(this, 150, manager); //TODO Speed
                case 2:
                    return new ActionPray(this, 150, manager); //TODO Speed
                case 3:
                    return null; //Sub-menue returns null
                default:
                    Debug.LogError("Invalid choice for player");
                    return new ActionNothing(this, manager); //Failsafe to prevent softlock
            }
        }
        else if (question == "target")
        {
            return new ActionAttack(this, selection, 150, manager); //TODO Speed
        }
        else
        {
            Debug.LogError("Class processing incorrect choice");
            return new ActionNothing(this, manager); //Failsafe to prevent softlock
        }
    }
}
