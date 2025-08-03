using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Musictracks
{
    public string trackName;
    public AudioClip clip;
}
public class MusicLibrary : MonoBehaviour
{
    public Musictracks[] tracks;

    public AudioClip GetClipByName(string nameTrack)
    {

        foreach (var track in tracks)
        {
            if (track.trackName == nameTrack)
            {
                return track.clip;
            } 
        }

        return null;
    }
}
