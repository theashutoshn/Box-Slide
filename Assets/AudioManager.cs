using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioClip buttonClickClip;
    public AudioClip boxTileClickClip;

    private AudioSource effectAudioSource;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        effectAudioSource = gameObject.AddComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayButtonClick()
    {
        Debug.Log("PlayButtonClick called");
        effectAudioSource.PlayOneShot(buttonClickClip);
    }

    public void PlayBoxTileClick()
    {
        Debug.Log("PlayBoxTileClick called");
        effectAudioSource.PlayOneShot(boxTileClickClip);
    }
}
