using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
[RequireComponent(typeof(ARAnchorManager))]
[RequireComponent(typeof(ARPointCloudManager))]
public class ReferencePointManagerWithFeaturePoints : MonoBehaviour
{
    [SerializeField]
    private Text debugLog;

    [SerializeField]
    private Text referencePointCount;

    [SerializeField]
    private Button toggleButton;

    [SerializeField]
    private Button clearReferencePointsButton;

    private ARRaycastManager arRaycastManager;

    private ARAnchorManager arReferencePointManager;

    private ARPointCloudManager arPointCloudManager;

    private List<ARAnchor> referencePoints = new List<ARAnchor>();

    private static List<ARRaycastHit> hits = new List<ARRaycastHit>(); 
    
    void Awake() 
    {
        arRaycastManager = GetComponent<ARRaycastManager>();
        arReferencePointManager = GetComponent<ARAnchorManager>();
        arPointCloudManager = GetComponent<ARPointCloudManager>();

        toggleButton.onClick.AddListener(ToggleDetection);
        clearReferencePointsButton.onClick.AddListener(ClearReferencePoints);

        debugLog.gameObject.SetActive(false);
    }

    void Update()
    {
        if(Input.touchCount == 0)
            return;

        Touch touch = Input.GetTouch(0);

        if(touch.phase != TouchPhase.Began)
            return;

        if(arRaycastManager.Raycast(touch.position, hits, TrackableType.FeaturePoint))
        {
            Pose hitPose = hits[0].pose;
            ARAnchor referencePoint = arReferencePointManager.AddAnchor(hitPose);

            if(referencePoint == null)
            {
                debugLog.gameObject.SetActive(true);
                string errorEntry = "There was an error creating a reference point\n";
                Debug.Log(errorEntry);
                debugLog.text += errorEntry; 
            }
            else 
            {
                referencePoints.Add(referencePoint);
                referencePointCount.text = $"Reference Point Count: {referencePoints.Count}";
            }
        }
    }

    private void ToggleDetection()
    {
        arPointCloudManager.enabled = !arPointCloudManager.enabled;

        foreach(ARPointCloud pointCloud in arPointCloudManager.trackables)
        {   
            pointCloud.gameObject.SetActive(arPointCloudManager.enabled);
        }
        
        toggleButton.GetComponentInChildren<Text>().text = arPointCloudManager.enabled ? 
            "Disable Plane Detection" : "Enable Plane Detection";
    }

    private void ClearReferencePoints()
    {
        foreach(ARAnchor referencePoint in referencePoints)
        {
            arReferencePointManager.RemoveAnchor(referencePoint);
        }
        referencePoints.Clear();
        referencePointCount.text = $"Reference Point Count: {referencePoints.Count}";
    }
}
