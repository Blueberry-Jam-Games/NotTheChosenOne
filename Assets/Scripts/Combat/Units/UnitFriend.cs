using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitFriend : CombatUnit
{
    public override string GetDialogueChoiceTitle()
    {
        return "ChooseFriend";
    }

    public override CombatAction ResolveAction(string question, int selection, CombatManager cmRef)
    {
        if(question == "friendAction")
        {
            switch(selection)
            {
                case 0:
                    if (cmRef.enemy.Count > 1)
                    {
                        DisplayActionTargeting(cmRef);
                        return null;
                    }
                    else
                    {
                        return new ActionAttack(this, 0, 50, cmRef);
                    }
                case 1:
                    return new ActionGuard(this, 50, cmRef); //Speed
                case 2:
                    return new ActionAdvance(this, 50, cmRef);
                case 3:
                    return new ActionWithdraw(this, 50, cmRef);
                default:
                    Debug.LogError("Invalid selection for friend");
                    return new ActionNothing(this, cmRef); //Failsafe to prevent softlock
            }
        }
        else if (question == "target")
        {
            return new ActionAttack(this, selection, 50, cmRef);
        }
        Debug.LogError("Invalid choice for friend");
        return new ActionNothing(this, cmRef); //Failsafe to prevent softlock
    }

    public override CombatAction AIResolveAction(CombatManager cmRef)
    {
        Debug.LogError("Friend AI Called Unexpectedly");
        return null;
    }

}
