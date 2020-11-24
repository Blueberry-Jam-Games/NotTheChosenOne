using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CombatUnitPlayable : CombatUnit
{
    //Modifiers for player units
    protected bool guarding = false;

    protected override int GetAttack()
    {
        float atkBuff = atk;
        atkBuff *= manager.GetTensionModifier();
        return (int)atkBuff;
    }

    protected override int GetDefence()
    {
        float defBuff = def;
        defBuff *= manager.GetTensionModifier();
        defBuff *= guarding ? 1.5f : 1f;
        guarding = false;
        return (int)defBuff;
    }

    protected override int GetSpeed()
    {
        float speedBuff = spd;
        speedBuff *= manager.GetTensionModifier();
        return (int)speedBuff;
    }

    public void Guard()
    {
        guarding = true;
    }

    public void UpdateTension(float newTension)
    {
        manager.SetPartyTension(newTension);
    }

    public float GetTension()
    {
        return manager.GetPartyTension();
    }

    //This is here so each player unit doesn't need it.
    public override CombatAction AIResolveAction()
    {
        Debug.LogError("Player AI function called unexpectedly");
        return null;
    }
}
