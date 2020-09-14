using RPGTALK.Helper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{

    private RPGTalk dialogue;
    public List<CombatUnit> player;
    public List<CombatUnit> enemy;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start cm");
        GameObject dm = GameObject.FindGameObjectWithTag("TextboxManager");
        dialogue = dm.GetComponent<RPGTalk>();

        dialogue.OnNewTalk += NewTalkExists;
        dialogue.OnMadeChoice += OnMadeChoice;

        string title = "";

        foreach (CombatUnit cu in enemy)
        {
            if (title != "")
                title += ", ";
            title += cu.name;
        }
        Debug.Log("Title configured " + title);
        RPGTalkVariable rtv = new RPGTalkVariable();
        rtv.variableName = "%e";
        rtv.variableValue = title;
        dialogue.variables[0] = rtv;
        dialogue.callback.AddListener(IntroTextEnd);
        Debug.Log("Start coroutine");
        StartCoroutine(IntroTextLater());
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
    }

    void OnMadeChoice(string questionID, int choiceNumber)
    {
        Debug.Log("Aha! In the question " + questionID + " you choosed the option " + choiceNumber);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
