using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SceneLoader : EditorWindow
{
    private int _currentStage;

    [MenuItem("My Tools/SceneLoader")]
    public static void ShowWindow()
    {
        GetWindow(typeof(SceneLoader));
    }

    public void OnGUI()
    {
        _currentStage = EditorGUILayout.IntSlider("Stage Number", _currentStage, 0, 3);


        if (GUILayout.Button("LoadScene"))
        {
            SceneLoadManager.currentStage = _currentStage;
            if (_currentStage > 1)
            {
                SceneLoadManager.currentStage = 1;
                SceneLoadManager.OpenStage();
            }
            else
            {
                SceneLoadManager.OpenStage();
            }
        }

        // Layout the UI
        if (GUILayout.Button("NextScene"))
        {
            if (SceneLoadManager.currentStage > 1)
            {
                SceneLoadManager.NextStage();
            }
            else
            {
                SceneLoadManager.currentStage = 1;
                SceneLoadManager.OpenStage();
            }
        }
    }
}
