using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotSpawner : MonoBehaviour
{
    [SerializeField] private Vector3 position = Vector3.zero;

    private void Start()
    {
        var prefab = ShopController.Instance.SelectedRobot;
        Instantiate(prefab, position, Quaternion.identity);
    }
}
