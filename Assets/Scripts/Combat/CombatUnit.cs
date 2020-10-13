using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CombatUnit : MonoBehaviour
{
    public Facing direction;
    public string unitName;
    public int atk, def, acc;
    public int hp, maxHP;
    protected SpriteRenderer sp;
    protected HealthBar hpBar;

    public void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        hp = maxHP;
    }

    public void ProvideHPBar(HealthBar bar)
    {
        hpBar = bar;
    }

    public void InflictDamage(CombatUnit source, int attackPower)
    {
        Debug.Log("InflictDamage called");
        int damage = (source.atk * attackPower) - def;
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

    public abstract CombatAction ResolveAction(string question, int selection, CombatManager cmRef);

    public abstract CombatAction AIResolveAction(CombatManager cmRef);

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

    protected void MoveIn()
    {
        int newSortOrder = sp.sortingOrder;
        newSortOrder--;
        if(newSortOrder < 0)
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
}

public enum Facing
{
    FOREWARDS,
    BACKWARDS
}