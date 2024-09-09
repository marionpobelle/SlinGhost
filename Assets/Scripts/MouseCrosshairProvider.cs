using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Sets the position of the crosshair controller to the position of the mouse cursor on the plane.
/// It only used for debugging purposes.
/// </summary>
public class MouseCrosshairProvider : MonoBehaviour
{
    private CrosshairController _crosshairController;
    private Camera _mainCamera;
    
    private bool _isInCooldown;
    private WaitForSeconds _cooldownInSeconds;

    private void Awake()
    {
        _cooldownInSeconds = new WaitForSeconds(Data.GameData.CooldownBetweenShotsInSeconds);
    }

    private void Start()
    {
        _crosshairController = FindObjectOfType<CrosshairController>();
        if (!_crosshairController)
        {
            Debug.LogError("CrosshairController not found in the scene.", this);
        }
        _mainCamera = Camera.main;
        if (!_mainCamera)
        {
            Debug.LogError("Main camera not found in the scene.", this);
        }
    }

    private void Update()
    {
        if (!_crosshairController) return;
        // Get the mouse position
        var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit))
        {
            // Set the crosshair position to the hit point
            _crosshairController.transform.position = hit.point;
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (_isInCooldown) return;
            _crosshairController.Fire();
            StartCoroutine(CooldownCoroutine());
        }
    }
    
    private IEnumerator CooldownCoroutine()
    {
        if (_isInCooldown) yield break;
        Debug.Log($"Slingshot is in cooldown, waiting {Data.GameData.CooldownBetweenShotsInSeconds} seconds...", this);
        _isInCooldown = true;
        yield return _cooldownInSeconds;
        Debug.Log("Slingshot is ready to fire.", this);
        _isInCooldown = false;
    }
}