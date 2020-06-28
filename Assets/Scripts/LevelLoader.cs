using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;

    private bool isInTransition = false;
    private bool displayControl = false;

    public GameObject ControlMenu;


    private void Start()
    {
        // why didn't this work??? 
        if (transition == null) // ???????
            transition = GameObject.Find("FadeToBlack")?.GetComponent<Animator>();
    }

    private void Update()
    {
        // Because apparently they broke something and would like to undo.... Fools! haahaha
        if (Input.GetKeyDown(KeyCode.R))
            Restart();

        // Load the main menu in case the player gets upset and butthurt mad about how awesome our game is...
        if (Input.GetKeyDown(KeyCode.Escape))
            LoadMainMenu();

        // For those who doesn't know all of the control, might as well slap in their face and show them how this game works!
        if(ControlMenu != null )
        {
            displayControl = (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.RightAlt));
            if (isInTransition) // if we are in a transition, disable the control. 
                displayControl = false;
            ControlMenu.SetActive(displayControl);
        }
    }

    public void LoadMainMenu()
    {
        StartCoroutine(LoadLevel(0));
    }

    public void LoadNextLevel()
    {
        if(SceneManager.GetActiveScene().buildIndex >= SceneManager.sceneCount)
        {
            LoadMainMenu();
        }
        else
        {
            StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
        }
    }

    public void Restart()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        isInTransition = true;
        //???
        transition?.gameObject?.SetActive(true);
        transition?.SetTrigger("StartAnim");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);
    }
}
