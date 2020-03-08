using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GradientWindow : EditorWindow
{
    private CustomGradient gradient = null;
    private int borderSize = 10;

    public void SetGradient(CustomGradient g)
    {
        gradient = g;
    }

    private void OnGUI()
    {
        Rect r = new Rect(borderSize, borderSize, position.width - borderSize * 2, 32);
        GUI.DrawTexture(r, gradient.GetTexture((int)r.width));
    }
}

[CustomPropertyDrawer(typeof(CustomGradient))]
public class GradientDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var gradient = (CustomGradient)fieldInfo.GetValue(property.serializedObject.targetObject);
        float w = GUI.skin.label.CalcSize(label).x + 5;
        Rect r = new Rect(position.x + w, position.y, position.width - w, position.height);

        Event e = Event.current;

        if (e.type == EventType.Repaint)
        {
            GUI.Label(position, label);

            GUIStyle style = new GUIStyle();
            style.normal.background = gradient.GetTexture((int)position.width);
            GUI.Label(r, GUIContent.none, style);
        }
        else if (e.type == EventType.MouseDown)
        {
            if (e.button == 0)
            {
                if (r.Contains(e.mousePosition))
                {
                    var window = EditorWindow.GetWindow<GradientWindow>();
                    window.SetGradient(gradient);
                }
            }
        }
    }
}

[System.Serializable]
public class CustomGradient
{
    public Color Evaluate(float time)
    {
        return Color.Lerp(Color.white, Color.black, time);
    }

    public Texture2D GetTexture(int width)
    {
        Texture2D texture = new Texture2D(width, 1);
        Color[] colors = new Color[width];

        for (int i = 0; i < width; i++)
        {
            colors[i] = Evaluate((float)i / width - 1);
        }

        texture.SetPixels(colors);
        texture.Apply();

        return texture;
    }
}
