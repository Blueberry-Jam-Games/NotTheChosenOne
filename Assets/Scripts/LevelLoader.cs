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

        //Wait
        yield return new WaitForSeconds(transitionTime);

        //Load Scene
        SceneManager.LoadSceneAsync(level);
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        transition.SetTrigger("End");
    }
}
