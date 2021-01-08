using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    Rigidbody2D rb;
    public int targetDoor = 0;
    private bool recentTransport = false;
    private bool previouslyCombat = false;

    // Allows locking the player in place before combat or during scene change and dialogue.
    public bool lockedControls = false;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        rb = gameObject.GetComponent<Rigidbody2D>();
        SceneManager.sceneLoaded += OnSceneLoaded;

        Debug.Log("Player Starting");
        GameObject startpoint = GameObject.FindWithTag("PlayerStart");
        if(startpoint != null)
        {
            Debug.Log("Startpoint identified, going there");
            transform.position = startpoint.transform.position;
        }
    }

    //float framesSinceCombat = 1;

    void FixedUpdate()
    {
        if (!lockedControls)
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            rb.velocity = new Vector2(moveHorizontal * moveSpeed, moveVertical * moveSpeed);
        }

        /*if(moveHorizontal != 0 && moveVertical != 0)
        {
            framesSinceCombat++;
            if(Random.Range(0, 100) % framesSinceCombat >= 99.0f)
            {
                framesSinceCombat = 1.0f;
                LevelLoader ll = GameObject.FindGameObjectWithTag("LevelLoader").GetComponent<LevelLoader>();
                ll.BeginCombat("CombatBase");
            }
        }*/
    }

    public void RequestGoToLevel(string level, int door)
    {
        Debug.Log("Request Go To Level " + level + " at door " + door + " and recentTransport = " + recentTransport);
        if (!recentTransport)
        {
            recentTransport = true;
            targetDoor = door;
            LevelLoader ll = GameObject.FindGameObjectWithTag("LevelLoader").GetComponent<LevelLoader>();
            ll.LoadNextLevel(level);
        }
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);
        //Debug.Log(mode);
        GameObject doorreg = GameObject.FindGameObjectWithTag("DoorRegistry");
        if (doorreg != null && !previouslyCombat)
        {
            gameObject.SetActive(true);
            Debug.Log("Player identified door system in use, going to target door " + targetDoor);
            DoorRegistry dr = doorreg.GetComponent<DoorRegistry>();
            dr.RequestDoor(targetDoor).MarkUsedRecently();
            transform.position = dr.RequestDoor(targetDoor).GetPlayerSpawnPosition();
        }
        else if (GameObject.FindGameObjectWithTag("CombatManager") != null)
        {
            Debug.Log("Overworld Player identified combat level loaded. Disabling");
            previouslyCombat = true;
            gameObject.SetActive(false);
        }
        else
        {
            previouslyCombat = false;
            gameObject.SetActive(true);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Player detected leaving collider");
        recentTransport = false;
    }

    public void SavePlayer ()
    {
        SaveAndLoad.Save(GameObject.FindGameObjectWithTag("Player"));
    }

    public void LoadPlayer()
    {
        PlayerData data = SaveAndLoad.Load();

        Vector3 position;
        position.x = data.position[0];
        position.y = data.position[1];
        position.z = data.position[2];

        transform.position = position;
    }
}
