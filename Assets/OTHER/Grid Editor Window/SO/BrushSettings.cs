using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "Brush Settings", menuName = "SO/Brush Settings")]
public class BrushSettings : ScriptableObject
{
    [SerializeField] private Brush[] brushes = new Brush[0];
    public Brush[] Brushes => brushes;

    [ContextMenu("Save Colors")]
    private void SaveColors()
    {
        string savePath = UnityEditor.EditorUtility.OpenFolderPanel("Choose save path", Application.dataPath, string.Empty);

        if (string.IsNullOrEmpty(savePath))
        {
            string message = "You didn't choose save path, saving process cancelled";
            UnityEditor.EditorUtility.DisplayDialog("Error", message, "OK");
        }
        else
        {
            savePath += "/brushes.json";
            var data = new BrushData(brushes);
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(savePath, json);
            UnityEditor.AssetDatabase.Refresh();

            Debug.Log($"saved to: {savePath}");
        }
    }

    [ContextMenu("Fill Array")]
    private void FillArray()
    {
        BrushData data = null;
        string savePath = UnityEditor.EditorUtility.OpenFilePanel("Choose brushes file", Application.dataPath, "json");

        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            data = JsonUtility.FromJson<BrushData>(json);
            Debug.Log($"loaded from: {savePath}");
        }
        else
        {
            string message = "You didn't choose file, all brush colors is white";
            UnityEditor.EditorUtility.DisplayDialog("Error", message, "OK");
        }

        var names = Enum.GetNames(typeof(CellType));
        brushes = new Brush[names.Length];

        for (int i = 0; i < brushes.Length; i++)
        {
            var brushName = names[i];
            var type = (CellType)i;
            var color = data != null ? data.brushes[i].cellColor : Color.white;

            brushes[i] = new Brush(brushName, type, color);
        }
    }
}

public enum CellType
{
    Empty,
    Wall,
    Sasha,
    Gena
}

[Serializable]
public class Brush
{
    [ReadOnly] public string name = "Empty";
    [ReadOnly] public CellType cellType = CellType.Empty;
    public Color cellColor = Color.white;

    public Brush(string _name, CellType type, Color color)
    {
        name = _name;
        cellType = type;
        cellColor = color;
    }
}

[Serializable]
public class BrushData
{
    public Brush[] brushes;

    public BrushData(Brush[] _brushes)
    {
        brushes = _brushes;
    }
}

