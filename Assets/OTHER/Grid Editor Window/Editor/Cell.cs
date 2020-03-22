using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    private string title;
    private Rect rect;
    private Color cellColor;
    public CellData cellData;
    private GridWindow gridWindow;

    public Cell(int x, int y, CellType type, GridWindow window)
    {
        cellData = new CellData(x, y, type);
        gridWindow = window;
        cellColor = gridWindow.GetColorFromType(type);
    }

    public void Set(Rect _rect, string _title, int x, int y)
    {
        rect = _rect;
        title = _title;
        cellData.UpdateCoorditane(x, y);
    }

    public void Draw(GUIStyle style)
    {
        //var oldColor = GUI.color;
        GUI.color = cellColor;
        GUI.Box(rect, title, style);
        //GUI.color = oldColor;
    }

    public bool HasMousePosition(Event e) => rect.Contains(e.mousePosition);

    public bool TypeUpdated(CellType newType)
    {
        if (cellData.Type != newType)
        {
            cellData.UpdateType(newType);
            cellColor = gridWindow.GetColorFromType(newType);
            return true;
        }

        return false;
    }
}
