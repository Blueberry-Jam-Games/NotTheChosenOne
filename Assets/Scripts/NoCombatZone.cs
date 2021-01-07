using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoCombatZone : MonoBehaviour
{
    RandomEncounterGenerator encounterSystem;

    private void Start()
    {
        GameObject reg = GameObject.FindWithTag("EncounterManager");
        if(reg != null)
        {
            encounterSystem = reg.GetComponent<RandomEncounterGenerator>();
        }
        else
        {
            Debug.LogError("NoCombatZone present but RandomEncounterManager not found! This zone will disable itself");
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Player entered no combat zone");
        encounterSystem.enabled = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Player leaving no combat zone");
        encounterSystem.enabled = true;
    }
}
