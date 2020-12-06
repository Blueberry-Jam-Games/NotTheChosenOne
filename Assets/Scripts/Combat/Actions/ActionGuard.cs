using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionGuard : CombatAction
{
    private bool textDone = false;
    private Animation anim;

    public ActionGuard(CombatUnit user, int speed, CombatManager mngr) : base(user, speed, mngr)
    {

    }

    public override void Execute()
    {
        if(user is CombatUnitPlayable)
        {
            CombatUnitPlayable cup = (CombatUnitPlayable)user;
            DisplayTextAtTitle("ActionGuard");
            GameObject shield = user.RequestAnimationObject("guard", 1);
            anim = shield.GetComponent<Animation>();
            anim.Play();
            cup.Guard();
        }
        else
        {
            Debug.LogError("Non player used player guard action");
        }
    }

    public override void TextEnd()
    {
        textDone = true;
    }

    public override void ActiveFrame()
    {
        if(anim == null)
        {
            if (textDone)
            {
                actionDone = true;
            }
        } 
        else if(!anim.isPlaying)
        {
            GameObject.Destroy(anim.gameObject);
            anim = null;
        }
    }
}
