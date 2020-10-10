using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class clicky : MonoBehaviour
{
    public Button StartGame;

    void Start()
    {
        Button btn = StartGame.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        Debug.Log("You have clicked the button!");
        LevelLoader ll = GameObject.FindGameObjectWithTag("LevelLoader").GetComponent<LevelLoader>();
        ll.LoadNextLevel("House");
       
    }
}
