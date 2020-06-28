using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    public AudioSource walkingAudio;
    public AudioSource pickupClip;
    public AudioSource dropClip;
    public AudioSource jumpClip;
    public AudioSource negativeClip;

    public void PlayWalkingToggle(bool play)
    {
        if (play && !walkingAudio.isPlaying)
            walkingAudio.Play();
        else if (play == false)
            walkingAudio.Stop();
    }

    public void PlayPickup()
    {
        pickupClip.Play();
    }

    public void PlayDrop()
    {
        dropClip.Play();
    }

    public void PlayJump()
    {
        jumpClip.Play();
    }
    public void PlayNegative()
    {
        negativeClip.Play();
    }

}
