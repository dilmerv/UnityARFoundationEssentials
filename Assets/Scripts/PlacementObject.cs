using TMPro;
using UnityEngine;

public class PlacementObject : MonoBehaviour
{
    [SerializeField]
    private bool IsSelected;

    public bool Selected 
    { 
        get 
        {
            return this.IsSelected;
        }
        set 
        {
            IsSelected = value;
        }
    }

    [SerializeField]
    private TextMeshPro OverlayText;

    [SerializeField]
    private string OverlayDisplayText;

    void Awake ()
    {
        OverlayText = GetComponentInChildren<TextMeshPro>();
        if(OverlayText != null)
        {
            OverlayText.gameObject.SetActive(false);
        }
    }

    public void ToggleOverlay()
    {
        OverlayText.gameObject.SetActive(IsSelected);
        OverlayText.text = OverlayDisplayText;
    }
}
