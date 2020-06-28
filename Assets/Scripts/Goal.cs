using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] GameObject winScreen;

    private static bool player1InGoal;
    private static bool player2InGoal;
    private AudioSource audio;

    public void Start()
    {
        audio = GetComponent<AudioSource>();
        player1InGoal = false;
        player2InGoal = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player1")
        {
            player1InGoal = true;
            audio.Play();
        }
        if (other.gameObject.tag == "Player2")
        {
            player2InGoal = true;
            audio.Play();
        }

        if (player1InGoal && player2InGoal)
            winScreen.SetActive(true);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player1")
            player1InGoal = false;
        if (other.gameObject.tag == "Player2")
            player2InGoal = false;
    }
}
