using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TurretManager : MonoBehaviour
{
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
        int need = TurretDatabase.Instance.Length;
        turretButtons = new TurretButton[need];

        LoadFromFile();

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
            var t = TurretDatabase.Instance.GetTurret(i);
            turretButtons[i].UpdateButton(t, selectedIndex, saveData.Opened[i]);
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

            PlayerPrefs.SetInt(SELECTED_TURRET_INDEX_KEY, selectedIndex);
        }
    }

    public static void UnlockAt(int index)
    {
        if (File.Exists(TurretDatabase.SavePath))
        {
            string json = File.ReadAllText(TurretDatabase.SavePath);

            SaveData old = JsonUtility.FromJson<SaveData>(json);

            old.Opened[index] = true;

            json = JsonUtility.ToJson(old, true);

            File.WriteAllText(TurretDatabase.SavePath, json);

            Debug.Log($"вы открыли новую турель, ее индекс: {index}");
        }
    }

    private void SaveToFile()
    {
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(TurretDatabase.SavePath, json);
    }

    private void LoadFromFile()
    {
        selectedIndex = PlayerPrefs.GetInt(SELECTED_TURRET_INDEX_KEY, 0);

        int dbLength = TurretDatabase.Instance.Length;

        if (File.Exists(TurretDatabase.SavePath))
        {
            string json = File.ReadAllText(TurretDatabase.SavePath);
            saveData = JsonUtility.FromJson<SaveData>(json);

            var old = saveData.Opened;

            saveData = new SaveData(new bool[dbLength]);

            int count = dbLength < old.Length ? dbLength : old.Length;

            for (int i = 0; i < count; i++)
            {
                saveData.Opened[i] = old[i];
            }
        }
        else
        {
            saveData = new SaveData(new bool[dbLength]);

            for (int i = 0; i < 2; i++)
            {
                saveData.Opened[i] = true;
            }

            selectedIndex = 0;
            PlayerPrefs.SetInt(SELECTED_TURRET_INDEX_KEY, selectedIndex);
            SaveToFile();
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
