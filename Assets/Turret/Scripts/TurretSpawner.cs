using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretSpawner : MonoBehaviour
{
    [SerializeField] private TurretDatabase turretDatabase = null;
    [SerializeField] private Transform spawnPoint = null;

    private void Start()
    {
        SpawnTurret();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3)) TurretManager.UnlockAt(2);
        else if (Input.GetKeyDown(KeyCode.Alpha4)) TurretManager.UnlockAt(3);
    }

    private void SpawnTurret()
    {
        int index = PlayerPrefs.GetInt(TurretManager.SELECTED_TURRET_INDEX_KEY, 0);
        Turret turretPrefab = turretDatabase.GetTurret(index);
        Instantiate(turretPrefab, spawnPoint.position, Quaternion.identity);
    }
}
