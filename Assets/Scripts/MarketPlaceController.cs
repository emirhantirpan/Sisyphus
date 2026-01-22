using System.Collections.Generic;
using UnityEngine;

public class MarketPlaceController : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private Transform itemsParent;
    [SerializeField] private MarketItemView itemPrefab;

    [Header("Items")]
    [SerializeField] private List<MarketItemData> availableItems;

    private void Start()
    {
        PopulateMarket();
    }

    private void PopulateMarket()
    {
        foreach (Transform child in itemsParent)
            Destroy(child.gameObject);

        foreach (var item in availableItems)
        {
            MarketItemView view = Instantiate(itemPrefab, itemsParent);
            view.Setup(item, this);
            view.RefreshState();
        }

    }
    public void TryBuyItem(MarketItemData item)
    {
        // Non-consumable tekrar satın alınmasın
        if (!item.isConsumable && MarketSaveManager.IsPurchased(item.itemID))
        {
            Debug.Log("⚠ Bu item zaten satın alınmış");
            return;
        }

        if (!CurrencyManager.Instance.HasEnough(item.price))
        {
            Debug.Log("❌ Yetersiz para");
            return;
        }

        CurrencyManager.Instance.Spend(item.price);

        // KALICI KAYIT
        MarketSaveManager.AddItem(item.itemID, item.isConsumable);

        ApplyItemEffect(item);
    }


    private void ApplyItemEffect(MarketItemData item)
    {
        switch (item.itemType)
        {
            case MarketItemType.OxygenTube:
                Debug.Log("🫁 Oxygen + " + item.value);
                break;

            case MarketItemType.ScoreMultiplier:
                Debug.Log("✖ Score Multiplier x" + item.value);
                break;

            case MarketItemType.DoubleClicker:
                Debug.Log("🖱 Double Click Activated");
                break;
        }
    }
}