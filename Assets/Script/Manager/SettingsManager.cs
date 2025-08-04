using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public AudioMixer audioMaster;

    public Slider musicVolume;
    public Slider volumeVolume;

    public void UpdateMusicVolume(float volume)
    {
        audioMaster.SetFloat("MusicVolume", volume);
    }

    public void UpdateSoundVolume(float volume)
    {
        audioMaster.SetFloat("SoundVolume", volume);
    }

    public void SaveSettings()
    {
        audioMaster.GetFloat("MusicVolume", out float musicVolume);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);

        audioMaster.GetFloat("SoundVolume", out float soundVolume);
        PlayerPrefs.SetFloat("SoundVolume", soundVolume);
    }

    public void LoadSettings()
    {
        musicVolume.value = PlayerPrefs.GetFloat("MusicVolume");
        volumeVolume.value = PlayerPrefs.GetFloat("SoundVolume");
    }
}
