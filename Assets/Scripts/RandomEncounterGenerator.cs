using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEncounterGenerator : MonoBehaviour
{
    [SerializeField]
    public List<EncounterData> enemies;

    public GameObject semaphoreRef;

    private GameObject player;
    private Rigidbody2D playerRB2D;

    float framesSinceCombat = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FirstFrameInitialization());
    }

    IEnumerator FirstFrameInitialization()
    {
        yield return new WaitForEndOfFrame();
        Debug.Log("Random encounter system initializing");
        player = GameObject.FindWithTag("Player");
        playerRB2D = player.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (player != null)
        {
            if (playerRB2D.velocity.x != 0 || playerRB2D.velocity.y != 0)
            {
                framesSinceCombat++;
                if (Random.Range(0, 100) % framesSinceCombat >= 99.0f)
                {
                    framesSinceCombat = 1.0f;
                    //New stuff is creating a semaphore and configuring it with relavent information.
                    Debug.Log("Starting battle, generating enemies");
                    GameObject semaphore = Instantiate(semaphoreRef);
                    CombatSemaphore comSem = semaphore.GetComponent<CombatSemaphore>();
                    DontDestroyOnLoad(semaphore); //TODO do I need this?
                    int monstersToSpawn = Random.Range(0, 100);
                    Debug.Log("Random number is " + monstersToSpawn);
                    for (int i = 0; i < enemies.Count; i++)
                    {
                        Debug.Log("Checking random against " + enemies[i].probability + " with result " + monstersToSpawn % enemies[i].probability);
                        if ((monstersToSpawn % enemies[i].probability) <= 1)
                        {
                            Debug.Log("Monster added to encounter list");
                            comSem.enemiesToSpawn.Add(enemies[i].enemy);
                        }
                    }

                    LevelLoader ll = GameObject.FindGameObjectWithTag("LevelLoader").GetComponent<LevelLoader>();
                    ll.BeginCombat("CombatBase");
                }
            }
        }
    }
}

[System.Serializable]
public struct EncounterData
{
    public GameObject enemy;
    public int probability;
}
