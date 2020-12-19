using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSemaphore : MonoBehaviour
{
    public List<GameObject> enemiesToSpawn;

    private void Start()
    {
        enemiesToSpawn = new List<GameObject>();
        DontDestroyOnLoad(gameObject);
    }

    public void MarkJobCompleted()
    {
        Destroy(gameObject);
    }
}
