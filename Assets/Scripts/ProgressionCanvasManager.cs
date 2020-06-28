using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ProgressionCanvasManager : MonoBehaviour
{
    public TextMeshProUGUI textToUpdate = null;
    private int currentIndex = 0;
    private int world = 0;
    private int level = 0;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void Update()
    {
        if(SceneManager.GetActiveScene().buildIndex != currentIndex)
        {
            currentIndex = SceneManager.GetActiveScene().buildIndex;
            world = (int)Mathf.Ceil(currentIndex/5);

            if(currentIndex <= 5)
            {
                level = currentIndex;
            }
            else
            {
                level = ( currentIndex % 5 );
            }
            

            //Debug.Log("World: " + world + " Level: " + level);
            string str = "";
            str +=$"World: {world} \t Level: {level} / 5";
            textToUpdate.text = str;
        }
    }
}