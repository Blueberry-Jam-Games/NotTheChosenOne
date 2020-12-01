using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CombatUnit : MonoBehaviour
{
    public Facing direction;
    public string unitName;
    [SerializeField]
    protected int atk, def, acc, spd;
    public int hp, maxHP;
    protected CombatManager manager;
    protected SpriteRenderer sp;
    protected HealthBar hpBar;
    protected Animator unitAnim;

    public List<ActionParticle> particles;
    protected Dictionary<string, GameObject> actionParticles;

    public void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        hp = maxHP;
        unitAnim = GetComponent<Animator>();
        manager = GameObject.FindWithTag("CombatManager").GetComponent<CombatManager>();
        float tgt = GetTargetScale();
        transform.localScale = new Vector3(tgt, tgt, tgt);
        actionParticles = new Dictionary<string, GameObject>();
        foreach(ActionParticle ap in particles)
        {
            actionParticles.Add(ap.name, ap.particles);
        }
    }

    public void ProvideHPBar(HealthBar bar)
    {
        hpBar = bar;
    }

    public void InflictDamage(CombatUnit source, int attackPower)
    {
        Debug.Log("InflictDamage called");
        int damage = (source.GetAttack() * attackPower) - GetDefence();
        hp -= damage;
        if (hp < 0) hp = 0;
        Debug.Log("Starting hp bar");
        hpBar.DealDamage(hp);
    }

    public bool IsDead()
    {
        return hp == 0;
    }

    public void HandleKill()
    {
        Destroy(hpBar.gameObject);
        Destroy(this.gameObject);
    }

    public abstract string GetDialogueChoiceTitle();

    public abstract CombatAction ResolveAction(string question, int selection);

    public abstract CombatAction AIResolveAction();

    #region Stat Getters
    //Presumably you can override these on a per unit basis to achieve modifiers.
    protected virtual int GetAttack()
    {
        return atk;
    }

    protected virtual int GetDefence()
    {
        return def;
    }

    protected virtual int GetSpeed()
    {
        return spd;
    }
    #endregion

    #region Layer System
    public int GetDepth()
    {
        return sp.sortingOrder;
    }

    public void Advance()
    {
        if (direction == Facing.FOREWARDS)
        {
            MoveIn();
        }
        else if (direction == Facing.BACKWARDS)
        {
            MoveOut();
        }
    }

    public void Retreat()
    {
        if (direction == Facing.FOREWARDS)
        {
            MoveOut();
        }
        else if (direction == Facing.BACKWARDS)
        {
            MoveIn();
        }
    }

    public float GetTargetScale()
    {
        return 0.7f + GetDepth() * 0.1f;
    }

    protected void MoveIn()
    {
        int newSortOrder = sp.sortingOrder;
        newSortOrder--;
        if (newSortOrder < 0)
        {
            //Flee
        }
        else
        {
            sp.sortingOrder = newSortOrder;
        }
    }

    protected void MoveOut()
    {
        int newSortOrder = sp.sortingOrder;
        newSortOrder++;
        if (newSortOrder > 4)
        {
            //Flee
        }
        else
        {
            sp.sortingOrder = newSortOrder;
        }
    }
    #endregion
    protected void DisplayActionTargeting()
    {
        manager.CreateTalk("Target" + manager.enemy.Count);
    }

    public void ApplyMovementAnimation(int level, float animCount)
    {
        float tgt = 0.7f + (level) * 0.1f + animCount*0.1f;
        transform.localScale = new Vector3(tgt, tgt, tgt);
    }

    public GameObject RequestParticles(string action) //TODO callbacks
    {
        if(actionParticles.ContainsKey(action))
        {
            GameObject part = Instantiate(actionParticles[action], transform, false);
            Renderer rd = part.GetComponent<Renderer>();
            rd.sortingOrder = GetDepth();
            return part;
        }
        else
        {
            Debug.LogError("Particle " + action + " was not found for this unit.");
            return null;
        }
    }
}

public enum Facing
{
    FOREWARDS,
    BACKWARDS
}

[System.Serializable]
public struct ActionParticle
{
    public string name;
    public GameObject particles;
}