using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSemaphore : MonoBehaviour
{
    public List<GameObject> enemiesToSpawn = new List<GameObject>();
    public List<GameObject> playerParty;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void MarkJobCompleted()
    {
        Destroy(gameObject);
    }
}
