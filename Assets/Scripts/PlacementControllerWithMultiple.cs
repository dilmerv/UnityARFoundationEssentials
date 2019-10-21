using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARRaycastManager))]
public class PlacementControllerWithMultiple : MonoBehaviour
{
    [SerializeField]
    private Button arGreenButton;

    [SerializeField]
    private Button arRedButton;

    [SerializeField]
    private Button arBlueButton;

    [SerializeField]
    private Button dismissButton;

    [SerializeField]
    private GameObject welcomePanel;

    [SerializeField]
    private Text selectionText;

    private GameObject placedPrefab;

    private ARRaycastManager arRaycastManager;

    void Awake() 
    {
        arRaycastManager = GetComponent<ARRaycastManager>();

        // set initial prefab
        ChangePrefabTo("ARBlue");

        arGreenButton.onClick.AddListener(() => ChangePrefabTo("ARGreen"));
        arBlueButton.onClick.AddListener(() => ChangePrefabTo("ARBlue"));
        arRedButton.onClick.AddListener(() => ChangePrefabTo("ARRed"));
        dismissButton.onClick.AddListener(Dismiss);
    }

    private void Dismiss() => welcomePanel.SetActive(false);

    void ChangePrefabTo(string prefabName)
    {
        placedPrefab = Resources.Load<GameObject>($"Prefabs/{prefabName}");

        if(placedPrefab == null)
        {
            Debug.LogError($"Prefab with name {prefabName} could not be loaded, make sure you check the naming of your prefabs...");
        }
        
        switch(prefabName)
        {
            case "ARBlue":
                selectionText.text = $"Selected: <color='blue'>{prefabName}</color>";
            break;
            case "ARRed":
                selectionText.text = $"Selected: <color='red'>{prefabName}</color>";
            break;
            case "ARGreen":
                selectionText.text = $"Selected: <color='green'>{prefabName}</color>";
            break;
        }
    }

    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if(Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }

        touchPosition = default;

        return false;
    }

    void Update()
    {
        if(placedPrefab == null || welcomePanel.gameObject.activeSelf)
        {
            return;
        }

        if(!TryGetTouchPosition(out Vector2 touchPosition))
            return;

        if(arRaycastManager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
        {
            var hitPose = hits[0].pose;
            Instantiate(placedPrefab, hitPose.position, hitPose.rotation);
        }
    }


    static List<ARRaycastHit> hits = new List<ARRaycastHit>();
}
