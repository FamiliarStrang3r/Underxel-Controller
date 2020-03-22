using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

public class GridWindow : EditorWindow
{
    private Cell[,] cellsGrid = null;
    private int width = 5;
    private int height = 5;

    private int cellSize = 50;
    private int spacing = 5;
    private int padding = 10;
    private int sideWindowWidth = 300;
    private bool eraseIfRebuilded = false;

    private BrushSettings brushSettings = null;
    private LevelSettings levelSettings = null;
    private EditorToolbar toolbar = null;
    private SavedCells savedCells = null;

    [MenuItem("Custom/Grid %#g")]
    private static void OpenWindow()
    {
        GetWindow<GridWindow>();
    }

    private void OnEnable()
    {
        //LoadBrushGUID();
        LoadBrushResources();
        UpdateToolbar();
    }

    private void LoadBrushGUID()
    {
        string[] guids = AssetDatabase.FindAssets("t:BrushSettings");

        for (int i = 0; i < guids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            Debug.Log($"{i}: {path}");
        }

        if (guids != null && guids.Length > 0)
        {
            brushSettings = AssetDatabase.LoadAssetAtPath<BrushSettings>(guids[0]);
            Debug.Log(brushSettings != null);
        }
    }

    private void LoadBrushResources()
    {
        brushSettings = Resources.Load<BrushSettings>("Brush");
        //Debug.Log(brushSettings != null);
    }

    private void OnGUI()
    {
        Event e = Event.current;

        DrawSideWindow();
        DrawToolbar();
        HandleEvents(e);
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
            if (toolbar.WasKeyDown(e)) Repaint();
        }
        else if (e.type == EventType.Repaint)
        {
            FillGrid();
            DrawGrid();
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
            else if (e.button == 1)
                HandleRightMouseDown(e);
        }
    }

    private void HandleLeftMouseDown(Event e)
    {
        if (brushSettings == null) return;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var cell = cellsGrid[x, y];
                if (cell.HasMousePosition(e))
                {
                    if (cell.TypeUpdated((CellType)toolbar.Index))
                    {
                        SaveAfterChanging();
                        Repaint();
                    }
                    break;
                }
            }
        }
    }

    private void HandleRightMouseDown(Event e)
    {
        if (brushSettings == null) return;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var cell = cellsGrid[x, y];
                if (cell.HasMousePosition(e))
                {
                    if (cell.TypeUpdated(CellType.Empty))
                    {
                        SaveAfterChanging();
                        Repaint();
                    }
                    break;
                }
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
        EditorGUILayout.ObjectField("Script", this, typeof(GridWindow), false);
        GUI.enabled = true;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Properties", EditorStyles.boldLabel);
        EditorGUI.BeginChangeCheck();
        width = EditorGUILayout.IntSlider("Width", width, 1, 10);
        height = EditorGUILayout.IntSlider("Height", height, 1, 10);
        if (EditorGUI.EndChangeCheck())
        {
            UpdateCells();
        }

        cellSize = EditorGUILayout.IntSlider("Cell size", cellSize, 30, 70);
        spacing = EditorGUILayout.IntSlider("Spacing", spacing, 0, 10);
        eraseIfRebuilded = EditorGUILayout.Toggle("Erase if rebuild", eraseIfRebuilded);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Brush", EditorStyles.boldLabel);
        EditorGUI.BeginChangeCheck();
        brushSettings = EditorGUILayout.ObjectField("Brush settings", brushSettings, typeof(BrushSettings), false) as BrushSettings;
        if (EditorGUI.EndChangeCheck())
        {
            UpdateToolbar();
        }
        levelSettings = EditorGUILayout.ObjectField("Level settings", levelSettings, typeof(LevelSettings), false) as LevelSettings;

        if (brushSettings)
        {
            if (levelSettings)
            {
                if (GUILayout.Button("Save level"))
                {
                    SaveSO();
                }
            }
            else
            {
                if (GUILayout.Button("Save as new"))
                {
                    SaveNew();
                }
            }
        }
    }

    private void UpdateToolbar()
    {
        string[] names = new string[] { "None" };
        int activeIndex = 0;

        if (brushSettings)
        {
            activeIndex = 1;
            names = brushSettings.Brushes.Select(b => b.name).ToArray();
        }

        toolbar = new EditorToolbar(activeIndex, names);
        UpdateCells();
    }

    private void UpdateCells()
    {
        cellsGrid = new Cell[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                cellsGrid[x, y] = new Cell(x, y, CellType.Empty, this);
            }
        }

        if (savedCells != null)
        {
            int w = savedCells.GridWidth;
            int h = savedCells.GridHeight;

            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    if (x < width && y < height)
                        cellsGrid[x, y] = new Cell(x, y, savedCells.Data[x, y].Type, this);
                }
            }

            if (eraseIfRebuilded) SaveAfterChanging();
        }

        #region DOESNT WORK
        //int w = savedCells != null ? savedCells.GridWidth : width;
        //int h = savedCells != null ? savedCells.GridHeight : height;

        //for (int x = 0; x < w; x++)
        //{
        //    for (int y = 0; y < h; y++)
        //    {
        //        if (x < width && y < height)
        //        {
        //            var type = savedCells != null ? savedCells.Data[x, y].Type : CellType.Empty;
        //            cellsGrid[x, y] = new Cell(x, y, type);
        //        }
        //    }
        //}

        //Save();
        #endregion
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

                Rect rect = new Rect(position, Vector2.one * cellSize);
                int index = y * width + x;
                string title = $"{index}, [{x}:{y}]";

                //cells[index].Set(rect, title, x, y);//1d array
                cellsGrid[x, y].Set(rect, title, x, y);
            }
        }
    }

    private void DrawGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                cellsGrid[x, y].Draw(EditorStyles.helpBox);
            }
        }
    }

    private void SaveAfterChanging()
    {
        CellData[,] data = new CellData[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                data[x, y] = cellsGrid[x, y].cellData;
            }
        }

        savedCells = new SavedCells(data, width, height);
    }

    private void SaveSO()
    {
        string message = "Are you really want overwrite?";
        if (EditorUtility.DisplayDialog("Save options", message, "Yes", "Cancel"))
        {
            levelSettings.SetData(new Map(savedCells));
        }
        else
        {
            Debug.Log("cancelled");
        }
    }

    private void SaveNew()
    {
        string message = "Please enter message in code";
        string filePath = EditorUtility.SaveFilePanelInProject("Save file", "Exapmle", "asset", message);

        //Debug.Log(filePath);

        if (string.IsNullOrEmpty(filePath))
        {

        }
        else
        {
            levelSettings = CreateInstance<LevelSettings>();
            levelSettings.SetData(new Map(savedCells));

            AssetDatabase.CreateAsset(levelSettings, filePath);
            AssetDatabase.Refresh();
        }
    }

    public void EditLevel(LevelSettings level)
    {
        levelSettings = level;
        savedCells = new SavedCells(levelSettings.Map);

        width = savedCells.GridWidth;
        height = savedCells.GridHeight;

        UpdateToolbar();
    }

    public Color GetColorFromType(CellType type)
    {
        if (brushSettings)
            return System.Array.Find(brushSettings.Brushes, e => e.cellType == type).cellColor;
        else
            return Color.magenta;
    }
}
