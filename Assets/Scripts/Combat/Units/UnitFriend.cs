using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitFriend : CombatUnit
{
    public override string GetDialogueChoiceTitle()
    {
        return "ChooseFriend";
    }

    public override CombatAction ResolveAction(string question, int selection)
    {
        if(question == "friendAction")
        {
            switch(selection)
            {
                case 0:
                    if (manager.enemy.Count > 1)
                    {
                        DisplayActionTargeting();
                        return null;
                    }
                    else
                    {
                        return new ActionAttack(this, 0, 50, manager);
                    }
                case 1:
                    return new ActionGuard(this, 50, manager); //Speed
                case 2:
                    return new ActionAdvance(this, 50, manager);
                case 3:
                    return new ActionWithdraw(this, 50, manager);
                default:
                    Debug.LogError("Invalid selection for friend");
                    return new ActionNothing(this, manager); //Failsafe to prevent softlock
            }
        }
        else if (question == "target")
        {
            return new ActionAttack(this, selection, 50, manager);
        }
        Debug.LogError("Invalid choice for friend");
        return new ActionNothing(this, manager); //Failsafe to prevent softlock
    }

    public override CombatAction AIResolveAction()
    {
        Debug.LogError("Friend AI Called Unexpectedly");
        return null;
    }

}
