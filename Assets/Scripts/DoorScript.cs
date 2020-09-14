using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public string level;
    public int door;
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag.Contains("Player"))
        {
            PlayerMovement pm = collision.gameObject.GetComponent<PlayerMovement>();
            pm.RequestGoToLevel(level, door);
        }
    }
}
