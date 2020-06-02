using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.IO;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    public static ShopController Instance { get; private set; }

    private void InitSingleton()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public event Action<int> OnMoneyChanged;

    [SerializeField] private RobotsDatabase robotsDatabase = null;
    [SerializeField] private int startMoney = 100;
    [SerializeField] private int startIndex = 0;

    private string fileName = "saveShop.json";
    
    //private RobotsDatabase database = null;
    public RobotsDatabase RobotsDatabase
    {
        //get
        //{
        //    if (database == null)
        //    {
        //        //do it on awake?
        //        database = Instantiate(robotsDatabase);
        //    }

        //    return database;
        //}

        get; private set;
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
            //OnMoneyChanged(moneyAmount);
        }
    }

    public int SelectedIndex
    {
        get => selectedIndex;

        set
        {
            selectedIndex = value;
            Save();
        }
    }

    public RobotController SelectedRobot => RobotsDatabase.Robots[SelectedIndex].prefab;

    private void Awake()
    {
        InitSingleton();

        RobotsDatabase = Instantiate(robotsDatabase);

        Load();
    }

    private void OnEnable()
    {
        OnMoneyChanged += OnMoneyChange;
    }

    private void OnDisable()
    {
        OnMoneyChanged -= OnMoneyChange;
    }

    private void Start()
    {
        OnMoneyChanged?.Invoke(MoneyAmount);//invoke only at start because subscribing goes at OnEnable (after Awake, before Start)
    }

    private void Load()
    {
        string path = Path.Combine(Application.isEditor ? Application.dataPath : Application.persistentDataPath, fileName);
        if (File.Exists(path))
        {
            int maxCount = RobotsDatabase.Robots.Length;

            string json = File.ReadAllText(path);
            var data = JsonUtility.FromJson<ShopData>(json);
            MoneyAmount = data.moneyAmount;
            SelectedIndex = Mathf.Clamp(data.selectedIndex, 0, maxCount - 1);

            Array.Resize(ref data.opened, maxCount);

            for (int i = 0; i < maxCount; i++)
            {
                RobotsDatabase.Robots[i].isOpened = data.opened[i];
            }

            //Debug.Log("loaded");
        }
        else
        {
            MoneyAmount = startMoney;
            SelectedIndex = startIndex;
        }
    }

    private void Save()
    {
        bool[] opened = RobotsDatabase.Robots.Select(r => r.isOpened).ToArray();
        var data = new ShopData(MoneyAmount, SelectedIndex, opened);
        string json = JsonUtility.ToJson(data, true);
        string path = Path.Combine(Application.isEditor ? Application.dataPath : Application.persistentDataPath, fileName);
        File.WriteAllText(path, json);

        //Debug.Log("saved");
    }

    private void OnMoneyChange(int amount)
    {
        Save();
    }

    private void OnLevelWasLoaded(int level)
    {
        OnMoneyChanged?.Invoke(MoneyAmount);
        //Debug.Log(level);
    }
}

[Serializable]
public class ShopData
{
    public int moneyAmount = 0;
    public int selectedIndex = 0;
    public bool[] opened = null;

    public ShopData(int _moneyAmount, int _selectedIndex, params bool[] _opened)
    {
        moneyAmount = _moneyAmount;
        selectedIndex = _selectedIndex;
        opened = _opened;
    }
}

