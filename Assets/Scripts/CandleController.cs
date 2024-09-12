using UnityEngine;

public class CandleController : MonoBehaviour
{
    [SerializeField] bool enableLEDs;
    [SerializeField] private CrosshairController crosshairController;
    [SerializeField] private SerialController serialController;
    float nextAllowedMessage;

    private void Update()
    {
        if (enableLEDs) UpdateLightIntensity(crosshairController.CurrentEnemyRatio);
    }

    public void UpdateLightIntensity(float intensityRatio)
    {
        if (Time.time > 1 && Time.time > nextAllowedMessage)
        {
            string message;
            if (intensityRatio <.3f)
            {
                message = "0";
            }
            else if (intensityRatio < .3f)
            {
                message = "1";
            }
            else
            {
                message = "2";
            }

            serialController.SendSerialMessage(message);
            nextAllowedMessage = Time.time + .1f;
        }
    }
}
