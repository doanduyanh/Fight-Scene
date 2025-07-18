using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;
    public AudioSource[] audioSrc;
    private bool playing;
    private void Start()
    {
        if (GameRef.GetMusicState() == 0)
        {
            playing = true;
            foreach (AudioSource audio in audioSrc)
            {
                if (!audio.isPlaying)
                {
                    audio.Play();
                }
            }

        }
        else
        {
            playing = false;
            foreach (AudioSource audio in audioSrc)
            {
                if (audio.isPlaying)
                {
                    audio.Stop();
                }
            }
        }
        
    }

    public void UpdateMusicState()
    {
        if (GameRef.GetMusicState() == 0 && !playing)
        {
            playing = true;
            foreach (AudioSource audio in audioSrc)
            {
                if (!audio.isPlaying)
                {
                    audio.Play();
                }
            }

        }
        else if(GameRef.GetMusicState() != 0 && playing)
        {
            playing = false;
            foreach (AudioSource audio in audioSrc)
            {
                if (audio.isPlaying)
                {
                    audio.Stop();
                }
            }
        }
        
    }
}
