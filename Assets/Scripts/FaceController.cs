using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARFaceManager))]
public class FaceController : MonoBehaviour
{
    [SerializeField]
    private Button faceTrackingToggle;

    [SerializeField]
    private Button swapFacesToggle;

    private ARFaceManager arFaceManager;
    
    private bool faceTrackingOn = true;

    private int swapCounter = 0;

    [SerializeField]
    public FaceMaterial[] materials;

    void Awake() 
    {
        arFaceManager = GetComponent<ARFaceManager>();
        
        faceTrackingToggle.onClick.AddListener(ToggleTrackingFaces);
        swapFacesToggle.onClick.AddListener(SwapFaces);

        arFaceManager.facePrefab.GetComponent<MeshRenderer>().material = materials[0].Material;
    }

    void SwapFaces() 
    { 
        swapCounter = swapCounter == materials.Length - 1 ? 0 : swapCounter + 1;
        
        foreach(ARFace face in arFaceManager.trackables)
        {
            face.GetComponent<MeshRenderer>().material = materials[swapCounter].Material;
        }

        swapFacesToggle.GetComponentInChildren<Text>().text = $"Face Material ({materials[swapCounter].Name})";
    }

    void ToggleTrackingFaces() 
    { 
        faceTrackingOn = !faceTrackingOn;

        foreach(ARFace face in arFaceManager.trackables)
        {
            face.enabled = faceTrackingOn;
        }
        
        faceTrackingToggle.GetComponentInChildren<Text>().text = $"Face Tracking {(arFaceManager.enabled ? "Off" : "On" )}";
    }

}

[System.Serializable]
public class FaceMaterial
{
    public Material Material;

    public string Name;
}