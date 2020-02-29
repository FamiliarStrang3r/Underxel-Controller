using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(SomebodyController))]
public class SomebodyControllerEditor : Editor
{
    private bool drawDefault = false;
    private bool useStyle = true;

    private string currentToolbarName = string.Empty;

    private SerializedProperty script;

    private SerializedProperty walkSpeed, crouchSpeed, runSpeed;
    private SerializedProperty isGrounded, isMoving, isCrouching;
    private SerializedProperty horizontalAxis, verticalAxis;

    private Toolbar[] toolbars = null;

    private void OnEnable()
    {
        toolbars = new Toolbar[]
        {
            new Toolbar(0, new List<Window>
            {
                new Window("1", DrawFloat),
                new Window("2", DrawBool),
                new Window("3", DrawString)
            }),
            new Toolbar(-1, new List<Window>
            {
                new Window("4", () => EditorGUILayout.TextField("это")),
                new Window("5", () => EditorGUILayout.TextField("просто")),
                new Window("6", () => EditorGUILayout.TextField("охуенно"))
            }),
            new Toolbar(-1, new List<Window>
            {
                new Window("7", () => EditorGUILayout.TextField("весьма")),
                new Window("8", () => EditorGUILayout.TextField("шедеврально")),
                new Window("9", () => EditorGUILayout.TextField("блеать")),
                new Window("10", null)
            }),
        };

        var b = Array.Find(toolbars, bar => bar.Index == 0);
        currentToolbarName = b.Names[0];

        script = serializedObject.FindProperty("m_Script");
        PopulateProperties();
    }

    private void PopulateProperties()
    {
        walkSpeed = serializedObject.FindProperty("walkSpeed");
        crouchSpeed = serializedObject.FindProperty("crouchSpeed");
        runSpeed = serializedObject.FindProperty("runSpeed");

        isGrounded = serializedObject.FindProperty("isGrounded");
        isMoving = serializedObject.FindProperty("isMoving");
        isCrouching = serializedObject.FindProperty("isCrouching");

        horizontalAxis = serializedObject.FindProperty("horizontalAxis");
        verticalAxis = serializedObject.FindProperty("verticalAxis");
    }

    public override void OnInspectorGUI()
    {
        if (drawDefault)
        {
            //base.OnInspectorGUI();
            DrawDefaultInspector();
        }
        else
        {
            DrawScriptHeader();
            DrawEditorNew();
        }

        EditorGUILayout.Space();
        drawDefault = EditorGUILayout.Toggle("is Default", drawDefault);
    }

    private void DrawScriptHeader()
    {
        GUI.enabled = false;
        EditorGUILayout.PropertyField(script);//custom name - PropertyField(script, new GUIContent("name here"));
        GUI.enabled = true;
    }

    private void DrawFloat()
    {
        EditorGUILayout.PropertyField(walkSpeed);
        EditorGUILayout.PropertyField(crouchSpeed);
        EditorGUILayout.PropertyField(runSpeed);
    }

    private void DrawBool()
    {
        EditorGUILayout.PropertyField(isGrounded);
        EditorGUILayout.PropertyField(isMoving);
        EditorGUILayout.PropertyField(isCrouching);
    }

    private void DrawString()
    {
        EditorGUILayout.PropertyField(horizontalAxis);
        EditorGUILayout.PropertyField(verticalAxis);
    }

    private void DrawEditorNew()
    {
        DrawAllToolbars();

        EditorGUI.BeginChangeCheck();
        DrawProperties();
        if (EditorGUI.EndChangeCheck()) serializedObject.ApplyModifiedProperties();
    }

    private class Toolbar
    {
        public int Index;
        public List<Window> Windows { get; }

        public string[] Names { get; }

        public Toolbar(int index, List<Window> windows)
        {
            Index = index;
            Windows = windows;
            List<string> temp = new List<string>();
            for (int i = 0; i < windows.Count; i++) temp.Add(windows[i].Name);
            Names = temp.ToArray();
        }
    }

    private class Window
    {
        public string Name { get; }
        public Action Action { get; }

        public Window(string name, Action action)
        {
            Name = name;
            Action = action;
        }
    }

    private void DrawAllToolbars()
    {
        useStyle = EditorGUILayout.Toggle("Use style", useStyle);
        GUIStyle style = useStyle ? EditorStyles.toolbarButton : null;

        for (int i = 0; i < toolbars.Length; i++)
        {
            bool last = i == toolbars.Length - 1;
            bool clicked = DrawToolbar(toolbars[i], style, last);

            if (clicked)
            {
                foreach (var tb in toolbars) if (tb != toolbars[i]) tb.Index = tb.Names.Length;
                //foreach (var bar in toolbars.Where(t => t != toolbars[i])) bar.Index = bar.Names.Length;
            }
        }
    }

    private bool DrawToolbar(Toolbar toolbar, GUIStyle style = null, bool isLast = false)
    {
        EditorGUI.BeginChangeCheck();
        toolbar.Index = GUILayout.Toolbar(toolbar.Index, toolbar.Names, style);
        if (isLast) EditorGUILayout.Space();

        bool changed = EditorGUI.EndChangeCheck();
        if (changed)
        {
            currentToolbarName = toolbar.Names[toolbar.Index];
            GUI.FocusControl(string.Empty);
            //Debug.Log(currentToolbarName);
        }

        return changed;
    }

    private void DrawProperties()
    {
        for (int i = 0; i < toolbars.Length; i++)
        {
            if (Array.Exists(toolbars[i].Names, name => name == currentToolbarName))
            {
                int index = Array.IndexOf(toolbars[i].Names, currentToolbarName);
                toolbars[i].Windows[index].Action?.Invoke();
                break;
            }
        }
    }

}
