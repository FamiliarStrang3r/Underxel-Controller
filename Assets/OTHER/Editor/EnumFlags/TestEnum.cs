using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class TestEnum : MonoBehaviour
{
    [Flags]
    public enum Tag
    {
        Breakable = 1,
        Mergeable = 2,
        Movable = 4,
    }

    [SerializeField] private Tag tags;
    [SerializeField] private Tag myTag;

    [ContextMenu("Deb")]
    private void Deb()
    {
        var has = tags.HasFlag(myTag);
        Debug.Log(has);
    }

    private void Reset()
    {
        myTag = Tag.Breakable | Tag.Mergeable;
    }
}
