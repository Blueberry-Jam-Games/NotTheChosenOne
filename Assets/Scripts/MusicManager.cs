using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    public BackgroundSong[] backgroundMusic;

    public int currentTrack = -1;

    private void Awake()
    {
        foreach(BackgroundSong bgs in backgroundMusic)
        {
            bgs.AddSource(gameObject.AddComponent<AudioSource>());
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void PlayMusic(string name)
    {
        if (currentTrack == -1)
        {
            BackgroundSong bgs = Array.Find(backgroundMusic, music=>music.name == name);
            bgs.source.Play();
        }
        else
        {
            //Fade this track into next one.
        }
    }
}

[System.Serializable]
public class BackgroundSong
{
    public string name;

    public AudioClip clip;

    public int volumeOverride;

    [HideInInspector]
    public AudioSource source;

    public void AddSource(AudioSource source)
    {
        this.source = source;
        source.clip = this.clip;
        source.volume *= volumeOverride;
    }
}
