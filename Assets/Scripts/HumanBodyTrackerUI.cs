using System.Collections;
using System.Collections.Generic;
using DilmerGames.Core.Singletons;
using UnityEngine;
using UnityEngine.UI;

public class HumanBodyTrackerUI : Singleton<HumanBodyTrackerUI>
{
    [SerializeField]
    [Tooltip("The Human Body Tracker text used for debugging purposes.")]
    public Text humanBodyTrackerText;

    [SerializeField]
    [Tooltip("The Human Body text used for debugging purposes.")]
    public Text humanBodyText;

    [SerializeField]
    [Tooltip("The Human bone controller used for debugging purposes.")]
    public Text humanBoneControllerText;

    [SerializeField]
    private GameObject welcomePanel;

    [SerializeField]
    private Button dismissButton;

    [SerializeField]
    private Button toggleOptionsButton;

    [SerializeField]
    private GameObject options;

    [SerializeField]
    private Button toggleDebug;

    void OnEnable() 
    {
        if(humanBodyTrackerText == null || humanBodyTrackerText == null || humanBoneControllerText == null)
        {
            Debug.LogError($"{gameObject.name} is missing UI connections in inspector...");
        }
    }
    void Awake() 
    {
        dismissButton.onClick.AddListener(Dismiss);
        toggleOptionsButton.onClick.AddListener(ToggleOptions);
        toggleDebug.onClick.AddListener(ToggleDebugging);
    }

    private void Dismiss() => welcomePanel.SetActive(false);

    private void ToggleDebugging()
    {
        ToggleText(humanBodyText);
        ToggleText(humanBodyTrackerText);
        ToggleText(humanBoneControllerText);

        if(humanBodyText.gameObject.activeSelf)
            toggleDebug.GetComponentInChildren<Text>().text = "Debug Off";
        else 
            toggleDebug.GetComponentInChildren<Text>().text = "Debug On";
    }

    private void ToggleText(Text text) => text.gameObject.SetActive(!text.gameObject.activeSelf);

    private void ToggleOptions()
    {
        if(options.activeSelf)
        {
            toggleOptionsButton.GetComponentInChildren<Text>().text = "O";
            options.SetActive(false);
        }
        else 
        {
            toggleOptionsButton.GetComponentInChildren<Text>().text = "X";
            options.SetActive(true);
        }
    }
}
