using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New turret", menuName = "SO/Turret")]
public class TurretBlueprint : ScriptableObject
{
    //[SerializeField] private string turretName = string.Empty;
    [SerializeField] private Sprite turretSprite = null;
    [SerializeField] private Turret turretPrefab = null;

    //public string Name => turretName;
    public Sprite Sprite => turretSprite;
    public Turret Prefab => turretPrefab;
}
