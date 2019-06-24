using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARRaycastManager))]
public class PlacementWithManyObjectsController : MonoBehaviour
{

    [SerializeField]
    private GameObject placedPrefab;

    [SerializeField]
    private GameObject welcomePanel;

    [SerializeField]
    private Button dismissButton;

    private Vector2 touchPosition = default;

    private ARRaycastManager arRaycastManager;

    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Awake() 
    {
        arRaycastManager = GetComponent<ARRaycastManager>();
        dismissButton.onClick.AddListener(Dismiss);
    }
    private void Dismiss() => welcomePanel.SetActive(false);

    void Update()
    {
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Began)
            {
                touchPosition = touch.position;

                if(arRaycastManager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
                {
                    var hitPose = hits[0].pose;
                    Instantiate(placedPrefab, hitPose.position, hitPose.rotation);
                }
            }
        }

    }
}
