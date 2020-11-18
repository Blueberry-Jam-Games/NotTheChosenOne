using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDeer : CombatUnit
{
    //Perimative deer ai keeps the deer 1 layer away from the player. If the player advances too much the deer will eventually flee
    public override CombatAction AIResolveAction()
    {
        int averagePosition = 0;
        foreach (CombatUnit cu in manager.player)
        {
            averagePosition += cu.GetDepth();
        }
        averagePosition /= manager.player.Count;

        if(averagePosition- GetDepth() > 1)
        {
            return new ActionAdvance(this, 20, manager); //TODO Speed
        }
        else
        {
            return new ActionWithdraw(this, 20, manager); //TODO Speed
        }
    }

    public override string GetDialogueChoiceTitle()
    {
        Debug.LogError("Deer Script Called Unexpectedly");
        return "";
    }

    public override CombatAction ResolveAction(string question, int selection)
    {
        Debug.LogError("Deer Resolve Action Called Unexpectedly");
        return new ActionNothing(this, manager);
    }
}
