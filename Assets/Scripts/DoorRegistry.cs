using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorRegistry : MonoBehaviour
{
    public List<DoorScript> doors;

    public DoorScript RequestDoor(int id)
    {
        return doors[id];
    }
}
