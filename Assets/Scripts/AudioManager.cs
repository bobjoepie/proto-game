using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //public AudioSource audioData;
    public AudioSource backgroundMusic;
    // Start is called before the first frame update
    void Start()
    {
        backgroundMusic = GetComponent<AudioSource>();
        backgroundMusic.Play();
    }

    public void PlaySound(AudioSource audio)
    {
        audio.Play();
    }
}
