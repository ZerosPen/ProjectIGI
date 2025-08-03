using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] private SoundLibary soundLibraryEffect;
    [SerializeField] private AudioSource SoundFX2D;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlaySound2D(string soundName)
    {
        SoundFX2D.PlayOneShot(soundLibraryEffect.GetAudioClipFromName(soundName));
            
    }

}
