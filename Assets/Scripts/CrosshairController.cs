using System;
using UnityEngine;

[DisallowMultipleComponent]
[SelectionBase]
public class CrosshairController : MonoBehaviour
{
    public event Action<Vector3> OnSlingshotFired;

    public void Fire()
    {
        Debug.Log("Slingshot fired at position: " + transform.position, this);
        OnSlingshotFired?.Invoke(transform.position);
    }
}