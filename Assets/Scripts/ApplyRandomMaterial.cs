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
    private ARFaceManager faceManager;

    private MeshRenderer faceManagerFaceRenderer;
    
    [SerializeField]
    private bool debugOn = false;

    private GameObject debugGameObject;

    void Start()
    {
        applyRandomMaterialButton.onClick.AddListener(GenerateRandomMaterial);
        if(debugOn)
            debugGameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
    }

    void GenerateRandomMaterial()
    {
        if(faceManagerFaceRenderer == null)
        {
            Debug.LogError("Face Manager did not get a Mesh Renderer");
            return;
        }

        GenerateMaterial();
    }

    private void GenerateMaterial()
    {
        Material randomMaterial = new Material(Shader.Find("Standard"));
        randomMaterial.name = $"Random_Material";
        randomMaterial.color = GetRandomColor();
        randomMaterial.EnableKeyword("_EMISSION");
        randomMaterial.SetInt("_Cull", 0);
        randomMaterial.SetColor("_EmissionColor", randomMaterial.color);
        if(!debugOn)
            faceManager.facePrefab.GetComponent<MeshRenderer>().material = randomMaterial;
        else
            faceManagerFaceRenderer = debugGameObject.GetComponent<MeshRenderer>();
    }

    static Color GetRandomColor() => Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
}
