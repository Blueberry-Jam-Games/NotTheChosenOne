using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Assume it's the player attacking (for now)
public class ActionAttack : CombatAction
{
    public ActionAttack(CombatUnit user, int speed, CombatManager mngr) : base(user, speed, mngr)
    {

    }

    public override void Execute(RPGTalk dialogue)
    {
        CombatUnit cu = manager.enemy[0];
        cu.InflictDamage(user, 10);
        DisplayText(dialogue);
    }

    public override string GetText()
    {
        return user.unitName + " Attacked!";
    }
}
