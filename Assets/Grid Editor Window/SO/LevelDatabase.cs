using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "LevelDatabase", menuName = "SO/Level Database")]
public class LevelDatabase : ScriptableObject
{
    [SerializeField] private List<LevelSettings> levels = new List<LevelSettings>();

    [ContextMenu("Add")]
    private void Add()
    {
        var so = CreateInstance<LevelSettings>();
        so.name = levels.Count.ToString();

        levels.Add(so);

        AssetDatabase.AddObjectToAsset(so, this);

        ImportAsset();
    }

    [ContextMenu("Delete All")]
    private void DeleteAll()
    {
        for (int i = 0; i < levels.Count; i++)
        {
            AssetDatabase.RemoveObjectFromAsset(levels[i]);
        }

        levels = new List<LevelSettings>();

        ImportAsset();
    }

    private void ImportAsset(bool debugPath = false)
    {
        string path = AssetDatabase.GetAssetPath(this);
        if (debugPath) Debug.Log(path);
        AssetDatabase.ImportAsset(path);
    }

    //public void AddLevel(Map map)
    //{
    //    var so = CreateInstance<LevelSettings>();
    //    so.name = levels.Count.ToString();
    //    levels.Add(so);
    //    so.SetData(map);
    //    AssetDatabase.AddObjectToAsset(so, this);
    //    ImportAsset();
    //}
}
