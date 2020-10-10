using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventoryloader : MonoBehaviour
{
    public static bool GameIsInventory = false;
    public GameObject InventoryMenuUI;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (GameIsInventory)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    void Resume()
    {
        InventoryMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsInventory = false;
    }
    void Pause()
    {
        InventoryMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsInventory = true;
    }
}
