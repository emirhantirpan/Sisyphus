using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MarketItemView : MonoBehaviour
{
    [Header("UI")]
    public Image iconImage;
    public TMP_Text nameText;
    public TMP_Text priceText;
    public Button buyButton;
    [SerializeField] private TMP_Text stackText;


    private MarketItemData itemData;
    private MarketPlaceController marketplace;

    public void Setup(MarketItemData data, MarketPlaceController controller)
    {
        itemData = data;
        marketplace = controller;

        iconImage.sprite = data.icon;
        nameText.text = data.itemName;
        priceText.text = data.price.ToString();

        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(BuyItem);
        
        RefreshState();
    }

    private void BuyItem()
    {
        marketplace.TryBuyItem(itemData);
    }
    public void RefreshState()
    {
        int count = MarketSaveManager.GetItemCount(itemData.itemID);

        if (itemData.isConsumable)
        {
            stackText.gameObject.SetActive(count > 0);
            stackText.text = "x" + count;
        }
        else
        {
            if (count > 0)
            {
                buyButton.interactable = false;
                priceText.text = "OWNED";
            }
        }
    }

}