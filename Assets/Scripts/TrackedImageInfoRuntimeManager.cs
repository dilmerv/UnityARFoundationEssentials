using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.Jobs;
using Unity.Collections;
using System.Collections;

[RequireComponent(typeof(ARTrackedImageManager))]
public class TrackedImageInfoRuntimeManager : MonoBehaviour
{
    [SerializeField]
    private Text debugLog;

    [SerializeField]
    private Text jobState;

    [SerializeField]
    private Vector3 scaleFactor = new Vector3(0.1f,0.1f,0.1f);
    
    [SerializeField]
    private ARTrackedImageManager trackImageManager;

    private MutableRuntimeReferenceImageLibrary runtimeReferenceImageLibrary;

    [SerializeField]
    private Texture2D dynamicTexture;

    [SerializeField]
    private XRReferenceImage xRReferenceImage;

    private NativeSlice<byte> imageBytes;
    

    void Awake()
    {
        debugLog.text += "Creating Runtime Mutable Image Library\n";
        runtimeReferenceImageLibrary = trackImageManager.CreateRuntimeLibrary() as MutableRuntimeReferenceImageLibrary;
        trackImageManager.referenceLibrary = runtimeReferenceImageLibrary;
        debugLog.text += "Created Runtime Mutable Image Library\n";

        jobState.text = "Starting...";

        StartCoroutine(AddImageJob());
    }

    public IEnumerator AddImageJob()
    {
        yield return null;
        
        debugLog.text += "Add Image Job\n";

        var firstGuid = new SerializableGuid(0,0);
        var secondGuid = new SerializableGuid(0,0);

        //XRReferenceImage newImage = new XRReferenceImage(firstGuid, secondGuid, new Vector2(0.1f,0.1f), Guid.NewGuid().ToString(), dynamicTexture);
        
        try
        {
            Debug.Log(xRReferenceImage.ToString());

            NativeArray<byte> imageBytes = new NativeArray<byte>(dynamicTexture.GetRawTextureData(), Allocator.Persistent);
            
            debugLog.text += $"TextureFormat.RGBA32 supported: {runtimeReferenceImageLibrary.IsTextureFormatSupported(TextureFormat.RGBA32)}\n";

            var jobHandle = runtimeReferenceImageLibrary.ScheduleAddImageJob(imageBytes.Slice(), new Vector2Int(dynamicTexture.width, dynamicTexture.height), TextureFormat.RGBA32, xRReferenceImage);

            while(!jobHandle.IsCompleted)
            {
                jobState.text = "Running...";
            }

            jobState.text = "Completed...";
            debugLog.text += $"Job Completed ({runtimeReferenceImageLibrary.count})\n";
            debugLog.text += $"Supported Texture Count ({runtimeReferenceImageLibrary.supportedTextureFormatCount})\n";
            debugLog.text += $"trackImageManager.trackables.count ({trackImageManager.trackables.count})\n";
            debugLog.text += $"trackImageManager.trackedImagePrefab.name ({trackImageManager.trackedImagePrefab.name})\n";
            debugLog.text += $"trackImageManager.maxNumberOfMovingImages ({trackImageManager.maxNumberOfMovingImages})\n";
            debugLog.text += $"trackImageManager.supportsMutableLibrary ({trackImageManager.subsystem.SubsystemDescriptor.supportsMutableLibrary})\n";
            debugLog.text += $"trackImageManager.requiresPhysicalImageDimensions ({trackImageManager.subsystem.SubsystemDescriptor.requiresPhysicalImageDimensions})\n";
        }
        catch(Exception e)
        {
            debugLog.text = e.ToString();
        }
    }

    void OnEnable()
    {
        trackImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        trackImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        debugLog.text = "OnTrackedImagesChanged";

        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            debugLog.text = "Tracked Image Added: " + trackedImage.name;
            UpdateARImage(trackedImage);
        }

        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            debugLog.text = "Tracked Image Updated: " + trackedImage.name;
            UpdateARImage(trackedImage);
        }
    }

    private void UpdateARImage(ARTrackedImage trackedImage)
    {

    }
}
