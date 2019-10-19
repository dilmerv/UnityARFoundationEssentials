using UnityEngine;
using System.Collections;

public class csDestroyEffect : MonoBehaviour {
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.C))
        {
            Destroy(gameObject);
        }
    }
}
