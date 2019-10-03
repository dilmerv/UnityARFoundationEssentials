using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARFace))]
public class EyeTracker : MonoBehaviour
{
    [SerializeField]
    private GameObject leftEyePrefab;

    [SerializeField]
    private GameObject rightEyePrefab;

    private GameObject leftEye;
    private GameObject rightEye;

    private ARFace arFace;

    void Awake()
    {
        arFace = GetComponent<ARFace>();
    }

    void OnEnable()
    {
        ARFaceManager faceManager = FindObjectOfType<ARFaceManager>();
        if(faceManager != null && faceManager.subsystem != null && faceManager.subsystem.SubsystemDescriptor.supportsEyeTracking)
        {
            arFace.updated += OnUpdated;
            Debug.Log("Eye Tracking is supported on this device");
        }
        else 
        {
            Debug.LogError("Eye Tracking is not supported on this device");
        }
    }

    void OnDisable() 
    {
        arFace.updated -= OnUpdated;
        SetVisibility(false);    
    }

    void OnUpdated(ARFaceUpdatedEventArgs eventArgs)
    {
        if(arFace.leftEye != null && leftEye == null)
        {
            leftEye = Instantiate(leftEyePrefab, arFace.leftEye);
            leftEye.SetActive(false);
        }
        if(arFace.rightEye != null && rightEye == null)
        {
            rightEye = Instantiate(rightEyePrefab, arFace.rightEye);
            rightEye.SetActive(false);
        }

        // set visibility
        bool shouldBeVisible = (arFace.trackingState == TrackingState.Tracking) && (ARSession.state > ARSessionState.Ready);
        SetVisibility(shouldBeVisible);
    }

    void SetVisibility(bool isVisible)
    {
        if(leftEye != null && rightEye != null)
        {
            leftEye.SetActive(isVisible);
            rightEye.SetActive(isVisible);
        }
    }
}
