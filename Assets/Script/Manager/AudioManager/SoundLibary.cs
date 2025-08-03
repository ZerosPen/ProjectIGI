using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SoundEffect
{
    public string groupId;
    public AudioClip[] clips;
}

public class SoundLibary : MonoBehaviour
{
    public SoundEffect[] soundEffects;

    public AudioClip GetAudioClipFromName(string name)
    {
        foreach (var soundEffect in soundEffects)
        {
            if (soundEffect.groupId == name)
            {
                return soundEffect.clips[Random.Range(0, soundEffect.clips.Length)];
            }
        }
        return null;
    }
}
