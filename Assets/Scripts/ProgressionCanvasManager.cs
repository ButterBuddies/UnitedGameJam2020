﻿using System.Collections;
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
 
            world = (int)((float)currentIndex / (float)5);

            if((currentIndex / 6) > 1)
            {
                level = ((currentIndex + 1) % 6);
            }
            else
            {
                level = ((currentIndex) % 6);
            }

            //Debug.Log("World: " + world + " Level: " + level);
            string str = "";
            str +=$"World: {world} \t Level: {level} / 5";
            textToUpdate.text = str;
        }
    }
}