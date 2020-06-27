using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    private static bool player1InGoal;
    private static bool player2InGoal;


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player1")
            player1InGoal = true;
        if (other.gameObject.tag == "Player2")
            player2InGoal = true;

        if (player1InGoal && player2InGoal)
            Debug.Log("WOW both players touching the goal, you win!");
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player1")
            player1InGoal = false;
        if (other.gameObject.tag == "Player2")
            player2InGoal = false;
    }
}
