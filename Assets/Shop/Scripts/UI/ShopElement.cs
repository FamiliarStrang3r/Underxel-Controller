using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopElement : MonoBehaviour
{
    [SerializeField] private Text priceLabel = null;
    [SerializeField] private Image itemImage = null;
    [SerializeField] private Button buyButton = null;

    //[SerializeField] private Sprite selectedSprite = null;
    //[SerializeField] private Sprite openedSprite = null;
    //[SerializeField] private Sprite closedSprite = null;

    public System.Action OnClick;

    private void Start()
    {
        buyButton.onClick.AddListener(() => OnClick());
    }

    public void UpdateVisual(ShopItem item, bool isSelected)
    {
        //if (itemImage)
        itemImage.sprite = item.sprite;

        bool isOpened = item.isOpened;

        var buttonText = buyButton.GetComponentInChildren<Text>();
        buttonText.text = isOpened ? (isSelected ? "Выбрано" : "Выбрать") : "Купить";

        var buttonImage = buyButton.GetComponent<Image>();
        //buttonImage.sprite = isOpened ? (isSelected ? selectedSprite : openedSprite) : closedSprite;
        buttonImage.color = isOpened ? (isSelected ? Color.yellow : Color.green) : Color.red;

        priceLabel.text = isOpened ? string.Empty : $"Цена: {item.costAmount}";
    }
}