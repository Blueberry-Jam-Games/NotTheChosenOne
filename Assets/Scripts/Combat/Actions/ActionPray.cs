using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPray : CombatAction
{
    public ActionPray(CombatUnit user, int speed, CombatManager mngr) : base(user, speed, mngr)
    {

    }

    public override void Execute()
    {
        if (user is CombatUnitPlayable)
        {
            CombatUnitPlayable cup = (CombatUnitPlayable)user;
            DisplayTextAtTitle("ActionPray");
            cup.UpdateTension(cup.GetTension() * 0.8f);
        }
    }

    public override void ActiveFrame()
    {

    }
}
