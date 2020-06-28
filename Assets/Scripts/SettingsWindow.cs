using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsWindow : MonoBehaviour
{
    [SerializeField] AudioMixer mainMixer;

    public void SetMainVolume (float vol)
    {
        mainMixer.SetFloat("MasterVolume", Mathf.Log10(vol) * 20);
    }
    public void SetBGMVolume (float vol)
    {
        mainMixer.SetFloat("BGMVolume", Mathf.Log10(vol) * 20);
    }
    public void SetSEVolume (float vol)
    {
        mainMixer.SetFloat("SEVolume", Mathf.Log10(vol) * 20);
    }

    public void SetFullScreen (bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }
}