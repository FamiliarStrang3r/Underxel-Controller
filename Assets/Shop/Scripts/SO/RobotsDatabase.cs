using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New database", menuName = "SO/Robots/New database")]
public class RobotsDatabase : ScriptableObject
{
    [SerializeField] private ShopItem[] robots = null;

    public ShopItem[] Robots => robots;
}
