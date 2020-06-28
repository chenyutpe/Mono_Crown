using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public Audio[] audios;

    void Awake ()
    {
        foreach (Audio a in audios)
        {
            a.source = gameObject.AddComponent<AudioSource>();
            a.source.clip = a.clip;
            a.source.outputAudioMixerGroup = a.mixerGroup;
            a.source.volume = a.volume;
            a.source.loop = a.loop;
        }
    }

    void Start ()
    {
        Play("BattleBGM");
    }

    public void Play (string audioName)
    {
        Audio a = Array.Find(audios, audio => audio.audioName == audioName);
        
        // not found
        if (a == null)
        {
            Debug.Log("Audio " + audioName + " not found");
            return;
        }

        a.source.Play();
    }

    public void Stop (string audioName)
    {
        Audio a = Array.Find(audios, audio => audio.audioName == audioName);
        
        // not found
        if (a == null)
        {
            Debug.Log("Audio " + audioName + " not found");
            return;
        }

        a.source.Stop();
    }
}
