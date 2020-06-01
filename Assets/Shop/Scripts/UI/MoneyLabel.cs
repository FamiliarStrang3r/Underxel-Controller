using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyLabel : MonoBehaviour
{
    private Text label = null;
    private ShopController shopController = null;

    private void Awake()
    {
        label = GetComponent<Text>();
        shopController = FindObjectOfType<ShopController>();
    }

    private void OnEnable()
    {
        shopController.OnMoneyChanged += OnMoneyChange;
    }

    private void OnDisable()
    {
        shopController.OnMoneyChanged -= OnMoneyChange;
    }

    private void OnMoneyChange(int amount)
    {
        label.text = $"{amount} деняк";
    }
}
