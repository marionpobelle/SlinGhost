using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Management;
using Valve.VR;

public class VRCrosshairProvider : MonoBehaviour
{
    [SerializeField] private Vector3 _offsetPosition; // Saved in PlayerPrefs
    [SerializeField] private Vector3 _offsetRotation; // Saved in PlayerPrefs
    [SerializeField] private Vector3 _position;
    [SerializeField] private Vector3 _rotation;
    [SerializeField] private float _fireThreshold = 0.1f;
    [SerializeField] private float _cooldown = 1.0f;
    [SerializeField] private Vector3 _currentVelocity;
    [SerializeField] private float _currentVelocityMagnitude;
    [SerializeField] private float _currentStrechAmount; // Between 0 and 1
    [SerializeField] private Vector3 _idlePosition; // Saved in PlayerPrefs
    [SerializeField] private Vector3 _fullStretchPosition; // Saved in PlayerPrefs
    private CrosshairController _crosshairController;
    private SteamVR_Behaviour_Pose _pose;
    private Vector3 _previousPosition;
    private float _lastFireTime;
    private VRControllerRaycastOrigin _raycastOrigin;

    private void Start()
    {
        if( XRGeneralSettings.Instance.Manager.activeLoader )
        {
            XRGeneralSettings.Instance.Manager.StopSubsystems();
            XRGeneralSettings.Instance.Manager.DeinitializeLoader();
        }
        Debug.Log("Initializing XR SDK.");
        XRGeneralSettings.Instance.Manager.InitializeLoaderSync();
        XRGeneralSettings.Instance.Manager.StartSubsystems();
        Debug.Log("XR SDK initialized.", this);
        
        
        _crosshairController = FindObjectOfType<CrosshairController>();
        if (!_crosshairController)
        {
            Debug.LogError("CrosshairController not found in the scene.", this);
        }
        
        _pose = FindObjectOfType<SteamVR_Behaviour_Pose>();
        if (!_pose)
        {
            Debug.LogError("SteamVR_Behaviour_Pose not found in the scene.", this);
        }
        
        _raycastOrigin = FindObjectOfType<VRControllerRaycastOrigin>();
        if (!_raycastOrigin)
        {
            Debug.LogError("VRControllerRaycastOrigin not found in the scene.", this);
        }

        _previousPosition = _pose.transform.position;
        _lastFireTime = -_cooldown; // Initialize to allow immediate firing
        
        LoadOffset();
        LoadIdlePosition();
        LoadFullStretchPosition();
    }

    private void OnApplicationQuit()
    {
        // Clean all subsystems
        Debug.Log("Deinitializing XR SDK.");
        XRGeneralSettings.Instance.Manager.StopSubsystems();
        XRGeneralSettings.Instance.Manager.DeinitializeLoader();
    }

    private void OnDestroy()
    {
        // stop all XR subsystems
    }

    private void Update()
    {
        UpdateCrosshairPosition();
        UpdateFiring();
        UpdateOffset();
        UpdateStretch();
    }
    
    private void UpdateOffset()
    {
        if (Keyboard.current.downArrowKey.isPressed)
        {
            // Add 0.1 units to the x rotation
            transform.Rotate(0.1f, 0, 0);
        }
        else if (Keyboard.current.upArrowKey.isPressed)
        {
            // Subtract 0.1 units from the x rotation
            transform.Rotate(-0.1f, 0, 0);
        }
        if (Keyboard.current.rightArrowKey.isPressed)
        {
            // Add 0.1 units to the y rotation
            transform.Rotate(0, 0.1f, 0);
        }
        else if (Keyboard.current.leftArrowKey.isPressed)
        {
            // Subtract 0.1 units from the y rotation
            transform.Rotate(0, -0.1f, 0);
        }

        if (Keyboard.current.leftShiftKey.isPressed) // RECORD WITH LEFT SHIFT
        {
            if (Keyboard.current.oKey.wasPressedThisFrame)
            {
                SaveOffset();
            }
            else if (Keyboard.current.iKey.wasPressedThisFrame)
            {
                SaveIdlePosition();
            }
            else if (Keyboard.current.fKey.wasPressedThisFrame)
            {
                SaveFullStretchPosition();
            }
        }
        else if (Keyboard.current.rightShiftKey.isPressed) // LOAD WITH RIGHT SHIFT
        {
            if (Keyboard.current.oKey.wasPressedThisFrame)
            {
                LoadOffset();
            }
            else if (Keyboard.current.iKey.wasPressedThisFrame)
            {
                LoadIdlePosition();
            }
            else if (Keyboard.current.fKey.wasPressedThisFrame)
            {
                LoadFullStretchPosition();
            }
        }
    }
    
    private void UpdateCrosshairPosition()
    {
        // Do a raycast from the VR controller to determine the position of the crosshair
        if (Physics.Raycast(_raycastOrigin.transform.position, _raycastOrigin.transform.forward, out var hit))
        {
            _crosshairController.transform.position = hit.point;
        }
        else
        {
            // Do nothing, the last crosshair position should be at the edge of the plane, so it's fine
        }
    }

    private void UpdateFiring()
    {
        _position = _pose.transform.position;
        _rotation = _pose.transform.eulerAngles;
        
        float deltaTime = Time.deltaTime;
        _currentVelocity = (_position - _previousPosition) / deltaTime;
        _currentVelocityMagnitude = _currentVelocity.magnitude;

        if (_currentVelocity.magnitude > _fireThreshold && Time.time - _lastFireTime >= _cooldown)
        {
            FireDetected();
            _lastFireTime = Time.time;
        }

        _previousPosition = _position;
    }
    
    private void UpdateStretch()
    {
        float distanceFromIdleToController = Vector3.Distance(_idlePosition, _pose.transform.position);
        float distanceFromIdleToFullStretch = Vector3.Distance(_idlePosition, _fullStretchPosition);
        _currentStrechAmount = distanceFromIdleToController / distanceFromIdleToFullStretch;
        _currentStrechAmount = Mathf.Clamp(_currentStrechAmount, 0, 1);
    }
    
    private void FireDetected()
    {
        // Implement the logic for when a fire is detected
        Debug.Log("Fire detected!");
    }

    private void OnDrawGizmos()
    {
        if (!_raycastOrigin) return;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(_raycastOrigin.transform.position, _raycastOrigin.transform.position + _raycastOrigin.transform.forward * 50);
    }
    
    private void SaveOffset()
    {
        _offsetPosition = transform.position;
        _offsetRotation = transform.eulerAngles;
        
        PlayerPrefs.SetFloat("OffsetPositionX", _offsetPosition.x);
        PlayerPrefs.SetFloat("OffsetPositionY", _offsetPosition.y);
        PlayerPrefs.SetFloat("OffsetPositionZ", _offsetPosition.z);
        
        PlayerPrefs.SetFloat("OffsetRotationX", _offsetRotation.x);
        PlayerPrefs.SetFloat("OffsetRotationY", _offsetRotation.y);
        PlayerPrefs.SetFloat("OffsetRotationZ", _offsetRotation.z);
        
        Debug.Log("Offset position and rotation saved in PlayerPrefs for the VR Controller.", this);
    }
    
    private void LoadOffset()
    {
        // Check if the key exists in PlayerPrefs
        if (!PlayerPrefs.HasKey("OffsetPositionX"))
        {
            Debug.LogWarning("Offset position not found in PlayerPrefs for the VR Controller. This is normal if its the first time you're trying to load the offset position.", this);
            return;
        }
        
        _offsetPosition.x = PlayerPrefs.GetFloat("OffsetPositionX", 0);
        _offsetPosition.y = PlayerPrefs.GetFloat("OffsetPositionY", 0);
        _offsetPosition.z = PlayerPrefs.GetFloat("OffsetPositionZ", 0);
        
        _offsetRotation.x = PlayerPrefs.GetFloat("OffsetRotationX", 0);
        _offsetRotation.y = PlayerPrefs.GetFloat("OffsetRotationY", 0);
        _offsetRotation.z = PlayerPrefs.GetFloat("OffsetRotationZ", 0);
        
        transform.position = _offsetPosition;
        transform.eulerAngles = _offsetRotation;
        
        Debug.Log("Offset position and rotation loaded from PlayerPrefs for the VR Controller.", this);
    }
    
    private void SaveIdlePosition()
    {
        _idlePosition = _pose.transform.position;
        
        PlayerPrefs.SetFloat("IdlePositionX", _idlePosition.x);
        PlayerPrefs.SetFloat("IdlePositionY", _idlePosition.y);
        PlayerPrefs.SetFloat("IdlePositionZ", _idlePosition.z);
        
        Debug.Log("Idle position saved in PlayerPrefs for the VR Controller.", this);
    }
    
    private void LoadIdlePosition()
    {
        // Check if the key exists in PlayerPrefs
        if (!PlayerPrefs.HasKey("IdlePositionX"))
        {
            Debug.LogWarning("Idle position not found in PlayerPrefs for the VR Controller. This is normal if its the first time you're trying to load the idle position.", this);
            return;
        }
        
        _idlePosition.x = PlayerPrefs.GetFloat("IdlePositionX", 0);
        _idlePosition.y = PlayerPrefs.GetFloat("IdlePositionY", 0);
        _idlePosition.z = PlayerPrefs.GetFloat("IdlePositionZ", 0);
        
        Debug.Log("Idle position loaded from PlayerPrefs for the VR Controller.", this);
    }
    
    private void SaveFullStretchPosition()
    {
        _fullStretchPosition = _pose.transform.position;
        
        PlayerPrefs.SetFloat("FullStretchPositionX", _fullStretchPosition.x);
        PlayerPrefs.SetFloat("FullStretchPositionY", _fullStretchPosition.y);
        PlayerPrefs.SetFloat("FullStretchPositionZ", _fullStretchPosition.z);
        
        Debug.Log("Full stretch position saved in PlayerPrefs for the VR Controller.", this);
    }
    
    private void LoadFullStretchPosition()
    {
        // Check if the key exists in PlayerPrefs
        if (!PlayerPrefs.HasKey("FullStretchPositionX"))
        {
            Debug.LogWarning("Full stretch position not found in PlayerPrefs for the VR Controller. This is normal if its the first time you're trying to load the full stretch position.", this);
            return;
        }
        
        _fullStretchPosition.x = PlayerPrefs.GetFloat("FullStretchPositionX", 0);
        _fullStretchPosition.y = PlayerPrefs.GetFloat("FullStretchPositionY", 0);
        _fullStretchPosition.z = PlayerPrefs.GetFloat("FullStretchPositionZ", 0);
        
        Debug.Log("Full stretch position loaded from PlayerPrefs for the VR Controller.", this);
    }
}