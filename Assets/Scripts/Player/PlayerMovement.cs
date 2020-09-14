using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    Rigidbody2D rb;
    public int targetDoor = 0;
    private bool recentTransport = false;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        rb = gameObject.GetComponent<Rigidbody2D>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(moveHorizontal * moveSpeed, moveVertical * moveSpeed);
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
        DoorRegistry dr = doorreg.GetComponent<DoorRegistry>();
        transform.position = dr.RequestDoor(targetDoor).transform.position;
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        recentTransport = false;
    }
}
