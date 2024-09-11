using System.Collections.Generic;
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

    [SerializeField] Queue<Vector3> latestAccelValues;
    [SerializeField] int accelQueueSize = 25;
    [SerializeField] float fireThreshold = 2;
    [SerializeField] float sensitivity = 20;

    bool wasShotFired = false;

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
    }

    // Update is called once per frame
    void Update()
    {
        // make sure the Joycon only gets checked if attached
        if (joycon == null)
            return;

        CacheValues();

        //Haptic feedback
        joycon.SetRumble(
            crosshairController.IsLockedOn ? 160 : 0,
            crosshairController.IsLockedOn ? 320 : 0,
            crosshairController.IsLockedOn ? .6f : 0);

        float averageAccel = 0;
        foreach (Vector3 accelValue in latestAccelValues)
        {
            averageAccel += accelValue.z;
        }
        averageAccel /= accelQueueSize;

        if (!wasShotFired && averageAccel > fireThreshold || joycon.GetButtonDown(Joycon.Button.DPAD_UP))
        {
            Debug.Log("SHOT FIRED");
            crosshairController.Fire();
            wasShotFired = true;
        }

        if (averageAccel < 0)
        {
            wasShotFired = false;
        }

    }

    private void CacheValues()
    {
        orientation = joycon.GetVector();
        accel = joycon.GetAccel();
        gyro = joycon.GetGyro();

        latestAccelValues.Dequeue();
        latestAccelValues.Enqueue(accel);
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

