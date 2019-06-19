using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class LightEstimation : MonoBehaviour
{
    [SerializeField]
    private ARCameraManager arCameraManager;

    [SerializeField]
    private Text brightnessValue;

    [SerializeField]
    private Text tempValue;

    [SerializeField]
    private Text colorCorrectionValue;

    private Light currentLight;

    private void Awake() 
    {
        currentLight = GetComponent<Light>();
    }

    private void OnEnable() 
    {
        arCameraManager.frameReceived += FrameUpdated;
    }

    private void OnDisable() 
    {
        arCameraManager.frameReceived -= FrameUpdated;
    }

    private void FrameUpdated(ARCameraFrameEventArgs args)
    {
        if(args.lightEstimation.averageBrightness.HasValue)
        {
            brightnessValue.text = $"Brightness: {args.lightEstimation.averageBrightness.Value}";
            currentLight.intensity = args.lightEstimation.averageBrightness.Value;
        }

        if(args.lightEstimation.averageColorTemperature.HasValue)
        {
            tempValue.text = $"Color Temperature: {args.lightEstimation.averageColorTemperature.Value}";
            currentLight.colorTemperature = args.lightEstimation.averageColorTemperature.Value;
        }

        if(args.lightEstimation.colorCorrection.HasValue)
        {
            colorCorrectionValue.text = $"Color Correction: {args.lightEstimation.colorCorrection.Value}";
            currentLight.color = args.lightEstimation.colorCorrection.Value;
        }
    }
}
