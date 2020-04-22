using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EditorToolbar
{
    public int Index;
    public string[] Names { get; }

    private KeyCode[] keyCodes = new KeyCode[]
    {
        KeyCode.Alpha1,
        KeyCode.Alpha2,
        KeyCode.Alpha3,
        KeyCode.Alpha4,
        KeyCode.Alpha5,
        KeyCode.Alpha6,
        KeyCode.Alpha7,
        KeyCode.Alpha8,
        KeyCode.Alpha9
    };

    public EditorToolbar(int index, string[] names)
    {
        Index = index;
        Names = names;
    }

    //public EditorToolbar(int index, IEnumerable<Window> windows)
    //{
    //    Index = index;
    //    Names = windows.Select(w => w.Name).ToArray();
    //}

    public void Draw(Rect rect)
    {
        Index = GUI.Toolbar(rect, Index, Names);
    }

    public bool WasKeyDown(Event e)
    {
        for (int i = 0; i < Names.Length; i++)
        {
            if (e.keyCode == keyCodes[i])
            {
                Index = i;
                return true;
            }
        }

        return false;
    }
}

//public class Window
//{
//    public string Name { get; }
//    public Action Action { get; }

//    public Window(string name, Action action)
//    {
//        Name = name;
//        Action = action;
//    }
//}
