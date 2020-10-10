using RPGTALK.Helper;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CombatManager : MonoBehaviour
{

    public static readonly int BEGIN = 0;
    public static readonly int CHOOSE = 1;
    public static readonly int RUN = 2;
    public static readonly int END = 3;

    public static readonly int START_NEXT = 0;
    public static readonly int RUNNING = 1;

    private RPGTalk dialogue;
    public List<CombatUnit> player;
    public List<CombatUnit> enemy;

    List<CombatAction> turnActions;

    public int STATE = BEGIN;
    public int ACTION_STATE = START_NEXT;

    public GameObject playerHealthBase;
    public GameObject opponentHealthBase;

    public GameObject hpBarRef;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start cm");
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
    }

    public void NewTalkExists()
    {
        Debug.Log("On new talk called");
    }

    public void IntroTextEnd()
    {
        Debug.Log("Dialogue closed");
        dialogue.callback.RemoveListener(IntroTextEnd);
        dialogue.NewTalk("ChooseAction", "ChooseActionE");
        STATE = CHOOSE;
    }

    void OnMadeChoice(string questionID, int choiceNumber)
    {
        Debug.Log("Aha! In the question " + questionID + " you choosed the option " + choiceNumber);
        if (STATE == CHOOSE)
        {
            Debug.Log("In choose state and action chosen");
            if ((questionID == "action" && choiceNumber == 0) || (questionID == "skill" && choiceNumber == 3))
            {
                //Do nothing
            }
            else
            {
                Debug.Log("Identifying actions");
                ResolvePlayerActin(questionID, choiceNumber);
                ResolveEnemyAction();
                SortTurnActions();
                STATE = RUN;
                ACTION_STATE = START_NEXT;
            }
        }
        else
        {
            Debug.Log("Choice made when it shouldn't be possible");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (STATE == RUN && ACTION_STATE == START_NEXT)
        {
            if (turnActions.Count > 0)
            {
                Debug.Log("Executing");
                ACTION_STATE = RUNNING;
                turnActions[0].Execute(dialogue);
            }
            else
            {
                STATE = CHOOSE;
                dialogue.NewTalk("ChooseAction", "ChooseActionE");
            }
        }
        else if(STATE == RUN && ACTION_STATE == RUNNING)
        {
            if (turnActions[0].IsDone())
            {
                turnActions.RemoveAt(0);
                CheckKills();
                ACTION_STATE = START_NEXT;
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
        } 
        else if(enemy.Count == 0)
        {
            Debug.Log("Win");
            PlayerWin();
        }
    }

    private void PlayerWin()
    {

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

    private void ResolvePlayerActin(string questionID, int choiceNumber)
    {
        Debug.Log("Resolving player action");
        if (questionID == "action")
        {
            switch (choiceNumber)
            {
                case 0:
                    break; //Sub-menue
                case 1:
                    //Inventory
                    break;
                case 2:
                    Debug.Log("Advance");
                    turnActions.Add(new ActionAdvance(player[0], 100, this)); //TODO Speed
                    break;
                case 3:
                    Debug.Log("Withdraw");
                    turnActions.Add(new ActionWithdraw(player[0], 100, this)); //TODO Speed
                    break;
                default:
                    break; //Here as a failsafe
            }
        }
        else if (questionID == "skill")
        {
            switch (choiceNumber)
            {
                case 0:
                    turnActions.Add(new ActionAttack(player[0], 150, this)); //TODO Speed
                    break; //Attack
                case 1:
                    break; //Guard
                case 2:
                    break; //Pray
                case 3:
                    break; //Sub-menue
                default:
                    break;
            }
        }
    }
}
