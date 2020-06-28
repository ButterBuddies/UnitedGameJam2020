using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ProgressionCanvasManager : MonoBehaviour
{
    public TextMeshProUGUI textToUpdate = null;
    private int currentIndex = 0;
    private char world;
    private char level;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void Update()
    {
        if(SceneManager.GetActiveScene().buildIndex != currentIndex)
        {
            currentIndex = SceneManager.GetActiveScene().buildIndex;
            Debug.Log(SceneManager.GetActiveScene().name);
            level = SceneManager.GetActiveScene().name[7];
            world = SceneManager.GetActiveScene().name[5];
            
            textToUpdate.text = $"World: {world} \t Level: {level} / 5";
        }
    }
}