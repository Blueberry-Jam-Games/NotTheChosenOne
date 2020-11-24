using RPGTALK.Helper;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    private RPGTalk dialogue;
    public List<CombatUnit> player;
    public List<CombatUnit> enemy;

    List<CombatAction> turnActions;

    public BattleType battleType = BattleType.HUNT;
    public int LevelMin = 0;
    public int LevelMax = 3;

    public BattleState STATE = BattleState.BEGIN;
    public ActionState ACTION_STATE = ActionState.START_NEXT;

    private int choosingUnit = 0;

    public GameObject playerHealthBase;
    public GameObject opponentHealthBase;

    public GameObject hpBarRef;

    //The party tension system is handled via a slider
    private Slider tensionGauge;

    #region Battle Start Code
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start combat maager");
        GameObject dm = GameObject.FindGameObjectWithTag("TextboxManager");
        dialogue = dm.GetComponent<RPGTalk>();
        tensionGauge = GameObject.FindGameObjectWithTag("TensionGauge").GetComponent<Slider>();

        dialogue.OnNewTalk += NewTalkExists;
        dialogue.OnMadeChoice += OnMadeChoice;

        turnActions = new List<CombatAction>();

        SetRPGTalkVariables();

        dialogue.callback.AddListener(IntroTextEnd);

        foreach (CombatUnit cu in player)
        {
            CreateHealthBar(cu, playerHealthBase);
        }

        Debug.Log("Start coroutine");
        StartCoroutine(IntroTextLater());
    }

    #region Tension Stuff
    public float GetPartyTension()
    {
        return tensionGauge.value;
    }

    public void SetPartyTension(float newValue)
    {
        if(newValue >= tensionGauge.minValue && newValue <= tensionGauge.maxValue)
        {
            tensionGauge.value = newValue;
        }
    }

    public void SetRelativeTensionGauge(float multiplier)
    {
        tensionGauge.value *= multiplier;
    }

    public float GetTensionModifier()
    {
        //-\left(x - 0.75\right) ^{ 2+1
        return Mathf.Pow((GetPartyTension() - 0.75f), 2) + 1;
    }
    #endregion

    public RPGTalk GetDialogue()
    {
        return dialogue;
    }

    private void SetRPGTalkVariables()
    {
        //Title is first
        StringBuilder titleBuilder = new StringBuilder();

        titleBuilder.Append(enemy[0].unitName); // Handle enemy[0] independently to prime next step

        ConfigureVariable("%e0", enemy[0].unitName);
        CreateHealthBar(enemy[0], opponentHealthBase);
        for (int i = 1; i < enemy.Count; i++) // Now for the remainder
        {
            if(enemy.Count > 2)
            {
                titleBuilder.Append(", ");
            }

            if(i == enemy.Count - 1) // If this is the last one, add "and"
            {
                if (titleBuilder.ToString().Last() != ' ')
                    titleBuilder.Append(" ");
                titleBuilder.Append("and ");
            }

            titleBuilder.Append(enemy[i].unitName);
            ConfigureVariable("%e" + i, enemy[i].unitName);
            CreateHealthBar(enemy[i], opponentHealthBase);
        }

        Debug.Log("Title configured " + titleBuilder.ToString());

        ConfigureVariable("%title", titleBuilder.ToString());

        ConfigureVariable("%player", player[0].unitName);
        if(player.Count >= 2)
            ConfigureVariable("%ally", player[1].unitName);
    }

    public void ConfigureVariable(string key, string value)
    {
        //Find exising var
        RPGTalkVariable rtv = new RPGTalkVariable();
        foreach(RPGTalkVariable chk in dialogue.variables) // Check list
        {
            if (chk.variableName == key) // If we find the key we need, store it and quit
            {
                rtv = chk;
                break;
            }
        }
        rtv.variableValue = value;
    }

    private void CreateHealthBar(CombatUnit target, GameObject parent)
    {
        //Sets up the health bar
        GameObject hpBar = Instantiate(hpBarRef);
        hpBar.transform.SetParent(parent.transform);
        HealthBar hb = hpBar.GetComponent<HealthBar>();
        hb.CreateBase(target);
        target.ProvideHPBar(hb);
    }

    private IEnumerator IntroTextLater()
    {
        Debug.Log("Waiting for frame");
        yield return null;
        Debug.Log("Recieved frame");
        dialogue.NewTalk(battleType.StartDialogue(), battleType.StartDialogue() + "E");
        Debug.Log("Created StartDefault Dialogue");
    }

    public void NewTalkExists()
    {
        Debug.Log("On new talk called");
    }

    public void IntroTextEnd()
    {
        Debug.Log("Dialogue closed");
        dialogue.callback.RemoveListener(IntroTextEnd);
        BeginChooseState();
    }
    #endregion

    #region Choose Action Code
    private void BeginChooseState()
    {
        choosingUnit = 0;
        STATE = BattleState.CHOOSE;
        CreateChooseActionDialogue();
    }

    private void CreateChooseActionDialogue()
    {
        CombatUnit cu = player[choosingUnit];
        string choices = cu.GetDialogueChoiceTitle();
        dialogue.NewTalk(choices, choices + "E");
    }

    void OnMadeChoice(string questionID, int choiceNumber)
    {
        CombatAction ca = player[choosingUnit].ResolveAction(questionID, choiceNumber);
        if(ca != null)
        {
            turnActions.Add(ca);
            choosingUnit++;
            if(choosingUnit >= player.Count)
            {
                EnemyDecideActions();
                SortTurnActions();
                STATE = BattleState.RUN;
            }
            else
            {
                CreateChooseActionDialogue();
            }
        }
    }

    public void RegsterCombatAction(CombatAction additionalAction, bool advance = true)
    {
        turnActions.Add(additionalAction);
        if (advance)
        {
            choosingUnit++;
            if (choosingUnit >= player.Count)
            {
                EnemyDecideActions();
                STATE = BattleState.RUN;
            }
            else
            {
                CreateChooseActionDialogue();
            }
        }
    }

    private void EnemyDecideActions()
    {
        foreach(CombatUnit op in enemy)
        {
            CombatAction ca = op.AIResolveAction();
            if (ca != null)
                turnActions.Add(ca);
        }
    }

    public void CreateTalk(string title)
    {
        dialogue.NewTalk(title, title + "E");
    }

    #endregion

    #region Execute Action Code
    // Update is called once per frame
    void Update()
    {
        if (STATE == BattleState.RUN && ACTION_STATE == ActionState.START_NEXT)
        {
            if (turnActions.Count > 0)
            {
                if (turnActions[0].StillValid())
                {
                    Debug.Log("Executing Next Action");
                    ACTION_STATE = ActionState.RUNNING;
                    turnActions[0].Execute();
                }
                else
                {
                    Debug.Log("Removing Invalid Entry");
                    turnActions.RemoveAt(0);
                }
            }
            else
            {
                //Choose state
                BeginChooseState();
            }
        }
        else if(STATE == BattleState.RUN && ACTION_STATE == ActionState.RUNNING)
        {
            if(turnActions.Count == 0)
            {
                CheckKills();
                ACTION_STATE = ActionState.START_NEXT;
            }
            else if (turnActions[0].IsDone())
            {
                turnActions.RemoveAt(0);
                CheckKills();
                if (ACTION_STATE != ActionState.EXTRA_DIALOGUE)
                {
                    ACTION_STATE = ActionState.START_NEXT;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        //In battle, if we are in the animation state, allow that to run in real time so it is not bound to frame time.
        if(STATE == BattleState.RUN && ACTION_STATE == ActionState.RUNNING && turnActions.Count != 0 && !turnActions[0].IsDone())
        {
            turnActions[0].ActiveFrame();
        }
    }

    private void CheckKills()
    {
        List<string> deaths = new List<string>();
        bool anyKills = false;
        for(int i = 0; i < player.Count; i++)
        {
            if(player[i].IsDead())
            {
                anyKills = true;
                CombatUnit dead = player[i];
                deaths.Add(dead.unitName);
                player.RemoveAt(i);
                i--;
                dead.HandleKill();
            }
        }
        for (int i = 0; i < enemy.Count; i++)
        {
            if (enemy[i].IsDead())
            {
                anyKills = true;
                CombatUnit dead = enemy[i];
                deaths.Add(dead.unitName);
                enemy.RemoveAt(i);
                i--;
                dead.HandleKill();
            }
        }
        //If any entities have died, generate a string saying so and display it.
        if(anyKills)
        {
            StringBuilder killString = new StringBuilder();
            if(deaths.Count == 1)
            {
                ConfigureVariable("%target", deaths[0]);
                ACTION_STATE = ActionState.EXTRA_DIALOGUE;
                dialogue.callback.AddListener(DeathTextCallback);
                dialogue.NewTalk("EntityDie", "EntityDieE");
            }
            else
            {
                killString.Append(deaths[0]);
                int i = 1;
                bool comma = false;
                while (i < deaths.Count-1)
                {
                    killString.Append(", ");
                    killString.Append(deaths[i]);
                    comma = true;
                }
                if (comma)
                {
                    killString.Append(",");
                }
                killString.Append(" and ");
                killString.Append(deaths[deaths.Count - 1]);
            }
        }
    }

    private void DeathTextCallback()
    {
        ACTION_STATE = ActionState.START_NEXT;
        dialogue.callback.RemoveListener(DeathTextCallback);
        CheckDone();
    }

    private void CheckDone()
    {
        if(player.Count == 0)
        {
            Debug.Log("Game Over");
            STATE = BattleState.END;
        } 
        else if(enemy.Count == 0)
        {
            Debug.Log("Win");
            STATE = BattleState.END;
            PlayerWin();
        }
    }

    private void PlayerWin()
    {
        dialogue.callback.AddListener(EndTextEnd);
        dialogue.NewTalk(battleType.EndDialogue(), battleType.EndDialogue() + "E");
    }

    public void EndTextEnd()
    {
        Debug.Log("Return to overworld.");
    }

    private void SortTurnActions()
    {
        CombatAction val;

        for (int i = 1; i < turnActions.Count; i++)
        {
            val = turnActions[i];
            int flag = 0;
            for (int j = i - 1; j >= 0 && flag != 1;)
            {
                if (val.GetSpeed() > turnActions[j].GetSpeed())
                {
                    turnActions[j + 1] = turnActions[j];
                    j--;
                    turnActions[j + 1] = val;
                }
                else 
                { 
                    flag = 1; 
                }
            }
        }
    }
    #endregion

    public enum BattleState
    {
        BEGIN, CHOOSE, RUN, END
    }

    public enum ActionState
    {
        START_NEXT, EXTRA_DIALOGUE, RUNNING
    }
}

//This is going to be borrowed from Minecraft's CreativeTabs and Blocks system
public class BattleType
{
    //These are the options of battle types kind of like an enumeration
    public static readonly BattleType HUNT = new BattleType("StartDefault", "WinHunt");

    //The rest is the class implementation ----------------------
    //These are titles for RPGTalk
    private string startDialogue;
    private string endDialogue;
    //TODO anything else that can be decided here

    //The constructor needs to either get all information upfront, or follow up functions should return this allowing easy chaining of configuration into one line.
    public BattleType(string sd, string ed)
    {
        startDialogue = sd;
        endDialogue = ed;
    }

    public string StartDialogue()
    {
        return startDialogue;
    }

    public string EndDialogue()
    {
        return endDialogue;
    }

    //Setter functions should set the variable and return this object. This means they can be chained together.
    // ie. new BattleType("", "").SetStartDialogue("").SetEndDialogue("");
    public BattleType SetStartDialogue(string dialoge)
    {
        startDialogue = dialoge;
        return this;
    }

    public BattleType SetEndDialogue(string dialoge)
    {
        endDialogue = dialoge;
        return this;
    }
}
