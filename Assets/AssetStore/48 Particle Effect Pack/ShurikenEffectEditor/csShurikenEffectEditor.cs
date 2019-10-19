using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;

[System.Serializable]

public class csShurikenEffectEditor : EditorWindow
{
    private float Scale = 1;

    public GameObject Effect;
    public Color ShurikenSystemColor = Color.white;
    static csShurikenEffectEditor myWindow;

	[MenuItem("Window/Shuriken System Effect Editor")]

	public static void Init()
	{
        myWindow = EditorWindow.GetWindowWithRect<csShurikenEffectEditor>(new Rect(100, 100, 300, 220)); //set Editor Position and Size
		myWindow.title = "Scale Editor";
	}

    void OnGUI()
    {
        GUILayout.Box("Shuriken System Effect Scale Editor", GUILayout.Width(295));
        EditorGUILayout.Space();
        Effect = (GameObject)EditorGUILayout.ObjectField("Shuriken System Effect", Effect, typeof(GameObject), true);
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        Scale = float.Parse(EditorGUILayout.TextField("Scale Change Value", Scale.ToString()));
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        if (GUILayout.Button("Scale Apply", GUILayout.Height(70)))
        {
            if (Effect.GetComponent<csShurikenEffectChanger>() != null)
                Effect.GetComponent<csShurikenEffectChanger>().ShurikenParticleScaleChange(Scale);
            else
            {
                Effect.AddComponent<csShurikenEffectChanger>();
                Effect.GetComponent<csShurikenEffectChanger>().ShurikenParticleScaleChange(Scale);
            }
            DestroyImmediate(Effect.GetComponent<csShurikenEffectChanger>());
        }
    }

}
#endif
