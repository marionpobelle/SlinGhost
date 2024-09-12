using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CandleController : MonoBehaviour
{
    [SerializeField] private CrosshairController crosshairController;
    [SerializeField] private SerialController serialController;
    float nextAllowedMessage;

    private void Update()
    {
        UpdateLightIntensity(crosshairController.CurrentEnemyRatio);
    }

    public void UpdateLightIntensity(float intensityRatio)
    {
        if (Time.time > 1 && Time.time > nextAllowedMessage)
        {
            int lightStrength = Mathf.Clamp(
                Mathf.RoundToInt(
                    Mathf.Lerp(
                        0, 
                        255, 
                        intensityRatio)),
                    0,
                    255);

            serialController.SendSerialMessage(lightStrength.ToString());
            nextAllowedMessage = Time.time + .1f;
        }
    }
}
