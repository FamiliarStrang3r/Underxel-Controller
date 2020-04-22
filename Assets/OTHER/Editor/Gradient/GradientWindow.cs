using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GradientWindow : EditorWindow
{
    private CustomGradient gradient = null;
    private int borderSize = 10;
    private Vector2Int keySize = new Vector2Int(10, 20);
    private Rect[] keyRects = null;

    private bool mouseIsOverKey = false;
    private int selectedKey = 0;
    private bool needRepaint = false;

    [MenuItem("Custom/Gradient %#r")]
    private static void OpenWindow()
    {
        var window = GetWindow<GradientWindow>();
        window.SetGradient(new CustomGradient());
    }

    public void SetGradient(CustomGradient g)
    {
        gradient = g;
    }

    private void OnGUI()
    {
        Rect r = new Rect(borderSize, borderSize, position.width - borderSize * 2, 32);
        GUI.DrawTexture(r, gradient.GetTexture((int)r.width));

        keyRects = new Rect[gradient.KeysCount];
        for (int i = 0; i < gradient.KeysCount; i++)
        {
            var key = gradient.GetKey(i);
            float x = r.x + r.width * key.Time - keySize.x / 2f;
            float y = r.yMax + borderSize;
            Rect keyRect = new Rect(new Vector2(x, y), keySize);

            if (i == selectedKey)
            {
                var pos = new Vector2(x, y) - Vector2.one * 2;
                var size = keySize + Vector2.one * 4;
                var outline = new Rect(pos, size);
                EditorGUI.DrawRect(outline, Color.black);
            }

            EditorGUI.DrawRect(keyRect, key.Color);
            keyRects[i] = keyRect;
        }

        Event e = Event.current;

        if (e.button == 0)
        {
            if (e.type == EventType.MouseDown)
            {
                for (int i = 0; i < keyRects.Length; i++)
                {
                    if (keyRects[i].Contains(e.mousePosition))
                    {
                        mouseIsOverKey = true;
                        selectedKey = i;
                        needRepaint = true;
                        break;
                    }
                }

                if (mouseIsOverKey)
                {

                }
                else
                {
                    var c = new Color(Random.value, Random.value, Random.value);
                    float time = Mathf.InverseLerp(r.x, r.xMax, e.mousePosition.x);
                    selectedKey = gradient.AddKey(c, time);
                    mouseIsOverKey = true;
                    needRepaint = true;
                }
            }
            else if (e.type == EventType.MouseUp)
            {
                mouseIsOverKey = false;
            }
            else if (e.type == EventType.MouseDrag)
            {
                if (mouseIsOverKey)
                {

                }
            }
        }

        if (needRepaint)
        {
            Repaint();
            needRepaint = false;
        }
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
    [SerializeField] private List<ColorKey> keys = new List<ColorKey>();

    public int KeysCount => keys.Count;

    public int AddKey(Color color, float time)
    {
        ColorKey key = new ColorKey(color, time);
        for (int i = 0; i < keys.Count; i++)
        {
            if (key.Time < keys[i].Time)
            {
                keys.Insert(i, key);
                return i;
            }
        }

        keys.Add(key);
        return keys.Count - 1;
    }

    public void UpdateKeyTime(int index, float time)
    {

    }

    public ColorKey GetKey(int index) => keys[index];

    public Color Evaluate(float time)
    {
        if (keys.Count == 0) return Color.white;

        ColorKey left = keys[0];
        ColorKey right = keys[keys.Count - 1];

        for (int i = 0; i < keys.Count - 1; i++)
        {
            if (keys[i].Time <= time && keys[i + 1].Time >= time)
            {
                left = keys[i];
                right = keys[i + 1];
                break;
            }
        }

        float blendTime = Mathf.InverseLerp(left.Time, right.Time, time);
        return Color.Lerp(left.Color, right.Color, blendTime);
    }

    public Texture2D GetTexture(int width)
    {
        Texture2D texture = new Texture2D(width, 1);
        Color[] colors = new Color[width];

        for (int i = 0; i < width; i++)
        {
            colors[i] = Evaluate((float)i / (width - 1));
        }

        texture.SetPixels(colors);
        texture.Apply();

        return texture;
    }

    [System.Serializable]
    public struct ColorKey
    {
        [SerializeField] private Color color;
        [SerializeField] private float time;

        public Color Color => color;
        public float Time => time;

        public ColorKey(Color c, float t)
        {
            color = c;
            time = t;
        }
    }
}
