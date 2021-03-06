﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [System.NonSerialized]
    public bool waitingForCoolDown;


    public GameObject otherEndOfTeleporter;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!waitingForCoolDown && !other.gameObject.tag.Contains("Player"))
        {
            other.transform.position = otherEndOfTeleporter.transform.position;
            waitingForCoolDown = true;
            otherEndOfTeleporter.GetComponent<Teleporter>().waitingForCoolDown = true;
            Invoke("ReactivateTeleporter", 1f); //reactivate the teleporter in 1 second
        }
    }

    public void ReactivateTeleporter()
    {
        waitingForCoolDown = false;
        otherEndOfTeleporter.GetComponent<Teleporter>().waitingForCoolDown = false;
    }


}
