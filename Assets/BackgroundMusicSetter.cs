using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundMusicSetter : MonoBehaviour
{
    public AudioClip[] musicArray = new AudioClip[11];
    public AudioSource audioSource;
    int sceneNumber;

    // Start is called before the first frame update
    void Start()
    {
        sceneNumber = SceneManager.GetActiveScene().buildIndex;
        audioSource.clip = musicArray[sceneNumber];
        audioSource.Play();
    }
}
