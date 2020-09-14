using System.Collections.Generic;
using UnityEngine;

public class PersistanceController : MonoBehaviour
{

    [SerializeField]
    public List<PersistanceEntry> persistantObjects;

    // Start is called before the first frame update
    void Start()
    {
        foreach (PersistanceEntry pe in persistantObjects)
        {
            if(GameObject.FindGameObjectWithTag(pe.tag) == null)
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
