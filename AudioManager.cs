using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;

    public AudioSource audioSource;
    public AudioClip[] audioClips;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayClick()
    {
        audioSource.clip = audioClips[0];

        //randomize pitch
        audioSource.pitch = Random.Range(1f, 1.1f);


        audioSource.Play();
    }

    public void PlayMatch()
    {
        audioSource.clip = audioClips[1];

        //randomize pitch
        audioSource.pitch = Random.Range(1f, 1.1f);


        audioSource.Play();

    }
}
