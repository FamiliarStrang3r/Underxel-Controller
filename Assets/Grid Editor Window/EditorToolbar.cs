using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class EditorToolbar
{
    public int Index;
    public List<Window> Windows { get; }

    public string[] Names { get; }

    public EditorToolbar(int index, List<Window> windows)
    {
        Index = index;
        Windows = windows;
        List<string> temp = new List<string>();
        for (int i = 0; i < windows.Count; i++) temp.Add(windows[i].Name);
        Names = temp.ToArray();
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
