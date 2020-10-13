using RPGTALK.Helper;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    private RPGTalk dialogue;
    public List<CombatUnit> player;
    public List<CombatUnit> enemy;

    List<CombatAction> turnActions;

    public BattleState STATE = BattleState.BEGIN;
    public ActionState ACTION_STATE = ActionState.START_NEXT;

    private int choosingUnit = 0;

    public GameObject playerHealthBase;
    public GameObject opponentHealthBase;

    public GameObject hpBarRef;

    #region Battle Start Code
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start combat maager");
        GameObject dm = GameObject.FindGameObjectWithTag("TextboxManager");
        dialogue = dm.GetComponent<RPGTalk>();

        dialogue.OnNewTalk += NewTalkExists;
        dialogue.OnMadeChoice += OnMadeChoice;

        turnActions = new List<CombatAction>();

        string title = "";

        foreach (CombatUnit cu in enemy)
        {
            CreateHealthBar(cu, opponentHealthBase);
            //Builds the intro message
            if (title != "")
                title += ", ";
            title += cu.unitName;
        }

        Debug.Log("Title configured " + title);
        RPGTalkVariable rtv = new RPGTalkVariable();
        rtv.variableName = "%e";
        rtv.variableValue = title;
        dialogue.variables[0] = rtv;
        dialogue.callback.AddListener(IntroTextEnd);

        foreach (CombatUnit cu in player)
        {
            CreateHealthBar(cu, playerHealthBase);
        }

        Debug.Log("Start coroutine");
        StartCoroutine(IntroTextLater());
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
        dialogue.NewTalk("StartDefault", "StartDefaultE");
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
        //dialogue.NewTalk("ChooseAction", "ChooseActionE"); //What goes here?
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
        CombatAction ca = player[choosingUnit].ResolveAction(questionID, choiceNumber, this);
        if(ca != null)
        {
            turnActions.Add(ca);
            choosingUnit++;
            if(choosingUnit >= player.Count)
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
            CombatAction ca = op.AIResolveAction(this);
            if (ca != null)
                turnActions.Add(ca);
        }
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
                Debug.Log("Executing");
                ACTION_STATE = ActionState.RUNNING;
                turnActions[0].Execute(dialogue);
            }
            else
            {
                //dialogue.NewTalk("ChooseAction", "ChooseActionE");
                //Choose state
                BeginChooseState();
            }
        }
        else if(STATE == BattleState.RUN && ACTION_STATE == ActionState.RUNNING)
        {
            if (turnActions[0].IsDone())
            {
                turnActions.RemoveAt(0);
                CheckKills();
                ACTION_STATE = ActionState.START_NEXT;
            }
        }
    }

    private void CheckKills()
    {
        for(int i = 0; i < player.Count; i++)
        {
            if(player[i].IsDead())
            {
                CombatUnit dead = player[i];
                player.RemoveAt(i);
                i--;
                dead.HandleKill();
            }
        }
        for (int i = 0; i < enemy.Count; i++)
        {
            if (enemy[i].IsDead())
            {
                CombatUnit dead = enemy[i];
                enemy.RemoveAt(i);
                i--;
                dead.HandleKill();
            }
        }
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
        dialogue.NewTalk("WinHunt", "WinHuntE");
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

    private void ResolveEnemyAction()
    {
        turnActions.Add(new ActionAdvance(enemy[0], 99, this));
    }

    #endregion

    public enum BattleState
    {
        BEGIN, CHOOSE, RUN, END
}
    public enum ActionState
    {
        START_NEXT, RUNNING
}
}
