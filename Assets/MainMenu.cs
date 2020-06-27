using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject yinPanel;
    public GameObject yangPanel;
    private bool flip; 

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            flip = !flip;
            yinPanel.SetActive(!flip);
            yangPanel.SetActive(flip);
           
        }
    }
}
