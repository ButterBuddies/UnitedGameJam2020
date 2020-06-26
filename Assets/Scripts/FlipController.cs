using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipController : MonoBehaviour
{
    public GameObject flipAxis;
    public GameObject yinGroup;
    public GameObject yangGroup;

    private bool inFlipSequence;
    private float flipValue = 1;
    private float flipSpeed = 0.05f; //the smaller the slower the flip sequence
    private int flip = 1;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            flipAxis.gameObject.transform.localScale = new Vector3(1, flip, 1);

            if (flip == 1)
                flip = -1;
            else
                flip = 1;
        }

        //pretty flip
        if (Input.GetKeyDown(KeyCode.RightControl))
        {
            //flipAxis.gameObject.transform.localScale = new Vector3(1, flip, 1);
            inFlipSequence = true;

            if (flip == 1)
                flip = -1;
            else
                flip = 1;
        }

        if (inFlipSequence)
        {
            //updates pretyy flip over time (.5 seconds?)
            if (flip == 1)
            {
                if (flipValue < 1)
                {
                    flipValue += flipSpeed;
                    flipAxis.gameObject.transform.localScale = new Vector3(1, flipValue, 1);
                }
                else
                {   //done with flip sequence
                    inFlipSequence = false;
                }

            }
            else // (flip == -1)
            {
                if (flipValue > -1)
                {
                    flipValue -= flipSpeed;
                    flipAxis.gameObject.transform.localScale = new Vector3(1, flipValue, 1);
                }
                else
                {   //done with flip sequence
                    inFlipSequence = false;
                }

            }
        }

    }
}
