using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class TrackedImageInfoRuntimeManager : MonoBehaviour
{
    [SerializeField]
    private Text debugLog;

    [SerializeField]
    private Text currentImageText;

    [SerializeField]
    private GameObject prefabOnTrack;

    [SerializeField]
    private Vector3 scaleFactor = new Vector3(0.1f,0.1f,0.1f);

    [SerializeField]
    XRReferenceImageLibrary xrReferenceImageLibrary;
    
    private ARTrackedImageManager trackImageManager;

    void Start()
    {
        debugLog.text += "Creating Runtime Mutable Image Library\n";

        trackImageManager = gameObject.AddComponent<ARTrackedImageManager>();
        trackImageManager.referenceLibrary = trackImageManager.CreateRuntimeLibrary(xrReferenceImageLibrary);
        trackImageManager.maxNumberOfMovingImages = 3;
        trackImageManager.trackedImagePrefab = prefabOnTrack;

        trackImageManager.enabled = true;
        
        trackImageManager.trackedImagesChanged += OnTrackedImagesChanged;
        
        ShowTrackerInfo();
    }

    public void ShowTrackerInfo()
    {
        var runtimeReferenceImageLibrary = trackImageManager.referenceLibrary as MutableRuntimeReferenceImageLibrary;

        debugLog.text += $"TextureFormat.RGBA32 supported: {runtimeReferenceImageLibrary.IsTextureFormatSupported(TextureFormat.RGBA32)}\n";
        debugLog.text += $"Supported Texture Count ({runtimeReferenceImageLibrary.supportedTextureFormatCount})\n";
        debugLog.text += $"trackImageManager.trackables.count ({trackImageManager.trackables.count})\n";
        debugLog.text += $"trackImageManager.trackedImagePrefab.name ({trackImageManager.trackedImagePrefab.name})\n";
        debugLog.text += $"trackImageManager.maxNumberOfMovingImages ({trackImageManager.maxNumberOfMovingImages})\n";
        debugLog.text += $"trackImageManager.supportsMutableLibrary ({trackImageManager.subsystem.SubsystemDescriptor.supportsMutableLibrary})\n";
        debugLog.text += $"trackImageManager.requiresPhysicalImageDimensions ({trackImageManager.subsystem.SubsystemDescriptor.requiresPhysicalImageDimensions})\n";
    }
    void OnDisable()
    {
        trackImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            // Display the name of the tracked image in the canvas
            currentImageText.text = trackedImage.referenceImage.name;
            // Give the initial image a reasonable default scale
            trackedImage.transform.localScale = 
                new Vector3(-trackedImage.referenceImage.size.x, 0.005f, -trackedImage.referenceImage.size.y);
        }

        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            // Display the name of the tracked image in the canvas
            currentImageText.text = trackedImage.referenceImage.name;
            // Give the initial image a reasonable default scale
            trackedImage.transform.localScale = 
                new Vector3(-trackedImage.referenceImage.size.x, 0.005f, -trackedImage.referenceImage.size.y);
        }
    }
}
