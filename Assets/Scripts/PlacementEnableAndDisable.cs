using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARRaycastManager))]
public class PlacementEnableAndDisable : MonoBehaviour
{
    [SerializeField]
    private GameObject placedPrefab;

    [SerializeField]
    private GameObject welcomePanel;

    [SerializeField]
    private Button dismissButton;

    [SerializeField]
    private Camera arCamera;

    private PlacementObject[] placedObjects;

    private Vector2 touchPosition = default;

    private ARRaycastManager arRaycastManager;

    private bool onTouchHold = false;

    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private PlacementObject lastSelectedObject;

    [SerializeField]
    private Button redButton, greenButton, blueButton;

    [SerializeField]
    private Button toggleButton;


    private ARPlaneManager aRPlaneManager;

    private GameObject PlacedPrefab 
    {
        get 
        {
            return placedPrefab;
        }
        set 
        {
            placedPrefab = value;
        }
    }


    void Awake() 
    {
        arRaycastManager = GetComponent<ARRaycastManager>();
        dismissButton.onClick.AddListener(Dismiss);

        aRPlaneManager = GetComponent<ARPlaneManager>();

        if(redButton != null && greenButton != null && blueButton != null) 
        {
            redButton.onClick.AddListener(() => ChangePrefabSelection("ARRed"));
            greenButton.onClick.AddListener(() => ChangePrefabSelection("ARGreen"));
            blueButton.onClick.AddListener(() => ChangePrefabSelection("ARBlue"));
        }

        if(toggleButton != null)
        {
            toggleButton.onClick.AddListener(TogglePlaneDetection);
        }
    }

    private void TogglePlaneDetection()
    {
        aRPlaneManager.enabled = !aRPlaneManager.enabled;

        foreach(ARPlane plane in aRPlaneManager.trackables)
        {   
            plane.gameObject.SetActive(aRPlaneManager.enabled);
        }
        
        toggleButton.GetComponentInChildren<Text>().text = aRPlaneManager.enabled ? 
            "Disable Plane Detection" : "Enable Plane Detection";
    }

    
    private void ChangePrefabSelection(string name)
    {
        GameObject loadedGameObject = Resources.Load<GameObject>($"Prefabs/{name}");
        if(loadedGameObject != null)
        {
            PlacedPrefab = loadedGameObject;
            Debug.Log($"Game object with name {name} was loaded");
        }
        else 
        {
            Debug.Log($"Unable to find a game object with name {name}");
        }
    }

    private void Dismiss() => welcomePanel.SetActive(false);

    void Update()
    {
        // do not capture events unless the welcome panel is hidden
        if(welcomePanel.activeSelf)
            return;

        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            
            touchPosition = touch.position;

            if(touch.phase == TouchPhase.Began)
            {
                Ray ray = arCamera.ScreenPointToRay(touch.position);
                RaycastHit hitObject;
                if(Physics.Raycast(ray, out hitObject))
                {
                    lastSelectedObject = hitObject.transform.GetComponent<PlacementObject>();
                    if(lastSelectedObject != null)
                    {
                        PlacementObject[] allOtherObjects = FindObjectsOfType<PlacementObject>();
                        foreach(PlacementObject placementObject in allOtherObjects)
                        {
                            placementObject.Selected = placementObject == lastSelectedObject;
                        }
                    }
                }
            }

            if(touch.phase == TouchPhase.Ended)
            {
                lastSelectedObject.Selected = false;
            }
        }

        if(arRaycastManager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;

            if(lastSelectedObject == null)
            {
                lastSelectedObject = Instantiate(placedPrefab, hitPose.position, hitPose.rotation).GetComponent<PlacementObject>();
            }
            else 
            {
                if(lastSelectedObject.Selected)
                {
                    lastSelectedObject.transform.position = hitPose.position;
                    lastSelectedObject.transform.rotation = hitPose.rotation;
                }
            }
        }
    }
}
