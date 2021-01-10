using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    [SerializeField]
    public List<BackgroundSong> backgroundMusic;

    private Dictionary<string, BackgroundSong> songmap;

    public string currentTrack = "";

    private void Awake()
    {
        songmap = new Dictionary<string, BackgroundSong>();
        foreach(BackgroundSong bgs in backgroundMusic)
        {
            bgs.AddSource(gameObject.AddComponent<AudioSource>());
            songmap.Add(bgs.name, bgs);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void PlayMusic(string name)
    {
        if(currentTrack == name)
        {
            Debug.Log("Track " + name + " already playing, ignoring request.");
        } 
        else if(!songmap.ContainsKey(name))
        {
            Debug.LogError("Track " + name + " not found in sound manager. Did you edit the prefab?");
        }
        else if(currentTrack == "")
        {
            Debug.Log("Starting music with track " + name);
            currentTrack = name;
            songmap[currentTrack].source.Play();
        }
        else
        {
            Debug.Log("Transitioning from song " + currentTrack + " to " + name);
            StartCoroutine(SwitchSong(name));
        }
    }

    private IEnumerator SwitchSong(string songto)
    {
        BackgroundSong current = songmap[currentTrack];
        for(float i = 1; i > 0; i-=1.0f/30.0f)
        {
            current.source.volume = i;
            yield return null;
        }
        current.source.volume = 0.0f;
        if(current.resetOnEnd)
        {
            current.source.Stop();
        }
        else
        {
            current.source.Pause();
        }
        currentTrack = songto;
        songmap[currentTrack].source.volume = 1.0f;
        songmap[currentTrack].source.Play();
    }
}

[System.Serializable]
public class BackgroundSong
{
    public string name;

    public AudioClip clip;

    public float volumeOverride = 1.0f;

    public bool resetOnEnd = false;

    [HideInInspector]
    public AudioSource source;

    public void AddSource(AudioSource source)
    {
        this.source = source;
        source.clip = this.clip;
        source.volume *= volumeOverride;
        source.loop = true;
    }
}
