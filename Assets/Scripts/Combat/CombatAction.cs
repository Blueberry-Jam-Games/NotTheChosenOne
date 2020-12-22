using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CombatAction
{
    protected int speed;
    protected CombatUnit user;
    protected bool actionDone;
    protected CombatManager manager;

    public CombatAction(CombatUnit user, int speed, CombatManager mngr)
    {
        this.user = user;
        this.speed = speed;
        actionDone = false;
        manager = mngr;
    }
    
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

    protected void DisplayTextAtTitle(string title)
    {
        manager.GetDialogue().callback.AddListener(TextEnd);
        manager.ConfigureVariable("%user", user.unitName);
        manager.GetDialogue().NewTalk(title, title + "E");
    }

    public virtual void TextEnd()
    {
        actionDone = true;
        manager.GetDialogue().callback.RemoveListener(TextEnd);
    }

    public abstract void Execute();

    public abstract void ActiveFrame();

    public bool StillValid()
    {
        return user != null && user.gameObject != null;
    }
}

public class ActionAdvance : CombatAction
{
    float animationFrame;
    static readonly float ANIM_LEN = 30.0f;
    bool dialogueDone = false;
    bool forewards;
    int animationLayer;
    bool succeding;

    public ActionAdvance(CombatUnit user, int speed, CombatManager mngr) : base(user, speed, mngr)
    {
        forewards = user.direction == Facing.FOREWARDS;
        int destinationLayer = user.GetDepth() + (forewards ? -1 : 1);
        succeding = destinationLayer <= manager.LevelMax && destinationLayer >= manager.LevelMin;
        animationLayer = forewards ? user.GetDepth() - 1 : user.GetDepth();
    }

    public override void Execute()
    {
        if (succeding)
        {
            animationFrame = 0.0f;
            Debug.Log("Advance executing current layer " + user.GetDepth() + " target layer " + (user.direction == Facing.FOREWARDS ? user.GetDepth() - 1 : user.GetDepth() + 1));
            DisplayTextAtTitle("ActionAdvance");
        }
        else
        {
            animationFrame = ANIM_LEN + 1;
            DisplayTextAtTitle("ActionAdvanceFail");
        }
    }

    public override void TextEnd()
    {
        Debug.Log("advance recieved text end Triggered");
        dialogueDone = true;
        manager.GetDialogue().callback.RemoveListener(TextEnd);
    }

    public override void ActiveFrame()
    {
        if (animationFrame < ANIM_LEN) // When the animation is over, this is false
        {
            animationFrame++; //Else advance the animation
            if (Mathf.Abs(animationFrame - ANIM_LEN / 2) < Mathf.Epsilon)
            {
                user.Advance();
            }
            float distance = forewards ? 1-animationFrame / ANIM_LEN : animationFrame / ANIM_LEN;
            user.ApplyMovementAnimation(animationLayer, distance);
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
    static readonly float ANIM_LEN = 30.0f;
    bool dialogueDone = false;
    bool forewards;
    int animationLayer;
    bool succeding;

    public ActionWithdraw(CombatUnit user, int speed, CombatManager mngr) : base(user, speed, mngr)
    {
        forewards = user.direction == Facing.FOREWARDS;
        int destinationLayer = user.GetDepth() + (forewards ? 1 : -1);
        succeding = destinationLayer <= manager.LevelMax && destinationLayer >= manager.LevelMin;
        animationLayer = forewards ? user.GetDepth() : user.GetDepth() - 1;
    }

    public override void Execute()
    {
        if (succeding)
        {
            animationFrame = 0.0f;
            DisplayTextAtTitle("ActionRetreat");
            forewards = user.direction == Facing.FOREWARDS;
        }
        else
        {
            animationFrame = ANIM_LEN + 1;
            //DisplayTextAtTitle("ActionEndByRetreat");
            Flee();
        }
    }

    public override void TextEnd()
    {
        dialogueDone = true;
        manager.GetDialogue().callback.RemoveListener(TextEnd);
    }

    public override void ActiveFrame()
    {
        if (animationFrame < ANIM_LEN) // When the animation is over, this is false
        {
            animationFrame++; //Else advance the animation
            if (Mathf.Abs(animationFrame - ANIM_LEN / 2) < Mathf.Epsilon)
            {
                user.Retreat();
            }
            float distance = forewards ? animationFrame / ANIM_LEN : 1 - animationFrame / ANIM_LEN;
            user.ApplyMovementAnimation(animationLayer, distance);
        }
        else
        {
            if(dialogueDone)
            {
                actionDone = true;
            }
        }
    }

    public void Flee()
    {
        float averageSpeed = 0;
        foreach (CombatUnit cu in manager.enemy)
        {
            averageSpeed += cu.GetSpeed();
        }
        averageSpeed /= manager.enemy.Count;

        float deltaSpeed = averageSpeed - GetSpeed();
        if (Random.Range(0, 100) > deltaSpeed)
        {
            manager.RequestEndBattle("PlayerFlee");
        }
    }
}

public class ActionNothing : CombatAction
{
    public ActionNothing(CombatUnit user, CombatManager mngr) : base(user, 0, mngr)
    {

    }

    public override void Execute()
    {
        actionDone = true;
    }

    public override void ActiveFrame()
    {

    }
}
