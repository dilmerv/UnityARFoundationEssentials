using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class HumanBodyTracker : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The Skeleton prefab to be controlled.")]
    private GameObject skeletonPrefab;

    [SerializeField]
    [Range(-10.0f, 10.0f)]
    private float skeletonOffsetX = 0;

    [Range(-10.0f, 10.0f)]
    [SerializeField]
    private float skeletonOffsetY = 0;

    [Range(-10.0f, 10.0f)]
    [SerializeField]
    private float skeletonOffsetZ = 0;

    [SerializeField]
    [Tooltip("The ARHumanBodyManager which will produce body tracking events.")]
    private ARHumanBodyManager humanBodyManager;

    private Dictionary<TrackableId, HumanBoneController> skeletonTracker = new Dictionary<TrackableId, HumanBoneController>();

    public ARHumanBodyManager HumanBodyManagers
    {
        get { return humanBodyManager; }
        set { humanBodyManager = value; }
    }

    public GameObject SkeletonPrefab
    {
        get { return skeletonPrefab; }
        set { skeletonPrefab = value; }
    }

    void OnEnable()
    {
        Debug.Assert(humanBodyManager != null, "Human body manager is required.");
        humanBodyManager.humanBodiesChanged += OnHumanBodiesChanged;
    }

    void OnDisable()
    {
        if (humanBodyManager != null)
            humanBodyManager.humanBodiesChanged -= OnHumanBodiesChanged;
    }

    void OnHumanBodiesChanged(ARHumanBodiesChangedEventArgs eventArgs)
    {
        HumanBoneController humanBoneController;

        foreach (var humanBody in eventArgs.added)
        {
            if (!skeletonTracker.TryGetValue(humanBody.trackableId, out humanBoneController))
            {
                Debug.Log($"Adding a new skeleton [{humanBody.trackableId}].");
                var newSkeletonGO = Instantiate(skeletonPrefab, humanBody.transform);

                humanBoneController = newSkeletonGO.GetComponent<HumanBoneController>();
                
                // add an offset just when the human body is added
                humanBoneController.transform.position = humanBoneController.transform.position + 
                    new Vector3(skeletonOffsetX, skeletonOffsetY, skeletonOffsetZ);

                skeletonTracker.Add(humanBody.trackableId, humanBoneController);
            }

            humanBoneController.InitializeSkeletonJoints();
            humanBoneController.ApplyBodyPose(humanBody, Vector3.zero);

            HumanBodyTrackerUI.Instance.humanBodyText.text = $"{this.gameObject.name} {humanBody.name} Position: {humanBody.transform.position}\n"+
            $"LocalPosition: {humanBody.transform.localPosition}";

            HumanBodyTrackerUI.Instance.humanBoneControllerText.text = $"{this.gameObject.name} {humanBoneController.name} Position: {humanBoneController.transform.position}\n"+
            $"LocalPosition: {humanBoneController.transform.localPosition}";
        }

        foreach (var humanBody in eventArgs.updated)
        {
            if (skeletonTracker.TryGetValue(humanBody.trackableId, out humanBoneController))
            {
                humanBoneController.ApplyBodyPose(humanBody, Vector3.zero);
            }

            HumanBodyTrackerUI.Instance.humanBodyText.text = $"{this.gameObject.name} {humanBody.name} Position: {humanBody.transform.position}\n"+
            $"LocalPosition: {humanBody.transform.localPosition}";

            HumanBodyTrackerUI.Instance.humanBoneControllerText.text = $"{this.gameObject.name} {humanBoneController.name} Position: {humanBoneController.transform.position}\n"+
            $"LocalPosition: {humanBoneController.transform.localPosition}";
        }

        foreach (var humanBody in eventArgs.removed)
        {
            Debug.Log($"Removing a skeleton [{humanBody.trackableId}].");
            if (skeletonTracker.TryGetValue(humanBody.trackableId, out humanBoneController))
            {
                Destroy(humanBoneController.gameObject);
                skeletonTracker.Remove(humanBody.trackableId);
            }
        }

        HumanBodyTrackerUI.Instance.humanBodyTrackerText.text = $"{this.gameObject.name} Position: {this.gameObject.transform.position}\n"+
            $"LocalPosition: {this.gameObject.transform.localPosition}";
    }
}
