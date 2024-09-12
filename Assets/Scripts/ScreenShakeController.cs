using System;
using System.Collections;
using System.Security.Cryptography;
using Unity.Cinemachine;
using UnityEngine;

/// <summary>
/// This class uses Cinemachine impulse source to shake the camera.
/// - When the player shoots the slingshot, the camera shakes big.
/// - When the player is stretching the slingshot, the camera shakes.
/// </summary>
public class ScreenShakeController : MonoBehaviour
{
    [SerializeField] private CinemachineImpulseSource _stretchImpulseSource;
    [SerializeField] private CinemachineImpulseSource _fireImpulseSource;
    [SerializeField] private float _secondsBeforeStretchImpulseAfterFire = 1f;
    
    private CrosshairController _crosshairController;
    private Coroutine _stretchCoroutine;
    private WaitForSeconds _stretchImpulseDuration;
    private WaitForSeconds _secondsBeforeStretchImpulseAfterFireWait;
    private bool _stretchImpulseActive = true;
    
    private void Start()
    {
        _crosshairController = FindObjectOfType<CrosshairController>();
        if (!_crosshairController)
        {
            Debug.LogError("Couldn't find CrosshairController in the scene !", this);
        }
        else
        {
            _crosshairController.OnSlingshotFired += CrosshairControllerOnSlingshotFired;
        }
        
        if (!_stretchImpulseSource)
        {
            Debug.LogError("CinemachineImpulseSource for stretching not set !", this);
        }
        else
        {
            _stretchImpulseDuration = new WaitForSeconds(_stretchImpulseSource.ImpulseDefinition.ImpulseDuration);
            _stretchCoroutine = StartCoroutine(StretchShakeCoroutine());
        }
        
        if (!_fireImpulseSource)
        {
            Debug.LogError("CinemachineImpulseSource for firing not set !", this);
        }
        
        _secondsBeforeStretchImpulseAfterFireWait = new WaitForSeconds(_secondsBeforeStretchImpulseAfterFire);
    }

    private void CrosshairControllerOnSlingshotFired(Vector3 _)
    {
        _fireImpulseSource.GenerateImpulse();
        StartCoroutine(StretchImpulseAfterFireCoroutine());
    }

    private void Update()
    {
        if (_crosshairController && _stretchImpulseSource)
        {
            _stretchImpulseSource.ImpulseDefinition.AmplitudeGain = _crosshairController.CurrentStretchAmout;
        }
        else
        {
            _stretchImpulseSource.ImpulseDefinition.AmplitudeGain = 0;
        }
    }

    private void OnDestroy()
    {
        if (_stretchCoroutine != null)
        {
            StopCoroutine(_stretchCoroutine);
        }
        _crosshairController.OnSlingshotFired -= CrosshairControllerOnSlingshotFired;
    }

    private IEnumerator StretchShakeCoroutine()
    {
        while (true)
        {
            // If the current stretch amount is less than 0.15, don't shake the camera
            // Because when releasing the slingshot the value will oscillate a bit
            if (_crosshairController.CurrentStretchAmout < .15f || !_stretchImpulseActive)
            {
                yield return null;
                continue;
            }
            _stretchImpulseSource.GenerateImpulse(_crosshairController.CurrentStretchAmout);
            yield return _stretchImpulseDuration;
        }
    }
    
    private IEnumerator StretchImpulseAfterFireCoroutine()
    {
        if (!_stretchImpulseActive) yield break;
        _stretchImpulseActive = false;
        yield return _secondsBeforeStretchImpulseAfterFireWait;
        _stretchImpulseActive = true;
    }
}