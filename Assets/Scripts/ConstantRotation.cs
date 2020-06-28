using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantRotation : MonoBehaviour
{
    public float rotationSpeed = 1f;

    private void Update()
    {
        this.gameObject.transform.Rotate(Vector3.forward * (rotationSpeed * Time.deltaTime));
    }
}
