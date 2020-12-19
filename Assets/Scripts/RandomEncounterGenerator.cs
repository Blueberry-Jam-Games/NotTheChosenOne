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
        player = GameObject.FindWithTag("Player");
        playerRB2D = player.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (playerRB2D.velocity.x != 0 || playerRB2D.velocity.y != 0)
        {
            framesSinceCombat++;
            if (Random.Range(0, 100) % framesSinceCombat >= 99.0f)
            {
                framesSinceCombat = 1.0f;
                //New stuff is creating a semaphore and configuring it with relavent information.
                GameObject semaphore = Instantiate(semaphoreRef);
                CombatSemaphore comSem = semaphore.GetComponent<CombatSemaphore>();
                int monstersToSpawn = Random.Range(0, 100);
                for(int i = 0; i < enemies.Count; i++)
                {
                    if(monstersToSpawn % enemies[i].probability <= 1)
                    {
                        comSem.enemiesToSpawn.Add(enemies[i].enemy);
                    }
                }

                LevelLoader ll = GameObject.FindGameObjectWithTag("LevelLoader").GetComponent<LevelLoader>();
                ll.BeginCombat("CombatBase");
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
