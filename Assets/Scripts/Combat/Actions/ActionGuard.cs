using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionGuard : CombatAction
{
    public ActionGuard(CombatUnit user, int speed, CombatManager mngr) : base(user, speed, mngr)
    {

    }

    public override void Execute()
    {
        if(user is CombatUnitPlayable)
        {
            CombatUnitPlayable cup = (CombatUnitPlayable)user;
            DisplayTextAtTitle("ActionGuard");
            cup.Guard();
        }
        Debug.LogError("Non player used player guard action");
    }

    public override void ActiveFrame()
    {

    }
}
