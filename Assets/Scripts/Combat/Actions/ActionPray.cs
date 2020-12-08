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
            animParticle = user.RequestAnimationObject("pray");
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
            Debug.Log("Prayer Dialogue closed");
            if(animParticle == null)
            {
                Debug.Log("Particle is null, action done");
                actionDone = true;
            }
            else
            {
                Debug.Log("Got Particle, checking for particle end");
                if(!animParticle.activeInHierarchy)
                {
                    Debug.Log("Destronying particles and flagging done");
                    GameObject.Destroy(animParticle);
                    actionDone = true;
                }
            }
        }
    }
}
