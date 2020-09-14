using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    GameObject player;        //Public variable to store a reference to the player game object
    public float maxOffsetX = 1;
    public float maxOffsetY = .5f;

    private Vector3 offset;            //Private variable to store the offset distance between the player and camera

    // Use this for initialization
    void Start()
    {
        if (player != null)
        {
            //Calculate and store the offset value by getting the distance between the player's position and camera's position.
            offset = transform.position - player.transform.position;
        }
    }

    // LateUpdate is called after Update each frame
    void LateUpdate()
    {
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            offset = transform.position - player.transform.position;
        }

        float newX = transform.position.x;
        float newY = transform.position.y;

        // Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
        //transform.position = player.transform.position + offset;
        if (Mathf.Abs(player.transform.position.x - transform.position.x) > maxOffsetX)
        {
            if (player.transform.position.x > transform.position.x)
            {
                newX = player.transform.position.x - maxOffsetX;
            }
            else
            {
                newX = player.transform.position.x + maxOffsetX;
            }
        }

        if (Mathf.Abs(player.transform.position.y - transform.position.y) > maxOffsetY)
        {
            if (player.transform.position.y > transform.position.y)
            {
                newY = player.transform.position.y - maxOffsetY;
            }
            else
            {
                newY = player.transform.position.y + maxOffsetY;
            }
        }

        transform.SetPositionAndRotation(new Vector3(newX, newY, transform.position.z), transform.rotation);
    }
}
