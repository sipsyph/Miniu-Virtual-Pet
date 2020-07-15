using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class AudioManager : MonoBehaviour
{
    public AudioClip[] musicClips;
    private int currentTrack;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlayMusic();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayMusic()
    {
        if(audioSource.isPlaying)
        {
            return;
        }

        currentTrack--;

        if(currentTrack<0)
        {
            currentTrack = musicClips.Length - 1;
        }

        StartCoroutine(WaitForMusicEnd());
    }

    IEnumerator WaitForMusicEnd()
    {
        while(audioSource.isPlaying)
        {
            yield return null;
        }
        NextTitle();
    }

    public void NextTitle()
    {
        audioSource.Stop();
        currentTrack++;
        if(currentTrack > musicClips.Length - 1)
        {
            currentTrack = 0;
        }
        audioSource.clip = musicClips[currentTrack];
        audioSource.Play();

        StartCoroutine(WaitForMusicEnd());
    }
}
