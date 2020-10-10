using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPray : CombatAction
{
    public ActionPray(CombatUnit user, int speed, CombatManager mngr) : base(user, speed, mngr)
    {

    }

    public override void Execute(RPGTalk dialogue)
    {
        DisplayText(dialogue);
    }

    public override string GetText()
    {
        return user.unitName + " Prayed!";
    }
}
