using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Video;

[RequireComponent(typeof(ARRaycastManager))]
public class PlacementWithManyController : MonoBehaviour
{

    [SerializeField]
    private GameObject[] placedPrefabs;

    [SerializeField]
    private GameObject welcomePanel;

    [SerializeField]
    private int maxNumberOfTVs = 1;

    [SerializeField]
    private Button dismissButton;

    private List<GameObject> addedInstances = new List<GameObject>();

    private Vector2 touchPosition = default;

    public GameObject[] PlacedPrefab
    {
        get 
        {
            return placedPrefabs;
        }
        set 
        {
            placedPrefabs = value;
        }
    }

    [SerializeField]
    private VideoClip[] videoClips;

    private ARRaycastManager arRaycastManager;

    void Awake() 
    {
        arRaycastManager = GetComponent<ARRaycastManager>();
        dismissButton.onClick.AddListener(Dismiss);
    }
    private void Dismiss() => welcomePanel.SetActive(false);

    void Update()
    {
        // on double touch swap clips
        if(Input.touchCount >= 2)
        {
            foreach(GameObject go in placedPrefabs)
            {
                go.GetComponent<VideoPlayer>().clip = videoClips[Random.Range(0, videoClips.Length - 1)];
            }
        }

        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Began)
            {
                touchPosition = touch.position;

                if(arRaycastManager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
                {
                    var hitPose = hits[0].pose;
                    if(addedInstances.Count < maxNumberOfTVs)
                    {
                        GameObject randomPrefab =  placedPrefabs[Random.Range(0, placedPrefabs.Length - 1)];
                        GameObject addedPrefab = Instantiate(randomPrefab, hitPose.position, hitPose.rotation);
                        addedInstances.Add(addedPrefab);
                    }
                }
            }

            if(touch.phase == TouchPhase.Moved)
            {
                touchPosition = touch.position;
                
                if(arRaycastManager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
                {
                    Pose hitPose = hits[0].pose;
                    if(addedInstances.Count > 0)
                    {
                       GameObject lastAdded = addedInstances[addedInstances.Count - 1];
                       lastAdded.transform.position = hitPose.position;
                       lastAdded.transform.rotation = hitPose.rotation;
                    }
                }
            }
        }

    }


    static List<ARRaycastHit> hits = new List<ARRaycastHit>();
}
