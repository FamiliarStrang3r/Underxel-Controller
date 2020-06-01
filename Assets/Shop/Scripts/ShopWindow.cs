using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopWindow : MonoBehaviour
{
    [SerializeField] private Text moneyLabel = null;
    [SerializeField] private Transform elementsParent = null;
    [SerializeField] private ShopElement elementPrefab = null;

    private ShopElement[] shopElements = null;
    private ShopController shopController = null;

    private void Awake()
    {
        shopController = FindObjectOfType<ShopController>();
    }

    private void Start()
    {
        PopulateButtons();
        UpdateElements();
    }

    private void OnEnable()
    {
        shopController.OnMoneyChanged += OnMoneyChanged;
    }

    private void OnDisable()
    {
        shopController.OnMoneyChanged -= OnMoneyChanged;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = 0; i < shopElements.Length; i++)
            {
                var e = shopElements[i];
                int index = e.transform.GetSiblingIndex();
                Debug.Log(index);
            }
        }
    }

    private void PopulateButtons()
    {
        //destroy all existing
        for (int i = 0; i < elementsParent.childCount; i++)
        {
            var child = elementsParent.GetChild(i);
            Destroy(child.gameObject);
        }

        //spawn new
        shopElements = new ShopElement[shopController.RobotsDatabase.Robots.Length];

        for (int i = 0, length = shopElements.Length; i < length; i++)
        {
            var element = Instantiate(elementPrefab, elementsParent);
            int k = i;
            element.OnClick = () => OnClick(k);
            element.name = $"Element: {i}";
            element.transform.SetSiblingIndex(i);
            shopElements[i] = element;
        }
    }

    private void UpdateElements()
    {
        for (int i = 0, length = shopElements.Length; i < length; i++)
        {
            var robot = shopController.RobotsDatabase.Robots[i];
            shopElements[i].UpdateVisual(robot, i == shopController.SelectedIndex);
        }
    }

    public void OnClick(int buttonIndex)
    {
        var robot = shopController.RobotsDatabase.Robots[buttonIndex];

        if (robot.isOpened)
        {
            shopController.SelectedIndex = buttonIndex;
        }
        else
        {
            if (shopController.MoneyAmount >= robot.costAmount)
            {
                shopController.MoneyAmount -= robot.costAmount;
                robot.isOpened = true;
            }
            else
            {
                //недостаточно денег
            }
        }

        UpdateElements();
    }

    private void OnMoneyChanged(int amount)
    {
        moneyLabel.text = $"У тебя {amount} деняк";

        //UpdateElements();
    }
}
