using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryController : MonoBehaviour
{
    public const string TITLE_LEVEL = "PaintedVillage";
    public const string LOADING_LEVEL = "IntermediateScene";

    public const string STORY_STAGE_KEY = "storystage";
    public const string LAST_LEVEL_KEY = "savedlevel";
    public const int STAGE_TITLE = 0;
    public const int STAGE_GET_ARROW = 1;
    public const int STAGE_HAS_ARROW = 2;
    
    public int StoryStage = STAGE_TITLE;
    public string LastLevel = "";

    private LevelLoader levelLoader;
    private MusicManager musicManager;
    private PlayerMovement player;

    // Start is called before the first frame update
    void Start()
    {
        //Should always start on the title screen
        DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;

        Debug.Log("Story controller starting and initializing");

        if(StoryStage == STAGE_TITLE)
        {
            Debug.Log("Detecting title screen mode");
            //Load async other world
            SceneManager.LoadScene(TITLE_LEVEL, LoadSceneMode.Additive);
        }

        levelLoader = GameObject.FindWithTag("LevelLoader").GetComponent<LevelLoader>();
    }

    public void OnSceneLoaded(Scene newScene, LoadSceneMode loadMode)
    {
        Debug.Log("Story Controller noted scene " + newScene.name + " is loading");
        if(newScene.name == TITLE_LEVEL && StoryStage == STAGE_TITLE)
        {
            SetupTitleScreenPostLoad();
        }
        else if(newScene.name == LOADING_LEVEL)
        {
            //This is our chance to enable everything to be ready once the next level is loaded
            musicManager.IsEnabled = true;
            player.lockedControls = false;
            //Load the scene needed for this part of the story:
            if (StoryStage == STAGE_GET_ARROW)
            {
                //levelLoader.LoadNextLevel("House");
                StartCoroutine(LoadLevelNextFrame("House"));
            }
            //Here, the palyer can be in their house, the village, or the forest
            //TODO Maybe player position?
            else if (StoryStage == STAGE_HAS_ARROW)
            {
                if (LastLevel != "")
                {
                    //levelLoader.LoadNextLevel(LastLevel);
                    StartCoroutine(LoadLevelNextFrame(LastLevel));
                }
                else
                {
                    //levelLoader.LoadNextLevel("House");
                    StartCoroutine(LoadLevelNextFrame("House"));
                }
            }
        }
        //TODO be more pickey
        LastLevel = newScene.name;
        PlayerPrefs.SetString(LAST_LEVEL_KEY, LastLevel);
        PlayerPrefs.Save();
    }

    private IEnumerator LoadLevelNextFrame(string level)
    {
        yield return null;
        levelLoader.LoadNextLevel(level);
    }

    private void SetupTitleScreenPostLoad()
    {
        Debug.Log("In title screen initialization stage");
        GameObject plr= GameObject.FindGameObjectWithTag("Player");
        player = plr.GetComponent<PlayerMovement>();
        player.lockedControls = true;
        plr.SetActive(false);

        //Configure camera
        GameObject[] cameras = GameObject.FindGameObjectsWithTag("MainCamera");
        int count = cameras.Length;
        for (int i = 0; i < count; i++)
        {
            if (cameras[i].GetComponent<FollowCamera>() != null)
            {
                cameras[i].SetActive(false);
            }
        }
        //Configure Music
        GameObject mm = GameObject.FindGameObjectWithTag("MusicManager");
        musicManager = mm.GetComponent<MusicManager>();
        musicManager.IsEnabled = false;
        musicManager.StopMusic();
        StartCoroutine(TitleScreenInitialization());
    }

    public IEnumerator TitleScreenInitialization()
    {
        yield return null;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.SetActive(false);
        musicManager.IsEnabled = true;
        //musicManager.PlayMusic("title");
    }

    public void TitleScreenNewGame()
    {
        StoryStage = STAGE_GET_ARROW;
        PlayerPrefs.SetInt(STORY_STAGE_KEY, StoryStage);
        LastLevel = "";
        PlayerPrefs.SetString(LAST_LEVEL_KEY, LastLevel);
        PlayerPrefs.Save();
        TitleScreenStartGame();
    }

    public void TitleScreenStartGame()
    {
        StoryStage = PlayerPrefs.GetInt(STORY_STAGE_KEY, STAGE_GET_ARROW);
        LastLevel = PlayerPrefs.GetString(LAST_LEVEL_KEY, "");
        PlayerPrefs.Save();
        //Load scene without breaking things
        levelLoader.LoadNextLevel(LOADING_LEVEL);
    }

    public void ProgressStoryStage(int nextStoryStage)
    {
        StoryStage = nextStoryStage;
        StoryStage = PlayerPrefs.GetInt(STORY_STAGE_KEY, StoryStage);
        PlayerPrefs.Save();
    }
}
