using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretManager : MonoBehaviour
{
    public static TurretManager Instance { get; private set; }

    [SerializeField] private TurretDatabase turretDatabase = null;
    [SerializeField] private bool[] opened = null;
    [SerializeField] private int selectedTurretIndex = 0;
    [SerializeField] private Transform spawnPoint = null;
    [SerializeField] private GameObject selectMenu = null;
    public Turret CurrentTurret = null;
    public Button Btn = null;

    [Header("Buttons")]
    [SerializeField] private Transform buttonsParent = null;
    [SerializeField] private TurretButton turretButtonPrefab = null;

    private TurretButton[] turretButtons = null;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Btn.onClick.AddListener(() =>
        {
            if (CurrentTurret != null) Destroy(CurrentTurret.gameObject);
            selectMenu.SetActive(!selectMenu.activeSelf);

        });
        PopulateButtons();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            UnlockAt(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            UnlockAt(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            UnlockAt(3);
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            selectMenu.SetActive(false);
            if (CurrentTurret != null) Destroy(CurrentTurret.gameObject);
            CurrentTurret = Instantiate(turretDatabase.GetTurret(selectedTurretIndex).Prefab, spawnPoint.position, Quaternion.identity);
        }
    }

    private void UnlockAt(int index)
    {
        opened[index] = true;
        turretButtons[index].Unlock(true);
    }

    private void PopulateButtons()
    {
        int has = buttonsParent.childCount;
        int need = turretDatabase.Length;
        turretButtons = new TurretButton[need];

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
            turretButtons[i].UpdateButton(turretDatabase.GetTurret(i), selectedTurretIndex, opened[i]);
        }
    }

    public void SetNewIndex(int index)
    {
        if (selectedTurretIndex != index)
        {
            turretButtons[selectedTurretIndex].ChangeCheckbox(index);
            selectedTurretIndex = index;
            turretButtons[selectedTurretIndex].ChangeCheckbox(selectedTurretIndex);
        }
    }
}
