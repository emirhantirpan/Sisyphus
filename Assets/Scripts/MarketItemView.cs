using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MarketItemView : MonoBehaviour
{
    [Header("UI")]
    public Image iconImage;
    public TMP_Text nameText;
    public TMP_Text priceText;
    public TMP_Text stackText;
    public Button buyButton;

    private MarketItemData itemData;
    private MarketPlaceController marketplace;

    public void Setup(MarketItemData data, MarketPlaceController controller)
    {
        itemData = data;
        marketplace = controller;

        if (iconImage != null) iconImage.sprite = data.icon;
        if (nameText != null) nameText.text = data.itemName;
        if (priceText != null) priceText.text = data.price.ToString();

        if (buyButton != null)
        {
            buyButton.onClick.RemoveAllListeners();
            buyButton.onClick.AddListener(() => marketplace.TryBuyItem(itemData));
        }
    }

    public void RefreshState()
    {
        if (itemData == null) return;

        int count = MarketSaveManager.GetItemCount(itemData.itemID);

        if (stackText != null)
        {
            stackText.gameObject.SetActive(true);
            stackText.text = $"x{count}/{itemData.maxPurchases}";
        }

        bool canBuy = count < itemData.maxPurchases;

        if (buyButton != null) buyButton.interactable = canBuy;
        if (!canBuy && priceText != null) priceText.text = "MAX";
    }
}