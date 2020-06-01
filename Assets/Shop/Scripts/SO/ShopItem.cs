using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShopItem
{
    public Sprite sprite = null;
    public int costAmount = 100;
    public bool isOpened = false;

    public RobotController prefab = null;
}
