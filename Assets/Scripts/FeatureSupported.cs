using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class FeatureSupported : MonoBehaviour
{
    [SerializeField]
    private Text features;

    [SerializeField]
    private ARFaceManager arFaceManager;

    [SerializeField]
    private ARHumanBodyManager arHumanBodyManager;

    [SerializeField]
    private ARPointCloudManager arPointCloudManager;

    void Start()
    {
        // Face Support Checks
        bool supportsEyeTracking = arFaceManager.subsystem.SubsystemDescriptor.supportsEyeTracking;
        bool supportsFacePose = arFaceManager.subsystem.SubsystemDescriptor.supportsFacePose;
        bool supportsFaceMeshVerticesAndIndices = arFaceManager.subsystem.SubsystemDescriptor.supportsFaceMeshVerticesAndIndices;

        // Human Body Support Checks
        bool supportsHumanBody2D = arHumanBodyManager.subsystem.SubsystemDescriptor.supportsHumanBody2D;
        bool supportsHumanBody3D = arHumanBodyManager.subsystem.SubsystemDescriptor.supportsHumanBody3D;
        bool supportsHumanDepthImage = arHumanBodyManager.subsystem.SubsystemDescriptor.supportsHumanDepthImage;

        // Point Cloud Support Checks
        bool supportsConfidence = arPointCloudManager.subsystem.SubsystemDescriptor.supportsConfidence;
        bool supportsFeaturePoints = arPointCloudManager.subsystem.SubsystemDescriptor.supportsFeaturePoints;
        
        features.text = $"supportsEyeTracking : {supportsEyeTracking}\n" +
            $"supportsFacePose : {supportsFacePose}\n" +
            $"supportsFaceMeshVerticesAndIndices : {supportsFaceMeshVerticesAndIndices}\n" +
            $"supportsHumanBody2D : {supportsHumanBody2D}\n" +
            $"supportsHumanBody3D : {supportsHumanBody3D}\n" +
            $"supportsHumanDepthImage : {supportsHumanDepthImage}\n" +
            $"supportsConfidence : {supportsConfidence}\n" +
            $"supportsFeaturePoints : {supportsFeaturePoints}";
    }
}
