using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipController : MonoBehaviour
{
    public GameObject flipAxis;
    public GameObject yinGroupHolder;
    public GameObject yangGroupHolder;
    public GameObject[] yinGroup;
    public GameObject[] yangGroup;

    private bool inFlipSequence;
    private float flipValue = -1;
    private float flipSpeed = 0.05f; //the smaller the slower the flip sequence
    private int flip = -1;

    public void Start() //gets the yin and yang groups based on the group holders children in the hierarchy
    {
        yinGroup = new GameObject[yinGroupHolder.transform.childCount];

        for (int i = 0; i < yinGroupHolder.transform.childCount; ++i)
            yinGroup[i] = yinGroupHolder.transform.GetChild(i).gameObject;

        yangGroup = new GameObject[yangGroupHolder.transform.childCount];

        for (int i = 0; i < yangGroupHolder.transform.childCount; ++i)
            yangGroup[i] = yangGroupHolder.transform.GetChild(i).gameObject;
    }

    public void FlipGravityOfGroups()
    {
        foreach (GameObject obj in yinGroup)
        {
            if (obj.GetComponent<FlipsGravity>())
            {
                obj.GetComponent<Rigidbody2D>().gravityScale = -obj.GetComponent<Rigidbody2D>().gravityScale;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            flipAxis.gameObject.transform.localScale = new Vector3(1, flip, 1);

            if (flip == 1)
                flip = -1;
            else
                flip = 1;

            FlipGravityOfGroups();
        }

        //pretty flip
        if (Input.GetKeyDown(KeyCode.RightShift))
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
            //updates pretty flip over time (.5 seconds?)
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

            FlipGravityOfGroups();
        }

    }
}
