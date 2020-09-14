using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorRegistry : MonoBehaviour
{
    public List<GameObject> doors;

    public GameObject RequestDoor(int id)
    {
        return doors[id];
    }
}
