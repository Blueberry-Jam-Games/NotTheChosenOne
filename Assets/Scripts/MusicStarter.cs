using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicStarter : MonoBehaviour
{
    public string track;

    public bool destroyOnSuccess = true;

    private bool musicStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        GameObject musicManager = GameObject.FindWithTag("MusicManager");
        if(musicManager != null)
        {
            Debug.Log("Requesting background music " + track + " to start");
            MusicManager mm = musicManager.GetComponent<MusicManager>();
            mm.PlayMusic(track);
            musicStarted = true;
            if (destroyOnSuccess)
            {
                Destroy(gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!musicStarted)
        {
            GameObject musicManager = GameObject.FindWithTag("MusicManager");
            if (musicManager != null)
            {
                Debug.Log("Requesting background music " + track + " to start");
                MusicManager mm = musicManager.GetComponent<MusicManager>();
                mm.PlayMusic(track);
                musicStarted = true;
                if (destroyOnSuccess)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
