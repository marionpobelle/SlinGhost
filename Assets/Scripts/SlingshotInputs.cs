using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SlingshotInputs : MonoBehaviour
{
    private Joycon joycon;

    // Values made available via Unity
    public float[] stick;
    public Vector3 gyro;
    public Vector3 accel;
    public int jc_ind = 0;
    public Quaternion orientation;

    [SerializeField] CrosshairController crosshairController;
    [SerializeField] Vector3 rotationOffset = Vector3.zero;

    [SerializeField] Queue<Vector3> latestAccelValues;
    [SerializeField] Queue<Quaternion> latestOrientValues;
    [SerializeField] int accelQueueSize = 25;
    [SerializeField] int orientQueueSize = 25;
    [SerializeField] float currentMoveSpeed;
    [SerializeField] float fireThreshold = 2;
    [SerializeField] float sensitivity = 20;

    bool wasShotFired = false;
    Vector3 lastShotDir;

    void Start()
    {
        gyro = new Vector3(0, 0, 0);
        accel = new Vector3(0, 0, 0);
        // get the public Joycon array attached to the JoyconManager in scene
        if (JoyconManager.Instance.j.Count != 0)
            joycon = JoyconManager.Instance.j[0];

        latestAccelValues = new Queue<Vector3>();

        for (int i = 0; i < accelQueueSize; i++)
        {
            latestAccelValues.Enqueue(Vector3.zero);
        }

        latestOrientValues = new Queue<Quaternion>();

        for (int i = 0; i < orientQueueSize; i++)
        {
            latestOrientValues.Enqueue(Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // make sure the Joycon only gets checked if attached
        if (joycon == null)
            return;

        CacheValues();
        SetCrosshairPosition();
        //Recenter
        if (joycon.GetButtonDown(Joycon.Button.DPAD_RIGHT))
        {
            joycon.Recenter();
            Debug.Log("Recentering joycon");
        }

        float averageAccel = 0;
        foreach (Vector3 accelValue in latestAccelValues)
        {
            averageAccel += accelValue.z;
        }
        averageAccel /= accelQueueSize;

        //Debug value   
        currentMoveSpeed = averageAccel;

        if (!wasShotFired && averageAccel > fireThreshold || joycon.GetButtonDown(Joycon.Button.DPAD_UP))
        {
            Debug.Log("SHOT FIRED");
            lastShotDir = Average(latestOrientValues.ToList()) * Vector3.forward;
            wasShotFired = true;
        }

        if (averageAccel < 0)
        {
            wasShotFired = false;
        }
        transform.rotation = orientation;
    }

    public Vector3 defaultRotation = new Vector3(-90, 0, -90);

    public float totalTests = 100;

    float ProjectQuaternion(Quaternion a, Quaternion b, float index, Quaternion joyconQuat)
    {
        Quaternion slerpedQuaternion = Quaternion.Slerp(a, b, index);

        return Mathf.Pow(joyconQuat.w - slerpedQuaternion.w, 2) +
        Mathf.Pow(joyconQuat.x - slerpedQuaternion.x, 2) +
        Mathf.Pow(joyconQuat.y - slerpedQuaternion.y, 2) +
        Mathf.Pow(joyconQuat.z - slerpedQuaternion.z, 2);

    }

    void SetCrosshairPosition()
    {
        Vector3 targetPos = Vector3.zero;

        Quaternion left = Quaternion.Euler(defaultRotation + Vector3.left * sensitivity);
        Quaternion right = Quaternion.Euler(defaultRotation + Vector3.right * sensitivity);

        int bestIndex = -1;
        float lowestValue = float.MaxValue;
        for (int i = 0; i < totalTests; i++)
        {
            float quaternionValue = ProjectQuaternion(left, right, i / totalTests, orientation);
            if (lowestValue >= quaternionValue)
            {
                bestIndex = i;
                lowestValue = quaternionValue;
            }
        }
        // Debug.Log(bestIndex + " : " + lowestValue);


        targetPos.x = Mathf.Lerp(-1, 1, bestIndex / totalTests);

        crosshairController.transform.position = targetPos;
    }

    private void CacheValues()
    {
        orientation = joycon.GetVector();
        accel = joycon.GetAccel();
        gyro = joycon.GetGyro();

        latestAccelValues.Dequeue();
        latestAccelValues.Enqueue(accel);
        latestOrientValues.Dequeue();
        latestOrientValues.Enqueue(orientation);
    }

    public static Quaternion Average(List<Quaternion> quaternions)
    {
        if (quaternions == null || quaternions.Count < 1)
            return Quaternion.identity;

        if (quaternions.Count < 2)
            return quaternions[0];

        int count = quaternions.Count;
        float weight = 1.0f / (float)count;
        Quaternion avg = Quaternion.identity;

        for (int i = 0; i < count; i++)
            avg *= Quaternion.Slerp(Quaternion.identity, quaternions[i], weight);

        return avg;
    }
}

