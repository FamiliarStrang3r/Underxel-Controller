using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelSettings))]
public class LevelSettingsEditor : Editor
{
    private LevelSettings lvl = null;

    //private SerializedProperty gridSize = null;
    //private SerializedProperty cellData = null;

    private void OnEnable()
    {
        lvl = (LevelSettings)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(GUILayout.Button("Edit"))
        {
            var window = EditorWindow.GetWindow<GridWindow>();
            window.EditLevel(lvl);
        }
    }
}
