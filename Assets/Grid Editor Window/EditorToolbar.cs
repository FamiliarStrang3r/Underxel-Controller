using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

public class EditorToolbar
{
    public int Index;
    public string[] Names { get; }

    public EditorToolbar(int index, IEnumerable<Window> windows)
    {
        Index = index;
        Names = windows.Select(w => w.Name).ToArray();
    }

    public void Draw(Rect rect)
    {
        Index = GUI.Toolbar(rect, Index, Names);
    }
}

public class Window
{
    public string Name { get; }
    public Action Action { get; }

    public Window(string name, Action action)
    {
        Name = name;
        Action = action;
    }
}
