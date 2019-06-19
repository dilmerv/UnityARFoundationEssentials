using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class ApplyRandomMaterial : MonoBehaviour
{
    [SerializeField]
    private Button applyRandomMaterialButton;

    [SerializeField]
    private Material faceMaterial;
    
    [SerializeField]
    private bool debugOn = false;
    private GameObject debugGameObject;

    void Start()
    {
        applyRandomMaterialButton.onClick.AddListener(GenerateRandomColorForMaterial);
        if(debugOn)
            debugGameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
    }

    private void GenerateRandomColorForMaterial()
    {
        faceMaterial.color = GetRandomColor();
    }

    static Color GetRandomColor() => Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
}
