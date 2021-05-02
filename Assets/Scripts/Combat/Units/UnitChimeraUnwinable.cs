using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitChimeraUnwinable : CombatUnit
{
    private int health = 100; // This is different than HP, this goes linearly hp goes exponentially

    public override CombatAction AIResolveAction()
    {
        return new ActionNothing(this, manager);
    }

    public override string GetDialogueChoiceTitle()
    {
        Debug.LogError("Chimera Script Called Unexpectedly");
        return "";
    }

    public override CombatAction ResolveAction(string question, int selection)
    {
        Debug.LogError("Chimera Resolve Action Called Unexpectedly");
        return new ActionNothing(this, manager);
    }

    public override void InflictDamage(CombatUnit source, int attackPower)
    {
        Debug.Log("Chimera InflictDamage called");
        //int damage = (source.GetAttack() * attackPower) - GetDefence();
        health -= 10;
        if (health < 0) health = 0;
        hp = (1000 / (100-health)) + 15;
        if (hp < 0) hp = 0;
        Debug.Log("Starting hp bar");
        hpBar.DealDamage(hp);
    }
}
