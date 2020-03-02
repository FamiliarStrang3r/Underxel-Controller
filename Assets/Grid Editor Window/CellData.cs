using System;
using UnityEngine;

[Serializable]
public struct CellData
{
    [SerializeField] private string name;
    [SerializeField] private CellType type;
    [SerializeField] private Vector2Int coordinate;

    public CellType Type => type;

    public CellData(int x, int y, CellType _type)
    {
        coordinate = new Vector2Int(x, y);
        type = _type;
        name = Enum.GetName(typeof(CellType), type);
    }

    public void UpdateType(CellType _type)
    {
        type = _type;
        name = Enum.GetName(typeof(CellType), type);
    }

    public void UpdateCoorditane(int x, int y)
    {
        coordinate = new Vector2Int(x, y);
    }
}

public class SavedCells
{
    //user for storing data in runtime
    public CellData[,] Data;
    public int GridWidth;
    public int GridHeight;

    public SavedCells(CellData[,] data, int width, int height)
    {
        Data = data;
        GridWidth = width;
        GridHeight = height;
    }

    public SavedCells(Map map)
    {
        GridWidth = map.gridSize.x;
        GridHeight = map.gridSize.y;

        Data = new CellData[GridWidth, GridHeight];

        for (int x = 0; x < GridWidth; x++)
        {
            for (int y = 0; y < GridHeight; y++)
            {
                int index = y * GridWidth + x;
                Data[x, y] = map.cellData[index];
            }
        }
    }
}

[Serializable]
public struct Map
{
    public Vector2Int gridSize;
    public CellData[] cellData;

    public Map(SavedCells savedCells)
    {
        int width = savedCells.GridWidth;
        int height = savedCells.GridHeight;

        gridSize = new Vector2Int(width, height);
        cellData = new CellData[width * height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int index = y * width + x;
                cellData[index] = savedCells.Data[x, y];
            }
        }
    }
}
