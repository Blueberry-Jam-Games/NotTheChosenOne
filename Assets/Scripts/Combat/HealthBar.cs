using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    protected Slider display;

    private string entName;
    private int maxHp;
    private int hp;

    // Start is called before the first frame update
    void Start()
    {
        display = GetComponentInChildren<Slider>();
        display.value = display.maxValue;
    }

    public void CreateBase(CombatUnit cu)
    {
        //Text title = GetComponentInChildren<Text>();
        entName = cu.unitName;
        maxHp = cu.maxHP;
        hp = cu.hp;
        StartCoroutine(InitializeLater());
        //display.maxValue = cu.maxHP;
        //display.value = cu.hp;
    }

    private IEnumerator InitializeLater()
    {
        Debug.Log("Health Bar Delay");
        yield return null;
        Debug.Log("Health Bar Complete");
        Text title = GetComponentInChildren<Text>();
        title.text = entName;
        display.maxValue = maxHp;
        display.value = hp;
    }

    private int hpTarget;

    public void DealDamage(int newHP)
    {
        hpTarget = newHP;
        Debug.Log("HP Anim to " + newHP);
        StartCoroutine(DamageAnim());
    }

    private IEnumerator DamageAnim()
    {
        Debug.Log("Start anim coroutine");
        while(hp > hpTarget)
        {
            //Debug.Log("Anim Loop");
            hp--;
            display.value = hp;
            yield return null;
        }
    }
}
