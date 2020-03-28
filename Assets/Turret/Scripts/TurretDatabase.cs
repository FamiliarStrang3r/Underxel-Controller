using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TurretDatabase : MonoBehaviour
{
    [SerializeField] private Turret[] turretPrefabs = null;

    public int Length => turretPrefabs.Length;
    public Turret GetTurret(int index) => turretPrefabs[index];

    public static TurretDatabase Instance { get; private set; }

    public static string SavePath { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        string fileName = "save.json";

        SavePath = Application.isEditor ? Path.Combine(Application.dataPath, fileName) : Path.Combine(Application.persistentDataPath, fileName);
    }
}

