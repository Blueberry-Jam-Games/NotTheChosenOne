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
        manager.ConfigureVariable("%user", user.unitName);
        dialogue.NewTalk(title, title + "E");
        talkStore = dialogue;
    }

    public virtual void TextEnd()
    {
        Debug.Log("TextEnd Triggered");
        actionDone = true;
        talkStore.callback.RemoveListener(TextEnd);
        talkStore = null;
    }

    public abstract string GetText();
    public abstract void Execute(RPGTalk dialogue);

    public abstract void ActiveFrame();

    public bool StillValid()
    {
        return user != null && user.gameObject != null;
    }
}

public class ActionAdvance : CombatAction
{
    float animationFrame;
    bool halfway = false;
    static readonly float ANIM_LEN = 30.0f;
    bool dialogueDone = false;
    bool forewards;

    public ActionAdvance(CombatUnit user, int speed, CombatManager mngr) : base(user, speed, mngr)
    {
        
    }

    public override void Execute(RPGTalk dialogue)
    {
        forewards = user.direction == Facing.FOREWARDS;
        //Debug.Log("Executing Advance for " + user.unitName);
        //user.Advance();
        //DisplayTextAtTitle(dialogue, "ActionAdvance");
        //user.Retreat();

        //int newdepth = user.GetDepth() + (forewards ? -1 : 1);
        //Debug.Log("Checking advance with current depth " + user.GetDepth() + " and next depth " + newdepth);
        //if (newdepth <= manager.LevelMax && newdepth >= manager.LevelMin)
        //{
            animationFrame = 0.0f;
            Debug.Log("Advance executing current layer " + user.GetDepth() + " target layer " + (user.direction == Facing.FOREWARDS ? user.GetDepth() - 1 : user.GetDepth() + 1));
            DisplayTextAtTitle(dialogue, "ActionAdvance");
        /*}
        else
        {
            animationFrame = ANIM_LEN + 1;
            DisplayTextAtTitle(dialogue, "ActionAdvanceFail");
        }*/
    }

    public override string GetText()
    {
        Debug.Log("Advance executed");
        return user.unitName + " Advanced!";
    }

    public override void TextEnd()
    {
        Debug.Log("advance recieved text end Triggered");
        dialogueDone = true;
        talkStore.callback.RemoveListener(TextEnd);
        talkStore = null;
    }

    public override void ActiveFrame()
    {
        if (animationFrame < ANIM_LEN) // When the animation is over, this is false
        {
            animationFrame++; //Else advance the animation
            if (Mathf.Abs(animationFrame - ANIM_LEN / 2) < Mathf.Epsilon)
            {
                user.Advance();
                halfway = true;
            }

            int layer = forewards ? (halfway ? user.GetDepth() : user.GetDepth() - 1) : (halfway ? user.GetDepth() - 1 : user.GetDepth());
            float distance = forewards ? 1-animationFrame / ANIM_LEN : animationFrame / ANIM_LEN;
            //Debug.Log("Setting animation properties at frame " + animationFrame + " progress " + animationFrame / ANIM_LEN + " and target layer " + layer + " resulting in distance " + distance);
            user.ApplyMovementAnimation(layer, distance);
        }
        else
        {
            if (dialogueDone)
            {
                actionDone = true;
            }
        }
    }
}

public class ActionWithdraw : CombatAction
{
    float animationFrame;
    bool halfway = false;
    static readonly float ANIM_LEN = 30.0f;
    bool dialogueDone = false;
    bool forewards;

    public ActionWithdraw(CombatUnit user, int speed, CombatManager mngr) : base(user, speed, mngr)
    {

    }

    public override void Execute(RPGTalk dialogue)
    {
        //int newdepth = user.GetDepth() + (forewards ? 1 : -1);
        //Debug.Log("Checking withdraw with current depth " + user.GetDepth() + " and next depth " + newdepth + " Min is " + manager.LevelMin + " max is " + manager.LevelMax);
        //if (newdepth <= manager.LevelMax && newdepth >= manager.LevelMin)
        //{
            animationFrame = 0.0f;
            Debug.Log("Withdraw executed");
            DisplayTextAtTitle(dialogue, "ActionRetreat");
            forewards = user.direction == Facing.FOREWARDS;
        /*}
        else
        {
            animationFrame = ANIM_LEN + 1;
            DisplayTextAtTitle(dialogue, "ActionEndByRetreat");
            Debug.Log("Flee Condition"); // CombatUnit will handle this
        }*/
    }

    public override string GetText()
    {
        return user.unitName + " Retreated!";
    }

    public override void TextEnd()
    {
        Debug.Log("Withdraw recieved text end Triggered");
        dialogueDone = true;
        talkStore.callback.RemoveListener(TextEnd);
        talkStore = null;
    }

    public override void ActiveFrame()
    {
        if (animationFrame < ANIM_LEN) // When the animation is over, this is false
        {
            animationFrame++; //Else advance the animation
            if (Mathf.Abs(animationFrame - ANIM_LEN / 2) < Mathf.Epsilon)
            {
                user.Retreat();
                halfway = true;
            }

            int layer = !halfway ? user.GetDepth() : forewards ? user.GetDepth() - 1 : user.GetDepth() + 1;
            float distance = forewards ? animationFrame / ANIM_LEN : 1 - animationFrame / ANIM_LEN;
            user.ApplyMovementAnimation(layer, distance);
        }
        else
        {
            if(dialogueDone)
            {
                actionDone = true;
            }
        }
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

    public override void ActiveFrame()
    {

    }
}
