using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPray : CombatAction
{
    private GameObject animParticle;
    private bool dialogueDone;

    public ActionPray(CombatUnit user, int speed, CombatManager mngr) : base(user, speed, mngr)
    {

    }

    public override void Execute()
    {
        if (user is CombatUnitPlayable)
        {
            CombatUnitPlayable cup = (CombatUnitPlayable)user;
            animParticle = user.RequestParticles("pray");
            ParticleSystem ps = animParticle.GetComponent<ParticleSystem>();
            ps.Play();
            DisplayTextAtTitle("ActionPray");
            cup.UpdateTension(cup.GetTension() * 0.8f);
        }
    }

    public override void TextEnd()
    {
        dialogueDone = true;
        manager.GetDialogue().callback.RemoveListener(TextEnd);
    }

    public override void ActiveFrame()
    {
        if(dialogueDone)
        {
            if(animParticle == null)
            {
                actionDone = true;
            }
            else
            {
                if(animParticle.activeInHierarchy)
                {
                    actionDone = true;
                }
            }
        }
    }
}
