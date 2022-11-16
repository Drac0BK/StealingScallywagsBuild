using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityCore;
using UnityCore.Audio;


public class TestAudio : MonoBehaviour
{
    public AudioManager audioManager;
    [SerializeField]
    private AudioSource audioSource;
    private bool toggle;

    private void Start()
    {
        audioManager.RestartAudio(UnityCore.Audio.AudioType.BGM, "CanYouReview", false, 0.0f);        
    }

    private void Update()
    {
        Debug.Log (audioSource.isPlaying);

        if (audioSource.isPlaying == false)
        {
            if (toggle == true)
            {
                audioManager.PlayAudio(UnityCore.Audio.AudioType.BGM, "Main_Theme", false, 0.0f);
                toggle = false;
            }
            else
            {
                toggle = true;
            }
            
        }
    }

}

