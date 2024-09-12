using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateProjectile : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 5;
    bool isRotating;
    Quaternion rotationDir;

    void Update()
    {
        if (!isRotating)
            return;

        transform.Rotate(rotationDir.eulerAngles * rotationSpeed * Time.deltaTime);
    }

    public void StartRotation()
    {
        isRotating = true;
        rotationDir = Random.rotation;
    }

    public void StopRotation()
    {
        isRotating = false;
        transform.rotation = Quaternion.identity;
    }
}
