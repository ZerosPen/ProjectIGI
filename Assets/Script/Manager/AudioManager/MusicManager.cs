using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Timeline.TimelineAsset;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    [SerializeField] private MusicLibrary musicLibrary;
    [SerializeField] private AudioSource musicSource;

    private void Awake()
    {
        if (instance != null) Destroy(gameObject); 

        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void playMusic(string nameMusic, float durationFade = 0.2f )
    {
        StartCoroutine(AnimateMusiCrossFade(musicLibrary.GetClipByName(nameMusic), durationFade));
    }

    IEnumerator AnimateMusiCrossFade(AudioClip nextTrack, float durationFade = 0.5f)
    {
        float percent = 0f;
        while (percent < 1)
        {
            percent += Time.deltaTime * 1 / durationFade;
            musicSource.volume = Mathf.Lerp(1f, 0, percent);
            yield return null;
        }

        musicSource.clip = nextTrack;
        musicSource.Play();

        percent = 0f;
        while (percent < 1)
        {
            percent += Time.deltaTime * 1 / durationFade;
            musicSource.volume = Mathf.Lerp(0, 1f, percent);
            yield return null;
        }
    }
}
