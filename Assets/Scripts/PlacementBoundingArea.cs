using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlacementObject))]
public class PlacementBoundingArea : MonoBehaviour
{
    private PlacementObject placementObject;

    private Bounds placementBounds;

    private bool initialized = false;

    private GameObject boundingArea;

    [SerializeField]
    private float boundingRadius = 1.0f;

    [SerializeField]
    private Vector3 boundingBoxPosition = Vector3.zero;

    void Awake()
    {
        SetupBounds();
    }

    void SetupBounds()
    {   
        placementObject = GetComponent<PlacementObject>();

        if(placementObject == null)
        {
            Debug.LogError("Placement object is required");
            return;
        }
        initialized = true;
    }

    void Update()
    {
        if(initialized)
        {
            DrawBoundingBox(placementObject.Selected);
        }
    }

    void OnDrawGizmosSelected()
    {
        SetupBounds();
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere (boundingBoxPosition, boundingRadius);
    }

    void DrawBoundingBox(bool isActive)
    {
        if(boundingArea == null)
        {
            boundingArea = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            boundingArea.name = "BoundingArea";
            boundingArea.transform.parent = placementObject.transform.parent;
        }

        boundingArea.transform.localScale = new Vector3(boundingRadius * 1.5f, boundingRadius * 1.5f, boundingRadius * 1.5f);
        boundingArea.transform.localPosition = boundingBoxPosition;
    }
}
