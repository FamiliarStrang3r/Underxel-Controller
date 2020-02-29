using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Cell
{
    public int index;
    public Rect rect;
    public CellType type = CellType.Empty;
    public Color cellColor;

    public Cell()
    {
        UpdateType(CellType.Empty);
    }

    public void Set(Rect r, int _index)
    {
        rect = r;
        index = _index;
    }

    public void Draw(GUIStyle style)
    {
        //var oldColor = GUI.color;

        GUI.color = cellColor;
        GUI.Box(rect, index.ToString(), style);

        //GUI.color = oldColor;
    }

    public bool HasMousePosition(Event e) => rect.Contains(e.mousePosition);

    public void UpdateType(CellType _type)
    {
        type = _type;
        cellColor = System.Array.Find(GridWindow.brushSettings.Brushes, e => e.cellType == type).cellColor;
    }
}

[System.Serializable]
public class CellData
{
    public int Width;
    public int Height;
    public Cell[] Cells;

    public CellData(Cell[] cells, int w, int h)
    {
        Cells = cells;
        Width = w;
        Height = h;
    }
}
