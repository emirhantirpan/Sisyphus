using UnityEngine;

public class GameplayItemApplier : MonoBehaviour
{
    [Header("Base Values")]
    [SerializeField] private float baseOxygen = 100f;
    [SerializeField] private float baseScoreMultiplier = 1.5f;

    [Header("Marketplace Item Datas (Assign ScriptableObjects)")]
    [SerializeField] private MarketItemData oxygenUpgradeData;     // OxygenUpgrade (intValue=20, max=10)
    [SerializeField] private MarketItemData coinMultiplierData;    // CoinMultiplier (floatValue=0.2, max=5)
    [SerializeField] private MarketItemData scoreMultiplierData;   // ScoreMultiplier (floatValue=0.2, max=5)
    [SerializeField] private MarketItemData doubleClickerData;     // DoubleClicker (max=20, consumable=true)

    private void Start()
    {
        ApplyPermanentUpgrades();
        ApplyDoubleClickerConsumable();
    }

    private void ApplyPermanentUpgrades()
    {
        ApplyOxygenUpgrade();
        ApplyCoinMultiplier();
        ApplyScoreMultiplier();
    }

    private void ApplyOxygenUpgrade()
    {
        if (oxygenUpgradeData == null) return;

        int count = MarketSaveManager.GetItemCount(oxygenUpgradeData.itemID);
        count = Mathf.Min(count, oxygenUpgradeData.maxPurchases);

        if (OxygenSlider.instance != null)
        {
            float max = baseOxygen + (count * oxygenUpgradeData.intValue);
            OxygenSlider.instance.SetMaxStamina(max);
        }
    }

    private void ApplyCoinMultiplier()
    {
        if (coinMultiplierData == null) return;

        int count = MarketSaveManager.GetItemCount(coinMultiplierData.itemID);
        count = Mathf.Min(count, coinMultiplierData.maxPurchases);

        float coinMult = 1f + (count * coinMultiplierData.floatValue);
        CoinManager.instance?.SetCoinMultiplier(coinMult);
    }

    private void ApplyScoreMultiplier()
    {
        if (scoreMultiplierData == null) return;

        int count = MarketSaveManager.GetItemCount(scoreMultiplierData.itemID);
        count = Mathf.Min(count, scoreMultiplierData.maxPurchases);

        float scoreMult = baseScoreMultiplier + (count * scoreMultiplierData.floatValue);
        ScoreManager.instance?.SetUpgradeMultiplier(scoreMult);
    }

    private void ApplyDoubleClickerConsumable()
    {
        if (doubleClickerData == null)
        {
            CoinManager.instance?.SetDoubleClickActive(false);
            return;
        }

        // Run başında 1 tane tüket (varsa)
        int count = MarketSaveManager.GetItemCount(doubleClickerData.itemID);
        count = Mathf.Min(count, doubleClickerData.maxPurchases);

        if (count > 0)
        {
            bool consumed = MarketSaveManager.TryConsumeItem(doubleClickerData.itemID, 1);
            CoinManager.instance?.SetDoubleClickActive(consumed);
        }
        else
        {
            CoinManager.instance?.SetDoubleClickActive(false);
        }
    }
}
