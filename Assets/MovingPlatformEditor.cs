using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MovingPlatform))]
public class MovingPlatformEditor : Editor
{
    private MovingPlatform platform = null;
    private bool needRepaint = false;

    private int pixelDistance = 10;     //pixel treshold from mouse to line, used to insert new point
    private float radius = 1f;          //radius disk draw size in world coordinates

    private int lineIndex = -1;
    private bool isMouseOverLine = false;

    private int pointIndex = -1;
    private bool isMouseOverPoint = false;

    private void OnEnable()
    {
        platform = target as MovingPlatform;
        //Undo.ClearUndo(platform);//testing
        //Debug.Log(nameof(OnEnable));
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        //DrawDefaultInspector();//same as higher

        if (platform.Points.Count > 0)
        {
            if (GUILayout.Button("Clear"))
            {
                Undo.RecordObject(platform, "Clear list");//gavnojopa works fine at 2018
                platform.Points.Clear();
                //needRepaint = true;//doesnt work - should repaint sceneview after click
            }
        }
    }

    private void OnSceneGUI()
    {
        //Debug.Log(nameof(OnSceneGUI));

        Draw();

        Event e = Event.current;

        if (e.type == EventType.Layout)
        {
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
        }
        else
        {
            HandleInput(e);
            if (needRepaint) HandleUtility.Repaint();
        }
    }

    private void HandleInput(Event e)
    {
        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
        float height = 0;                                                       // Y coordinate of point
        float distance = (height - ray.origin.y) / ray.direction.y;
        Vector3 mousePos = ray.GetPoint(distance);
        mousePos.y = 0;

        if (e.button == 0)
        {
            if (e.type == EventType.MouseDown)
            {
                if (e.modifiers == EventModifiers.None)
                {
                    if (!isMouseOverPoint)
                    HandleLeftMouseDown(mousePos);
                }
                else if (e.modifiers == EventModifiers.Control)
                {
                    if (isMouseOverPoint)
                        platform.Points.RemoveAt(pointIndex);
                }
            }
        }

        UpdateMouse(mousePos);
    }

    private void HandleLeftMouseDown(Vector3 pos)
    {
        Undo.RecordObject(platform, "Add point");//gavnojopa works fine at 2018
        int newPointIndex = isMouseOverLine ? lineIndex + 1 : platform.Points.Count;
        platform.Points.Insert(newPointIndex, pos);
        //needRepaint = true;//doesnt matter
    }

    private void Draw()
    {
        for (int i = 0; i < platform.Points.Count; i++)
        {
            EditorGUI.BeginChangeCheck();
            Vector3 pos = Handles.PositionHandle(platform.Points[i], Quaternion.identity);
            if(EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(platform, "Move point");//gavnojopa works fine at 2018
                platform.Points[i] = pos;
            }

            Vector3 next = platform.Points[(i + 1) % platform.Points.Count];

            DrawVisualDebug(i, next);
        }

        needRepaint = false;
    }

    private void DrawVisualDebug(int i, Vector3 next)
    {
        //draw arrow
        if (platform.Points.Count > 2)
        {
            Handles.color = Color.green;
            Vector3 middle = Vector3.Lerp(platform.Points[i], next, 0.5f);
            Vector3 dir = next - platform.Points[i];
            int id = GUIUtility.GetControlID(FocusType.Passive);
            float size = HandleUtility.GetHandleSize(middle);//constant size
            Handles.ArrowHandleCap(id + i * 10, middle - dir.normalized, Quaternion.LookRotation(dir), 2, EventType.Repaint);
        }

        //draw dotted line from point to point
        Handles.color = i == lineIndex ? Color.red : Color.white;
        Handles.DrawDottedLine(platform.Points[i], next, 5);

        //draw yellow/white transparent disk
        var debugColor = i == pointIndex ? Color.yellow : Color.white;
        debugColor.a = 0.1f;
        Handles.color = debugColor;
        Handles.DrawSolidDisc(platform.Points[i], Vector3.up, radius);
    }

    private void UpdateMouse(Vector3 pos)
    {
        int pointNearIndex = -1;
        for (int i = 0; i < platform.Points.Count; i++)
        {
            //return pixel distance, but disk have constant radius in world coordinates
            //float distance = HandleUtility.DistanceToDisc(platform.Points[i], Vector3.up, radius);

            //returns world distance, doesnt affect on Y coordinate, point with Y != 0 coordinate finds uncorrectly
            float distance = Vector3.Distance(pos, platform.Points[i]);

            if (distance < radius)
            {
                pointNearIndex = i;
                break;
            }
        }

        if (pointIndex != pointNearIndex)
        {
            pointIndex = pointNearIndex;
            isMouseOverPoint = pointNearIndex != -1;
            needRepaint = true;
        }

        if (isMouseOverPoint)
        {
            isMouseOverLine = false;
            lineIndex = -1;
        }
        else
        {
            int mouseOverLineIndex = -1;
            float closestLineDistance = pixelDistance;

            for (int i = 0; i < platform.Points.Count; i++)
            {
                Vector3 next = platform.Points[(i + 1) % platform.Points.Count];
                float distance = HandleUtility.DistanceToLine(platform.Points[i], next);

                if (distance < closestLineDistance)
                {
                    closestLineDistance = distance;
                    mouseOverLineIndex = i;
                }
            }

            if (lineIndex != mouseOverLineIndex)
            {
                lineIndex = mouseOverLineIndex;
                isMouseOverLine = mouseOverLineIndex != -1;
                needRepaint = true;
            }
        }
    }
}
