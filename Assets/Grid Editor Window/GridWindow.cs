using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using UnityEditor;

public class GridWindow : EditorWindow
{
    private Cell[] cells = null;
    private int width = 5;
    private int height = 5;

    private int cellSize = 50;
    private int spacing = 5;
    private int padding = 10;
    //private int leftPadding = 10;
    //private int rightPadding = 10;

    private static int sideWindowWidth = 300;
    public static BrushSettings brushSettings = null;
    private EditorToolbar toolbar = null;
    //private MonoScript script = null;

    [MenuItem("Custom/Grid %#g")]
    private static void OpenWindow()
    {
        var window = GetWindow<GridWindow>();
        //window.minSize = Vector2.one * sideWindowWidth;
    }

    private void OnEnable()
    {
        //script = MonoScript.
        UpdateToolbar();
    }

    private void OnGUI()
    {
        Event e = Event.current;

        DrawSideWindow();

        if (brushSettings)
        {
            //FillGrid();
            DrawToolbar();
            HandleEvents(e);
        }
    }

    private void DrawToolbar()
    {
        Vector2 center = position.center - position.position;
        int count = toolbar.Names.Length;
        Vector2 size = new Vector2(50 * count, 30);
        Vector2 pos = center - size / 2;
        pos.x -= sideWindowWidth / 2f;
        pos.y = size.y / 2;

        Rect rect = new Rect(pos, size);
        toolbar.Draw(rect);
    }

    private void HandleEvents(Event e)
    {
        if (e.type == EventType.KeyDown)
        {
            if (e.keyCode == KeyCode.Alpha1) toolbar.Index = 0;
            else if (e.keyCode == KeyCode.Alpha2) toolbar.Index = 1;
            else if (e.keyCode == KeyCode.Alpha3) toolbar.Index = 2;
            else if (e.keyCode == KeyCode.Alpha4) toolbar.Index = 3;

            Repaint();
        }
        else if (e.type == EventType.Repaint)
        {
            DrawGrid();
            FillGrid();
        }
        else if (e.type == EventType.MouseDown)
        {
            if (e.button == 0)
                HandleLeftMouseDown(e);
            else if (e.button == 1)
                HandleRightMouseDown(e);
        }
        else if (e.type == EventType.MouseDrag)
        {
            if (e.button == 0)
                HandleLeftMouseDown(e);
            else
                HandleRightMouseDown(e);
        }
    }

    private void HandleLeftMouseDown(Event e)
    {
        for (int i = 0; i < cells.Length; i++)
        {
            var cell = cells[i];
            if (cell.HasMousePosition(e))
            {
                var type = (CellType)toolbar.Index;
                cell.UpdateType(type);

                Repaint();
                break;
            }
        }
    }

    private void HandleRightMouseDown(Event e)
    {
        for (int i = 0; i < cells.Length; i++)
        {
            var cell = cells[i];
            if (cell.HasMousePosition(e))
            {
                cell.UpdateType(CellType.Empty);

                Repaint();
                break;
            }
        }
    }

    private void DrawSideWindow()
    {
        Rect rect = new Rect(position.width - sideWindowWidth, 0, sideWindowWidth, position.height);
        //GUI.Box(rect, string.Empty,EditorStyles.helpBox);
        //Rect drawRect = new Rect(rect.position + Vector2.right * leftPadding, rect.size - Vector2.right * rightPadding);

        var style = new GUIStyle(EditorStyles.helpBox)
        {
            padding = new RectOffset(padding, padding, padding, 0)
        };

        GUILayout.BeginArea(rect, style);
        DrawFields();
        GUILayout.EndArea();
    }

    private void DrawFields()
    {
        GUI.enabled = false;
        //var script = EditorGUILayout.ObjectField("Script", this, typeof(GridWindow), false) as GridWindow;
        EditorGUILayout.ObjectField("Script", this, typeof(GridWindow), false);
        GUI.enabled = true;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Properties", EditorStyles.boldLabel);
        EditorGUI.BeginChangeCheck();
        width = EditorGUILayout.IntSlider("Width", width, 1, 10);
        height = EditorGUILayout.IntSlider("Height", height, 1, 10);
        if (EditorGUI.EndChangeCheck())
        {
            if (brushSettings) UpdateCells();
        }

        //sideWindowWidth = EditorGUILayout.IntField("Side width", sideWindowWidth);
        //leftPadding = EditorGUILayout.IntField("L Padding", leftPadding);
        //rightPadding = EditorGUILayout.IntField("R Padding", rightPadding);
        cellSize = EditorGUILayout.IntSlider("Cell size", cellSize, 30, 70);
        spacing = EditorGUILayout.IntSlider("Spacing", spacing, 0, 10);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Brush", EditorStyles.boldLabel);
        EditorGUI.BeginChangeCheck();
        brushSettings = EditorGUILayout.ObjectField("Brush settings", brushSettings, typeof(BrushSettings), false) as BrushSettings;
        if (EditorGUI.EndChangeCheck())
        {
            UpdateToolbar();
        }

        if (brushSettings)
        {
            DrawButtons();
        }
    }

    private void UpdateToolbar()
    {
        if (brushSettings)
        {
            Window[] buttons = new Window[brushSettings.Brushes.Length];

            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i] = new Window(brushSettings.Brushes[i].name, null);
            }

            toolbar = new EditorToolbar(0, buttons.ToList());

            UpdateCells();
        }
    }

    private void UpdateCells()
    {
        cells = new Cell[width * height];

        for (int i = 0; i < cells.Length; i++)
        {
            //cells[i] = new Cell(brushSettings, CellType.Empty);
            cells[i] = new Cell();
        }
    }

    private void FillGrid()
    {
        Vector2 center = position.center - position.position;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                //Vector2 pos = new Vector2(-width / 2f + x, -height / 2f + y);
                Vector2 pos = new Vector2(-width / 2f + x, (height - 2) / 2f - y);
                Vector2 spacingOffset = Vector2.one * spacing / 2f;
                Vector2 position = center + pos * cellSize + pos * spacing + spacingOffset;
                position.x -= sideWindowWidth / 2f;

                var index = x + width * y;
                var rect = new Rect(position, Vector2.one * cellSize);

                cells[index].Set(rect, index);
            }
        }
    }

    private void DrawGrid()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].Draw(EditorStyles.helpBox);
        }
    }

    private void DrawButtons()
    {
        //EditorGUILayout.BeginVertical(EditorStyles.helpBox);

        EditorGUILayout.Space();

        if (GUILayout.Button("Save"))
        {
            var data = new CellData(cells, width, height);
            string json = JsonUtility.ToJson(data, true);
            string savePath = Application.dataPath + "/cells.json";
            File.WriteAllText(savePath, json);
            AssetDatabase.Refresh();
        }
        else if (GUILayout.Button("Load"))
        {
            string savePath = Application.dataPath + "/cells.json";
            if (File.Exists(savePath))
            {
                string json = File.ReadAllText(savePath);
                var data = JsonUtility.FromJson<CellData>(json);
                width = data.Width;
                height = data.Height;
                cells = data.Cells;
            }
        }

        //EditorGUILayout.EndVertical();
    }
}

