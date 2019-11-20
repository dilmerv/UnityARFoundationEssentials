using TMPro;
using UnityEngine;

public class PlacementObject : MonoBehaviour
{
    [SerializeField]
    private bool IsSelected;

    [SerializeField]
    private bool IsLocked;

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

    public bool Locked 
    { 
        get 
        {
            return this.IsLocked;
        }
        set 
        {
            IsLocked = value;
        }
    }

    [SerializeField]
    private TextMeshPro OverlayText;

    [SerializeField]
    private Canvas canvasComponent;

    [SerializeField]
    private string OverlayDisplayText;

    public void SetOverlayText(string text)
    {
        if(OverlayText != null)
        {
            OverlayText.gameObject.SetActive(true);
            OverlayText.text = text;
        }
    }

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

    public void ToggleCanvas()
    {
        canvasComponent?.gameObject.SetActive(IsSelected);
    }
}
