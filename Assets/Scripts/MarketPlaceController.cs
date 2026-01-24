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

    public void PopulateMarket()
    {
        if (itemsParent == null || itemPrefab == null) return;

        foreach (Transform child in itemsParent)
            Destroy(child.gameObject);

        foreach (var item in availableItems)
        {
            if (item == null) continue;

            MarketItemView view = Instantiate(itemPrefab, itemsParent);
            view.Setup(item, this);     // MarketItemView parametresi MarketPlaceController olmalı (aşağıda veriyorum)
            view.RefreshState();
        }
    }

    public void TryBuyItem(MarketItemData item)
    {
        if (item == null) return;

        // Cap kontrol
        if (!MarketSaveManager.CanBuyMore(item.itemID, item.maxPurchases))
        {
            Debug.Log("⚠ Bu item max seviyede.");
            return;
        }

        // Coin kontrol (CoinManager'da HasEnough yok, mevcut totalCoins ile kontrol ediyoruz)
        if (CoinManager.instance == null)
        {
            Debug.LogError("❌ CoinManager.instance yok!");
            return;
        }

        if (CoinManager.instance.GetTotalCoins() < item.price)
        {
            Debug.Log("❌ Yetersiz coin");
            return;
        }

        // Öde
        CoinManager.instance.SpendCoins(item.price);

        // Kaydet
        bool added = MarketSaveManager.TryAddItem(item.itemID, item.isConsumable, item.maxPurchases);
        if (!added)
        {
            Debug.Log("⚠ Eklenemedi (cap).");
            return;
        }

        // UI yenile
        PopulateMarket();
    }
}