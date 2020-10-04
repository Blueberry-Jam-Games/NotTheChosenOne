using RPGTALK.Helper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CombatAction
{
    public CombatAction(CombatUnit user, int speed, CombatManager mngr)
    {
        this.user = user;
        this.speed = speed;
        actionDone = false;
        manager = mngr;
    }

    protected int speed;
    protected CombatUnit user;
    protected bool actionDone;
    protected RPGTalk talkStore;
    protected CombatManager manager;
    
    public int GetSpeed()
    {
        return speed;
    }

    public CombatUnit GetUser()
    {
        return user;
    }

    public bool IsDone()
    {
        return actionDone;
    }

    protected void DisplayText(RPGTalk dialogue)
    {
        RPGTalkVariable rtv = new RPGTalkVariable
        {
            variableName = "%s",
            variableValue = GetText()
        };
        dialogue.variables[1] = rtv;
        dialogue.callback.AddListener(TextEnd);
        dialogue.NewTalk("UseSkill", "UseSkillE");
        talkStore = dialogue;
    }

    public void TextEnd()
    {
        talkStore.callback.RemoveListener(TextEnd);
        actionDone = true;
        talkStore = null;
    }

    public abstract string GetText();
    public abstract void Execute(RPGTalk dialogue);
}

public class ActionAdvance : CombatAction
{
    public ActionAdvance(CombatUnit user, int speed, CombatManager mngr) : base(user, speed, mngr)
    {
        
    }

    public override void Execute(RPGTalk dialogue)
    {
        user.Advance();
        DisplayText(dialogue);
    }

    public override string GetText()
    {
        Debug.Log("Advance executed");
        return user.unitName + " Advanced!";
    }
}

public class ActionWithdraw : CombatAction
{
    public ActionWithdraw(CombatUnit user, int speed, CombatManager mngr) : base(user, speed, mngr)
    {

    }

    public override void Execute(RPGTalk dialogue)
    {
        Debug.Log("Withdraw executed");
        user.Retreat();
        DisplayText(dialogue);
    }

    public override string GetText()
    {
        return user.unitName + " Retreated!";
    }
}
