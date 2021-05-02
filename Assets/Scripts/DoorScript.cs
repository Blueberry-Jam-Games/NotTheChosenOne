using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public string level;
    public int door;

    private bool usedRecently = false;

    /*If this is null, the doors transform is used instead*/
    public Transform externalStart;

    public void MarkUsedRecently()
    {
        usedRecently = true;
        Debug.Log("Door to " + level + " marked as used.");
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(usedRecently)
        {
            Debug.Log("Door to " + level + " entered but ignored due to recent use");
        }
        else if(collision.gameObject.tag.Contains("Player"))
        {
            PlayerMovement pm = collision.gameObject.GetComponent<PlayerMovement>();
            pm.RequestGoToLevel(level, door);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        usedRecently = false;
        Debug.Log("Door to " + level + " now usable");
    }

    public void ResetDoorUsedRecently()
    {
        usedRecently = false;
    }

    public Vector3 GetPlayerSpawnPosition()
    {
        return externalStart == null ? transform.position : externalStart.position;
    }
}
