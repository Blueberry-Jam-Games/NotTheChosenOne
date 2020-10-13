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
                    return new ActionAttack(this, 50, cmRef); //TODO Speed
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
        Debug.LogError("Invalid choice for friend");
        return new ActionNothing(this, cmRef); //Failsafe to prevent softlock
    }

    public override CombatAction AIResolveAction(CombatManager cmRef)
    {
        Debug.LogError("Friend AI Called Unexpectedly");
        return null;
    }

}
