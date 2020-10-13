using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDeer : CombatUnit
{
    //Perimative deer ai keeps the deer 1 layer away from the player. If the player advances too much the deer will eventually flee
    public override CombatAction AIResolveAction(CombatManager cmRef)
    {
        int averagePosition = 0;
        foreach (CombatUnit cu in cmRef.player)
        {
            averagePosition += cu.GetDepth();
        }
        averagePosition /= cmRef.player.Count;

        if(averagePosition- GetDepth() > 1)
        {
            return new ActionAdvance(this, 20, cmRef); //TODO Speed
        }
        else
        {
            return new ActionWithdraw(this, 20, cmRef); //TODO Speed
        }
    }

    public override string GetDialogueChoiceTitle()
    {
        Debug.LogError("Deer Script Called Unexpectedly");
        return "";
    }

    public override CombatAction ResolveAction(string question, int selection, CombatManager cmRef)
    {
        Debug.LogError("Deer Resolve Action Called Unexpectedly");
        return new ActionNothing(this, cmRef);
    }
}
