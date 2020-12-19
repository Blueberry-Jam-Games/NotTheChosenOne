using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    Rigidbody2D rb;
    public int targetDoor = 0;
    private bool recentTransport = false;
    bool previouslyCombat = false;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        rb = gameObject.GetComponent<Rigidbody2D>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    //float framesSinceCombat = 1;

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        rb.velocity = new Vector2(moveHorizontal * moveSpeed, moveVertical * moveSpeed);

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
        //Debug.Log("OnSceneLoaded: " + scene.name);
        //Debug.Log(mode);
        GameObject doorreg = GameObject.FindGameObjectWithTag("DoorRegistry");
        if (doorreg != null && !previouslyCombat)
        {
            gameObject.SetActive(true);
            Debug.Log("Player identified door system in use, going to target door.");
            DoorRegistry dr = doorreg.GetComponent<DoorRegistry>();
            transform.position = dr.RequestDoor(targetDoor).transform.position;
            previouslyCombat = true;
        }
        else if (GameObject.FindGameObjectWithTag("CombatManager") != null)
        {
            Debug.Log("Overworld Player identified combat level loaded. Disabling");
            previouslyCombat = true;
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        recentTransport = false;
    }
}
