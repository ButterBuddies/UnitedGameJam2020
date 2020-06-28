using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    [SerializeField] GameObject winScreen;
    /// <summary>
    /// Skip showing the win screen and jump to the next level instead.
    /// </summary>
    public bool SkipShowingWinScreen = false;

    private static bool player1InGoal;
    private static bool player2InGoal;
    private new AudioSource audio;  

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
            if( audio != null )
                audio.Play();
        }
        if (other.gameObject.tag == "Player2")
        {
            player2InGoal = true;
            if (audio != null)
                audio.Play();
        }

        if(SkipShowingWinScreen && player1InGoal && player2InGoal)
        {
            // go ahead and load the next level without showing the winScreen...
            SceneManager.LoadScene(1);
            // simply skip the below code because why not?
            return;
        }

        if (player1InGoal && player2InGoal)
        {
            winScreen.SetActive(true);
        }
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            winScreen.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player1")
            player1InGoal = false;
        if (other.gameObject.tag == "Player2")
            player2InGoal = false;
    }
}
