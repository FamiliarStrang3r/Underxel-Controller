using System;
using UnityEngine;
using UnityEditor;

[Serializable]
public struct BoolVariable
{
    [SerializeField] private bool variableValue;
    public event Action<bool> OnValueChanged;

    public bool Value
    {
        get => variableValue;
        set
        {
            if (value != variableValue)
            {
                variableValue = value;
                OnValueChanged?.Invoke(variableValue);
            }
        }
    }
}

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