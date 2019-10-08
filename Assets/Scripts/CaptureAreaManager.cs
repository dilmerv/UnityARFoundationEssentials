using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CaptureAreaManager : MonoBehaviour
{

    [SerializeField]
    private Button topLeft;

    [SerializeField]
    private Button bottomRight;

    [SerializeField]
    private Rect captureRegion;

    // Update is called once per frame
    void Update()
    {
        var topLeftRect = topLeft.GetComponent<Rect>();

        captureRegion.position = new Vector2(topLeftRect.x, topLeftRect.y);
    }
}
