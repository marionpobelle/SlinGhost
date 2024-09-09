using System;
using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
[SelectionBase]
public class CrosshairController : MonoBehaviour
{
    public event Action<Vector3> OnSlingshotFired;
    private bool _isInCooldown;
    private WaitForSeconds _cooldownInSeconds;

    private void Awake()
    {
        _cooldownInSeconds = new WaitForSeconds(Data.GameData.CooldownBetweenShotsInSeconds);
    }

    public void Fire()
    {
        if (_isInCooldown) return;
        Debug.Log("Slingshot fired at position: " + transform.position, this);
        OnSlingshotFired?.Invoke(transform.position);
        StartCoroutine(CooldownCoroutine());
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