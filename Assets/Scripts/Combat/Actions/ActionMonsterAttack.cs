using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionMonsterAttack : CombatAction
{
    private int target = 0; //Yep, lazieness hurt someone

    public ActionMonsterAttack(CombatUnit user, int speed, CombatManager mngr) : base(user, speed, mngr)
    {

    }

    public override void ActiveFrame()
    {

    }

    public override void Execute()
    {
        actionDone = true;
    }
}
