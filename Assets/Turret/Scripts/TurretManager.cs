using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TurretManager : MonoBehaviour
{
    [SerializeField] private TurretDatabase turretDatabase = null;

    [Header("Buttons")]
    [SerializeField] private Transform buttonsParent = null;
    [SerializeField] private TurretButton turretButtonPrefab = null;

    private TurretButton[] turretButtons = null;
    private SaveData saveData = null;

    private int selectedIndex = 0;
    public static readonly string SELECTED_TURRET_INDEX_KEY = "SELECTED_TURRET_INDEX_KEY";

    private void Start()
    {
        PopulateButtons();
    }

    private void PopulateButtons()
    {
        int has = buttonsParent.childCount;
        int need = turretDatabase.Length;
        turretButtons = new TurretButton[need];

        Load();

        for (int i = 0; i < has; i++)
        {
            turretButtons[i] = buttonsParent.GetChild(i).GetComponent<TurretButton>();
        }

        if (has < need)
        {
            int difference = need - has;

            for (int i = 0; i < difference; i++)
            {
                turretButtons[has + i] = Instantiate(turretButtonPrefab, buttonsParent);
            }
        }

        for (int i = 0; i < turretButtons.Length; i++)
        {
            turretButtons[i].UpdateButton(turretDatabase.GetTurret(i), selectedIndex, saveData.Opened[i]);
        }
    }

    public void SetNewIndex(int index)
    {
        if (selectedIndex != index)
        {
            for (int i = 0; i < turretButtons.Length; i++)
            {
                turretButtons[i].ChangeCheckbox(index);
            }

            selectedIndex = index;

            Save();
        }
    }

    public static void UnlockAt(int index)
    {
        string path = Application.dataPath + "/save.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);

            SaveData old = JsonUtility.FromJson<SaveData>(json);

            old.Opened[index] = true;

            json = JsonUtility.ToJson(old, true);

            File.WriteAllText(path, json);

            Debug.Log($"вы открыли новую турель, ее индекс: {index}");
        }
    }

    private void Save()
    {
        PlayerPrefs.SetInt(SELECTED_TURRET_INDEX_KEY, selectedIndex);

        string json = JsonUtility.ToJson(saveData, true);
        string path = Application.dataPath + "/save.json";
        File.WriteAllText(path, json);
    }

    private void Load()
    {
        selectedIndex = PlayerPrefs.GetInt(SELECTED_TURRET_INDEX_KEY, 0);

        string path = Application.dataPath + "/save.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            saveData = JsonUtility.FromJson<SaveData>(json);

            var old = saveData.Opened;

            saveData = new SaveData(new bool[turretDatabase.Length]);

            int count = turretDatabase.Length < old.Length ? turretDatabase.Length : old.Length;

            for (int i = 0; i < count; i++)
            {
                saveData.Opened[i] = old[i];
            }
        }
        else
        {
            saveData = new SaveData(new bool[turretDatabase.Length]);

            for (int i = 0; i < 2; i++)
            {
                saveData.Opened[i] = true;
            }

            selectedIndex = 0;
        }
    }
}

[System.Serializable]
public class SaveData
{
    public bool[] Opened;

    public SaveData(bool[] opened)
    {
        Opened = opened;
    }
}
