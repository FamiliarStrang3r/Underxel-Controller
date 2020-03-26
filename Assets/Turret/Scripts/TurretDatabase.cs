using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New turret database", menuName = "SO/Turret Database")]
public class TurretDatabase : ScriptableObject
{
    //[SerializeField] private TurretBlueprint[] turretDatabase = null;
    [SerializeField] private Turret[] turretPrefabs = null;

    public int Length => turretPrefabs.Length;
    public Turret GetTurret(int index) => turretPrefabs[index];
}
