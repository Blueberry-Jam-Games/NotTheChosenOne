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

    protected void DisplayTextAtTitle(RPGTalk dialogue, string title)
    {
        dialogue.callback.AddListener(TextEnd);
        Debug.Log("Displaying text at title " + title + " and registering listener");
        dialogue.NewTalk(title, title + "E");
        talkStore = dialogue;
    }

    public void TextEnd()
    {
        Debug.Log("TextEnd Triggered");
        actionDone = true;
        talkStore.callback.RemoveListener(TextEnd);
        talkStore = null;
    }

    public abstract string GetText();
    public abstract void Execute(RPGTalk dialogue);

    public bool StillValid()
    {
        return user != null && user.gameObject != null;
    }
}

public class ActionAdvance : CombatAction
{
    public ActionAdvance(CombatUnit user, int speed, CombatManager mngr) : base(user, speed, mngr)
    {
        
    }

    public override void Execute(RPGTalk dialogue)
    {
        Debug.Log("Executing Advance for " + user.unitName);
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

public class ActionNothing : CombatAction
{
    public ActionNothing(CombatUnit user, CombatManager mngr) : base(user, 0, mngr)
    {

    }

    public override void Execute(RPGTalk dialogue)
    {
        actionDone = true;
    }

    public override string GetText()
    {
        return "";
    }
}
