using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    private CrosshairController _crosshairController;

    private void Start()
    {
        _crosshairController = FindObjectOfType<CrosshairController>();
        //_crosshairController.OnSlingshotFired += SlingshotFired;
    }

    private void SlingshotFired()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    private void OnDestroy()
    {
        //_crosshairController.OnSlingshotFired -= SlingshotFired;
    }
}
