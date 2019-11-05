using System.Linq;
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

    [SerializeField]
    private GameObject debugPanel;

    [SerializeField]
    private Button toggleDebugButton;

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
        toggleDebugButton.onClick.AddListener(ToggleDebugPanel);
    }

    void DissmissWelcomePanel()
    {
        welcomePanel.gameObject.SetActive(false);
    }

    void ToggleDebugPanel()
    {
        if(debugPanel.activeSelf)
        {
            debugPanel.SetActive(false);
            toggleDebugButton.GetComponentInChildren<Text>().text = "DEBUG ON";
        }
        else 
        {
            debugPanel.SetActive(true);
            toggleDebugButton.GetComponentInChildren<Text>().text = "DEBUG OFF";
        }
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
                    loggingText.text += $"Bone: {boneTracker.transform.parent.name} Position: {boneTracker.transform.position}\n";
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
                    loggingText.text += $"Bone: {boneTracker.transform.parent.name} Position: {boneTracker.transform.position}\n";
                }
            }
            
            // detecting if we are beyond the limits before we can spawn a particle (super powers)
            ApplyPowers(boneTrackers);
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

    private void ApplyPowers(BoneTracker[] bones)
    {
        BoneTracker leftHandBone = bones.Where(b => b.transform.parent.name == "LeftHand").SingleOrDefault();

        BoneTracker rightHandBone = bones.Where(b => b.transform.parent.name == "RightHand").SingleOrDefault();

        BoneTracker spine = bones.Where(b => b.transform.parent.name == "Spine4").SingleOrDefault();

        if(leftHandBone == null || rightHandBone == null || spine == null)
        {
            string error = "Bones tracked were not found...";
            // spawn particle on the left hand
            loggingText.text += $"{error}";
            Debug.LogError(error);
            return;
        }

        SpawnSuperPowers leftHandPowers = leftHandBone.GetComponent<SpawnSuperPowers>();
        SpawnSuperPowers rightHandPowers = rightHandBone.GetComponent<SpawnSuperPowers>();

        if(leftHandBone.transform.position.y > spine.transform.position.y)
        {
            // spawn particle on the left hand
            leftHandPowers.Started = true;
            loggingText.text += $"LeftHandBone is above Spine\n";
        }
        else 
        {
            leftHandPowers.Started = false;
        }

        if(rightHandBone.transform.position.y > spine.transform.position.y)
        {
            // spawn particle on the right hand
            rightHandPowers.Started = true;
            loggingText.text += $"RightHandBone is above Spine\n";
        }
        else 
        {
            rightHandPowers.Started = false;
        }
    }
}
