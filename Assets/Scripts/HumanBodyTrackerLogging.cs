using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class HumanBodyTrackerLogging : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The Skeleton prefab to be controlled.")]
    private GameObject skeletonPrefab;

    [SerializeField]
    private Text loggingText;

    [SerializeField]
    [Tooltip("The ARHumanBodyManager which will produce body tracking events.")]
    private ARHumanBodyManager humanBodyManager;

    private Dictionary<TrackableId, HumanBoneController> skeletonTracker = new Dictionary<TrackableId, HumanBoneController>();

    [SerializeField]
    private GameObject welcomePanel;

    [SerializeField]
    private Button dismissButton;

    private BoneTracker[] boneTrackers;

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

    void Awake()
    {
        dismissButton.onClick.AddListener(DissmissWelcomePanel);
    }

    void DissmissWelcomePanel()
    {
        welcomePanel.gameObject.SetActive(false);
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

        loggingText.text = string.Empty;

        foreach (var humanBody in eventArgs.added)
        {
            if (!skeletonTracker.TryGetValue(humanBody.trackableId, out humanBoneController))
            {
                Debug.Log($"Adding a new skeleton [{humanBody.trackableId}].");
                var newSkeletonGO = Instantiate(skeletonPrefab, humanBody.transform);
                humanBoneController = newSkeletonGO.GetComponent<HumanBoneController>();
                skeletonTracker.Add(humanBody.trackableId, humanBoneController);
            }

            humanBoneController.InitializeSkeletonJoints();
            humanBoneController.ApplyBodyPose(humanBody, Vector3.zero);

            if(boneTrackers == null)
            {
                boneTrackers = humanBoneController.skeletonRoot.GetComponentsInChildren<BoneTracker>();
                foreach(BoneTracker boneTracker in boneTrackers)
                {
                    loggingText.text += $"Bone: {boneTracker.gameObject.transform.parent.name} Position: {boneTracker.gameObject.transform.position}";
                    loggingText.text += $"Bone: {boneTracker.gameObject.transform.parent.name} LocalPosition: {boneTracker.gameObject.transform.localPosition}";
                }
            }

        }

        foreach (var humanBody in eventArgs.updated)
        {
            if (skeletonTracker.TryGetValue(humanBody.trackableId, out humanBoneController))
            {
                humanBoneController.ApplyBodyPose(humanBody, Vector3.zero);
            }

            if(boneTrackers != null)
            {
                foreach(BoneTracker boneTracker in boneTrackers)
                {
                    loggingText.text += $"Bone: {boneTracker.gameObject.transform.parent.name} Position: {boneTracker.gameObject.transform.position}";
                    loggingText.text += $"Bone: {boneTracker.gameObject.transform.parent.name} LocalPosition: {boneTracker.gameObject.transform.localPosition}";
                }
            }
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
    }
}
