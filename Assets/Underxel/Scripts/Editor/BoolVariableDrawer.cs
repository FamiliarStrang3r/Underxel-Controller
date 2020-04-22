using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(BoolVariable))]
public class BoolVariableDrawer : PropertyDrawer
{
    //how to make readonly: GUI.enabled = false; EditorGUI.PropertyField(); GUI.enabled = true;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        position = EditorGUI.PrefixLabel(position, label);
        EditorGUI.PropertyField(position, property.FindPropertyRelative("variableValue"), GUIContent.none);

        EditorGUI.EndProperty();
    }
}
