using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    public event Action<int> OnMoneyChanged;

    [SerializeField] private RobotsDatabase robotsDatabase = null;
    [SerializeField] private int startMoney = 100;
    [SerializeField] private int startIndex = 0;

    private RobotsDatabase database = null;
    public RobotsDatabase RobotsDatabase
    {
        get
        {
            if (database == null)
            {
                //do it on awake?
                database = Instantiate(robotsDatabase);
            }

            return database;
        }
    }

    private int moneyAmount = 0;
    private int selectedIndex = 0;

    public int MoneyAmount
    {
        get => moneyAmount;

        set
        {
            moneyAmount = value;
            OnMoneyChanged?.Invoke(MoneyAmount);
        }
    }

    public int SelectedIndex
    {
        get => selectedIndex;

        set
        {
            selectedIndex = value;
        }
    }

    private void Awake()
    {
        MoneyAmount = startMoney;
        SelectedIndex = startIndex;
        Load();
    }

    private void Start()
    {
        OnMoneyChanged?.Invoke(MoneyAmount);//invoke only at start because subscribing goes at OnEnable (after Awake, before Start)
    }

    private void Load()
    {
        
    }

    private void Save()
    {

    }
}
