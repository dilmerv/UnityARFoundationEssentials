using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARReferencePointManager))]
public class ReferencePointManagerWithCameraDistance : MonoBehaviour
{
    [SerializeField]
    private Text debugLog;

    [SerializeField]
    private Text referencePointCount;

    [SerializeField]
    private Button clearReferencePointsButton;

    [SerializeField]
    private Camera arCamera;

    private ARReferencePointManager arReferencePointManager;

    private List<ARReferencePoint> referencePoints = new List<ARReferencePoint>();
    
    void Awake() 
    {
        arReferencePointManager = GetComponent<ARReferencePointManager>();
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

        Vector3 newPos = arCamera.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, 0.3f));
        
        arReferencePointManager.AddReferencePoint(new Pose(newPos, Quaternion.identity));
    }

    private void ClearReferencePoints()
    {
        foreach(ARReferencePoint referencePoint in referencePoints)
        {
            arReferencePointManager.RemoveReferencePoint(referencePoint);
        }
        referencePoints.Clear();
        referencePointCount.text = $"Reference Point Count: {referencePoints.Count}";
    }
}
