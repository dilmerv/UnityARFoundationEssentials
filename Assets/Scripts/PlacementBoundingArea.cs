using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlacementObject))]
public class PlacementBoundingArea : MonoBehaviour
{
    private PlacementObject placementObject;

    private bool initialized = false;

    private GameObject boundingArea;

    [SerializeField]
    private float boundingRadius = 1.0f;

    [SerializeField]
    private Vector3 boundingAreaPosition = Vector3.zero;

    [SerializeField]
    private Material boundingAreaMaterial;

    void Awake()
    {
        SetupBounds();
    }

    void SetupBounds()
    {   
        placementObject = GetComponent<PlacementObject>();
        initialized = true;
    }

    void Update()
    {
        if(initialized)
        {
            DrawBoundingArea(placementObject.Selected);
        }
    }

    void DrawBoundingArea(bool isActive)
    {
        if(boundingArea == null)
        {
            boundingArea = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            boundingArea.name = "BoundingArea";
            boundingArea.transform.parent = placementObject.transform.parent;
            boundingArea.GetComponent<MeshRenderer>().material = boundingAreaMaterial;
        }

        boundingArea.transform.localScale = new Vector3(boundingRadius * 1.5f, boundingRadius * 1.5f, boundingRadius * 1.5f);
        boundingArea.transform.localPosition = boundingAreaPosition;
        
        boundingArea.SetActive(isActive);
    }
}
