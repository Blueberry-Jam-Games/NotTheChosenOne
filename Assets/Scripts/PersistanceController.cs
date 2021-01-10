using System.Collections.Generic;
using UnityEngine;

public class PersistanceController : MonoBehaviour
{

    [SerializeField]
    public List<PersistanceEntry> awakePersistantObjects; // Objects that can/should be created in the awake function (music manager)

    [SerializeField]
    public List<PersistanceEntry> startPersistantObjects; // Objects we shouldn' check/create until the start function (player)

    // Start is called before the first frame update
    void Awake()
    {
        foreach (PersistanceEntry pe in awakePersistantObjects)
        {
            if(GameObject.FindGameObjectWithTag(pe.tag) == null)
            {
                Instantiate(pe.target);
            }
        }
        //Destroy(this.gameObject);
    }

    private void Start()
    {
        foreach (PersistanceEntry pe in startPersistantObjects)
        {
            if (GameObject.FindGameObjectWithTag(pe.tag) == null)
            {
                Instantiate(pe.target);
            }
        }
        Destroy(this.gameObject);
    }
}

[System.Serializable]
public struct PersistanceEntry
{
    public string tag;
    public GameObject target;
}
