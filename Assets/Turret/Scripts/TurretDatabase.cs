using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[System.Serializable]
//public class TurretBlueprint : ScriptableObject
//{
//    public string Name = string.Empty;
//    //public bool IsOpen = false;
//    public Sprite Sprite = null;
//    public Turret Prefab = null;
//}

[CreateAssetMenu(fileName = "New turret database", menuName = "SO/Turret Database")]
public class TurretDatabase : ScriptableObject
{
    [SerializeField] private TurretBlueprint[] turretDatabase = null;

    public int Length => turretDatabase.Length;
    public TurretBlueprint GetTurret(int index) => turretDatabase[index];
}
