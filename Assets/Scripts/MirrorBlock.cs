using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorBlock : MonoBehaviour
{
    public GameObject otherSideOfMirror;
    private Vector3 myLastPos;

    private void Start()
    {
        myLastPos = transform.position;
    }

    public void Update()
    {
        if (transform.position != myLastPos) //must have moved, so update the other side
        {
            myLastPos = transform.position;
            Vector3 newPos = myLastPos;
            newPos.y = -newPos.y; //flip the y
            otherSideOfMirror.transform.position = newPos;
        }
    }
}
