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
        //IF ENEMY HIT
        //Game won
        //IF ENEMY MISSED
        //Move enemy
        //Check if game is ended
    }

    private void OnDestroy()
    {
        //_crosshairController.OnSlingshotFired -= SlingshotFired;
    }
}
