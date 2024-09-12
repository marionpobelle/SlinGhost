using UnityEngine;

public class SlingshotInputs : MonoBehaviour
{
    float[] stick;
    Vector3 gyro;
    Vector3 accel;
    int jc_ind = 0;
    Quaternion orientation;
    Joycon joycon;

    [SerializeField] float baseVibrationValue = .2f;
    [SerializeField] float curveDuration = 1f;
    [SerializeField] AnimationCurve aimingVibrationCurve;
    [SerializeField] CrosshairController crosshairController;
    [SerializeField] float pauseBetweenCurves = 1f;
    [SerializeField] float enemyDistanceThreshold = .5f;

    bool isEvaluatingCurve;
    float startCurveTime;
    float nextAllowedCurve;

    void Start()
    {
        gyro = new Vector3(0, 0, 0);
        accel = new Vector3(0, 0, 0);

        // get the public Joycon array attached to the JoyconManager in scene
        if (JoyconManager.Instance.j.Count != 0)
            joycon = JoyconManager.Instance.j[0];
    }

    void Update()
    {
        // make sure the Joycon only gets checked if attached
        if (joycon == null)
            return;

        CacheValues();

        //Depending on the scale of the enemy
        float vibrationStrengthMultiplier = baseVibrationValue + crosshairController.CurrentEnemyRatio;

        //Haptic feedback
        if (crosshairController.IsLockedOn)
        {
            joycon.SetRumble(160, 320, .6f * vibrationStrengthMultiplier);
        }
        else if (crosshairController.DistanceFromEnemy <= enemyDistanceThreshold)
        {
            bool vibrate = false;

            if (!isEvaluatingCurve && Time.time >= nextAllowedCurve)
            {
                nextAllowedCurve = Time.time + curveDuration + pauseBetweenCurves * Mathf.InverseLerp(.5f, enemyDistanceThreshold, crosshairController.DistanceFromEnemy);
                startCurveTime = Time.time;
                isEvaluatingCurve = true;
            }
            else if (isEvaluatingCurve && Time.time > startCurveTime + curveDuration)
            {
                isEvaluatingCurve = false;
            }

            if (isEvaluatingCurve)
            {
                float curveValue = aimingVibrationCurve.Evaluate(
                    Mathf.InverseLerp(
                        startCurveTime,
                        startCurveTime + curveDuration,
                        Time.time));

                if (curveValue > .5f)
                    vibrate = true;
            }

            if (vibrate)
                joycon.SetRumble(160, 320, .6f * vibrationStrengthMultiplier);
            else
                joycon.SetRumble(0, 0, 0);
        }
        else
        {
            joycon.SetRumble(0, 0, 0);
        }
    }

    private void OnDestroy()
    {
        joycon.SetRumble(0, 0, 0);
    }

    private void CacheValues()
    {
        orientation = joycon.GetVector();
        accel = joycon.GetAccel();
        gyro = joycon.GetGyro();
    }
}

