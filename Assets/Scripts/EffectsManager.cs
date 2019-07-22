using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class EffectsManager : MonoBehaviour
{
    [SerializeField]
    private Light defaultLight;

    [SerializeField]
    private Button toggleLightButton;

    [SerializeField]
    private Button toggleShadowsButton;

    [SerializeField]
    private Button togglePlaneDetectionButton;

    [SerializeField]
    private ARPlaneManager aRPlaneManager;
    
    // Start is called before the first frame update
    void Start()
    {
        if(toggleLightButton == null || toggleShadowsButton == null || togglePlaneDetectionButton == null)
        {
            Debug.LogError("You must set buttons in the inspector");
            enabled = false;
            return;
        }

        if(defaultLight == null)
        {
            Debug.LogError("You must set the light in the inspector");
            enabled = false;
            return;
        }

        toggleLightButton.onClick.AddListener(ToggleLights);
        toggleShadowsButton.onClick.AddListener(ToggleShadows);
        togglePlaneDetectionButton.onClick.AddListener(TogglePlaneDetection);
    }

    void TogglePlaneDetection()
    {
        aRPlaneManager.enabled = !aRPlaneManager.enabled;
        
        foreach(ARPlane plane in aRPlaneManager.trackables)
        {   
            plane.gameObject.SetActive(aRPlaneManager.enabled);
        }
        togglePlaneDetectionButton.GetComponentInChildren<Text>().text = aRPlaneManager.enabled ? "Disable Detection" : "Enable Detection";
    }

    void ToggleLights()
    {
        defaultLight.enabled = !defaultLight.enabled;
        toggleLightButton.GetComponentInChildren<Text>().text = defaultLight.enabled ? "Disable Lights" : "Enable Lights";
    }

    void ToggleShadows()
    {
        if(defaultLight.enabled)
        {
            float shadowValue = defaultLight.shadowStrength > 0 ? 0 : 1;
            defaultLight.shadowStrength = shadowValue;
            toggleShadowsButton.GetComponentInChildren<Text>().text = shadowValue == 0 ? "Enable Shadows" : "Disable Shadows";
        }
    }
}
