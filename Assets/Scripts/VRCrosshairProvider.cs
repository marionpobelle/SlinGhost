using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Valve.VR;

public class VRCrosshairProvider : MonoBehaviour
{
    [SerializeField] private Vector3 _position;
    [SerializeField] private Vector3 _rotation;
    [SerializeField] private float _fireThreshold = 0.1f;
    [SerializeField] private float _cooldown = 1.0f;
    [SerializeField] private Vector3 _currentVelocity;
    [SerializeField] private float _currentVelocityMagnitude;
    [SerializeField] private SteamVR_Behaviour_Boolean _calibrateButton;
    [SerializeField] private SteamVR_Behaviour_Vector2 _touchpad;
    private CrosshairController _crosshairController;
    private SteamVR_Behaviour_Pose _pose;
    private Vector3 _previousPosition;
    private float _lastFireTime;
    private VRControllerRaycastOrigin _raycastOrigin;

    private void Awake()
    {
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
        
        if (!_calibrateButton)
        {
            Debug.LogError("SteamVR_Behaviour_Boolean : Calibrate button not found in the scene.", this);
        }
        
        if (!_touchpad)
        {
            Debug.LogError("SteamVR_Behaviour_Vector2 : Touchpad not found in the scene.", this);
        }

        _previousPosition = _pose.transform.position;
        _lastFireTime = -_cooldown; // Initialize to allow immediate firing
    }

    private void Start()
    {
        // Subscribe to the button press event
        _calibrateButton.onPressDown.AddListener(OnCalibrate);
        _touchpad.onChange.AddListener(OnTouchpadAxis);
    }
    
    private void OnDestroy()
    {
        // Unsubscribe from the button press event
        _calibrateButton.onPressDown.RemoveListener(OnCalibrate);
        _touchpad.onChange.RemoveListener(OnTouchpadAxis);
    }
    
    private void Update()
    {
        UpdateCrosshairPosition();
        UpdateFiring();
        UpdateOffset();
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
    }

    private void OnTouchpadAxis(SteamVR_Behaviour_Vector2 arg0, SteamVR_Input_Sources arg1, Vector2 arg2, Vector2 arg3)
    {
        Debug.Log("Touchpad axis: " + arg2);
    }

    private void OnCalibrate(SteamVR_Behaviour_Boolean arg0, SteamVR_Input_Sources arg1, bool arg2)
    {
        Debug.Log("Calibrating...");

        // Get the current position and rotation of the VRController (parent)
        Vector3 controllerPosition = _pose.transform.position;
        //Quaternion controllerRotation = _pose.transform.rotation;

        // Compensate by applying the inverse of the controller's position and rotation
        // This will negate the controller's transform
        transform.position = -controllerPosition;
        //transform.rotation = Quaternion.Inverse(controllerRotation);

        Debug.Log("Calibration complete.");
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

    private void FireDetected()
    {
        // Implement the logic for when a fire is detected
        Debug.Log("Fire detected!");
    }

    private void Calibrate()
    {
        
    }

    private void OnDrawGizmos()
    {
        if (!_raycastOrigin) return;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(_raycastOrigin.transform.position, _raycastOrigin.transform.position + _raycastOrigin.transform.forward * 50);
    }
}