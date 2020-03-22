using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSettings : ScriptableObject
{
    [SerializeField] private Map map = new Map();
    public Map Map => map;

    public void SetData(Map _map)
    {
        map = _map;
    }
}

