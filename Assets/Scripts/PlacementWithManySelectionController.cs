using UnityEngine;
using UnityEngine.UI;

public class PlacementWithManySelectionController : MonoBehaviour
{
    [SerializeField]
    private GameObject welcomePanel;

    [SerializeField]
    private PlacementObject[] placedObjects;

    [SerializeField]
    private Color activeColor = Color.red;

    [SerializeField]
    private Color inactiveColor = Color.gray;

    [SerializeField]
    private Button dismissButton;

    [SerializeField]
    private Camera arCamera;

    private Vector2 touchPosition = default;

    [SerializeField]
    private bool displayOverlay = false;

    void Awake() 
    {
        dismissButton.onClick.AddListener(Dismiss);
    }

    void Start()
    {
        ChangeSelectedObject(placedObjects[0]);
    }

    private void Dismiss() => welcomePanel.SetActive(false);

    void Update()
    {
        // do not capture events unless the welcome panel is hidden
        if(welcomePanel.activeSelf)
            return;

        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            
            touchPosition = touch.position;

            if(touch.phase == TouchPhase.Began)
            {
                Ray ray = arCamera.ScreenPointToRay(touch.position);
                RaycastHit hitObject;
                if(Physics.Raycast(ray, out hitObject))
                {
                    PlacementObject placementObject = hitObject.transform.GetComponent<PlacementObject>();
                    if(placementObject != null)
                    {
                        ChangeSelectedObject(placementObject);
                    }
                }
            }
        }
    }

    void ChangeSelectedObject(PlacementObject selected)
    {
        foreach (PlacementObject current in placedObjects)
        {   
            MeshRenderer meshRenderer = current.GetComponent<MeshRenderer>();
            if(selected != current) 
            {
                current.Selected = false;
                meshRenderer.material.color = inactiveColor;
            }
            else 
            {
                current.Selected = true;
                meshRenderer.material.color = activeColor;  
            }
            
            if(displayOverlay)
                current.ToggleOverlay();
        }
    }
}
