﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Assume it's the player attacking (for now)
public class ActionAttack : CombatAction
{

    private int target;
    private int enemies;

    public ActionAttack(CombatUnit user, int target, int speed, CombatManager mngr) : base(user, speed, mngr)
    {
        this.target = target;
        this.enemies = mngr.enemy.Count;
    }

    public override void Execute()
    {
        if (manager.enemy.Count != enemies)
        {
            DisplayTextAtTitle("ActionAttackFail");
        }
        else
        {
            DisplayTextAtTitle("ActionAttackSuccess");
            CombatUnit cu = manager.enemy[target]; //TODO apply targeting
            cu.InflictDamage(user, 10);
        }
    }

    public override void ActiveFrame()
    {

    }
}
