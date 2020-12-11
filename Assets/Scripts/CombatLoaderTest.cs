using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatLoaderTest : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject levelLoader = GameObject.FindWithTag("LevelLoader");
        LevelLoader ll = levelLoader.GetComponent<LevelLoader>();
        ll.BeginCombat("CombatBase");
    }
}
