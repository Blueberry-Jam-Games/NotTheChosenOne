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
        Debug.Log("Returning modified attack for " + unitName + " as " + atkBuff);
        return (int)atkBuff;
    }

    protected override int GetDefence()
    {
        float defBuff = def;
        defBuff *= manager.GetTensionModifier();
        defBuff *= guarding ? 1.5f : 1f;
        guarding = false;
        Debug.Log("Returning modified defence for " + unitName + " as " + defBuff);
        return (int)defBuff;
    }

    protected override int GetSpeed()
    {
        float speedBuff = spd;
        speedBuff *= manager.GetTensionModifier();
        Debug.Log("Returning modified speed for " + unitName + " as " + speedBuff);
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
