using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1.0f;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void LoadNextLevel(string level)
    {
        StartCoroutine(LoadLevel(level));
    }
    
    IEnumerator LoadLevel(string level)
    {
        //Play Animation
        transition.SetTrigger("Start");
        Debug.Log("Loading level " + level + " applying cover");

        //Wait
        yield return new WaitForSeconds(transitionTime);

        Debug.Log("Cover applied, beginning async load");
        //Load Scene
        SceneManager.LoadSceneAsync(level);
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //transition.SetTrigger("End");
        if (GameObject.FindGameObjectWithTag("CombatManager") != null)
        {
            Debug.Log("Level loader detected combat scene");
        } 
        else if(mode == LoadSceneMode.Single)
        {
            Debug.Log("Level loader detected scene loaded, removing cover");
            transition.SetTrigger("End");
        }
    }

    string overworldLevel = "";

    public void BeginCombat(string combatScene)
    {
        overworldLevel = SceneManager.GetActiveScene().name;
        StartCoroutine(EnterCombatLevel(combatScene));
        //SceneManager.LoadSceneAsync(combatScene);
    }

    IEnumerator EnterCombatLevel(string combatLevel)
    {
        transition.SetTrigger("CombatForest");

        yield return new WaitForSeconds(2);

        SceneManager.LoadSceneAsync(combatLevel);
    }

    public void NotifyCombatSceneReady()
    {
        transition.SetTrigger("CombatReady");
    }

    public void EndCombat()
    {
        StartCoroutine(DoCombatEnd());
    }

    IEnumerator DoCombatEnd()
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        //Load Scene
        SceneManager.LoadSceneAsync(overworldLevel);
    }
}
