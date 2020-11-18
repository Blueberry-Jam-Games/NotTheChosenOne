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
        DisplayTextAtTitle("ActionGuard");
    }

    public override void ActiveFrame()
    {

    }
}
