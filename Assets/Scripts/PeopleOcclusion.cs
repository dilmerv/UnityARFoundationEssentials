
using UnityEngine;
using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARCameraManager))]
public class PeopleOcclusion : MonoBehaviour
{
    [SerializeField] 
    private ARSessionOrigin m_arOrigin = null;
    
    [SerializeField] 
    private ARHumanBodyManager m_humanBodyManager = null;
    
    [SerializeField] 
    private ARCameraManager m_cameraManager = null;
    
    [SerializeField] 
    private Shader m_peopleOcclusionShader = null;
    
    [SerializeField] 
    private Texture2D humanOverlayTexture;
    
    private Texture2D m_cameraFeedTexture = null;
    
    private Material m_material = null;

	void Awake()
	{
        m_material = new Material(m_peopleOcclusionShader);
        GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;
    }

    private void OnEnable()
    {
        m_cameraManager.frameReceived += OnCameraFrameReceived;
    }

    private void OnDisable()
    {
        m_cameraManager.frameReceived -= OnCameraFrameReceived;
    }
    void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
        if(PeopleOcclusionSupported())
        {
            if(m_cameraFeedTexture != null)
            {
                m_material.SetFloat("_UVMultiplierLandScape", CalculateUVMultiplierLandScape(m_cameraFeedTexture));
                m_material.SetFloat("_UVMultiplierPortrait", CalculateUVMultiplierPortrait(m_cameraFeedTexture));
            }
            
            if(Input.deviceOrientation == DeviceOrientation.LandscapeRight) 
            {
                m_material.SetFloat("_UVFlip", 0);
                m_material.SetInt("_ONWIDE", 1);
            }
            else if(Input.deviceOrientation == DeviceOrientation.LandscapeLeft) 
            {
                m_material.SetFloat("_UVFlip", 1);
                m_material.SetInt("_ONWIDE", 1);
            }
            else
            {
                m_material.SetInt("_ONWIDE", 0);
            }

            m_material.SetTexture("_OcclusionDepth", m_humanBodyManager.humanDepthTexture);
            m_material.SetTexture("_OcclusionStencil", m_humanBodyManager.humanStencilTexture);
            Graphics.Blit(source, destination, m_material);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }

    private void OnCameraFrameReceived(ARCameraFrameEventArgs eventArgs)
    {
        if(PeopleOcclusionSupported())
        {
            RefreshCameraFeedTexture();
        }
    }

    private bool PeopleOcclusionSupported()
    {
        return m_humanBodyManager.subsystem != null && m_humanBodyManager.humanDepthTexture != null && m_humanBodyManager.humanStencilTexture != null;
    }

    private void RefreshCameraFeedTexture()
    {
        XRCameraImage cameraImage;
        m_cameraManager.TryGetLatestImage(out cameraImage);

        if (m_cameraFeedTexture == null || m_cameraFeedTexture.width != cameraImage.width || m_cameraFeedTexture.height != cameraImage.height)
        {
            m_cameraFeedTexture = new Texture2D(cameraImage.width, cameraImage.height, TextureFormat.RGBA32, false);
        }

        CameraImageTransformation imageTransformation = Input.deviceOrientation == DeviceOrientation.LandscapeRight ? CameraImageTransformation.MirrorY : CameraImageTransformation.MirrorX;
        XRCameraImageConversionParams conversionParams = new XRCameraImageConversionParams(cameraImage, TextureFormat.RGBA32, imageTransformation);

        NativeArray<byte> rawTextureData = m_cameraFeedTexture.GetRawTextureData<byte>();

        try
        {
            unsafe
            {
                cameraImage.Convert(conversionParams, new IntPtr(rawTextureData.GetUnsafePtr()), rawTextureData.Length);
            }
        }
        finally
        {
            cameraImage.Dispose();
        }

        m_cameraFeedTexture.Apply();
        m_material.SetTexture("_CameraFeed", humanOverlayTexture);
    }

    private float CalculateUVMultiplierLandScape(Texture2D cameraTexture)
    {
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float cameraTextureAspect = (float)cameraTexture.width / (float)cameraTexture.height;
        return screenAspect / cameraTextureAspect;

    }
    private float CalculateUVMultiplierPortrait(Texture2D cameraTexture)
    {
        float screenAspect = (float)Screen.height / (float)Screen.width;
        float cameraTextureAspect = (float)cameraTexture.width / (float)cameraTexture.height;
        return screenAspect / cameraTextureAspect;

    }
}