using UnityEngine;

public class GameplayItemApplier : MonoBehaviour
{
    private bool doubleClickUsedThisRun = false;

    private void Start()
    {
        ApplyPermanentUpgrades();
    }

    private void ApplyPermanentUpgrades()
    {
        ApplyOxygenUpgrade();
        ApplyScoreMultiplierUpgrade();
        ApplyCoinUpgrade();
    }
    

    #region Permanent Upgrades

    private void ApplyOxygenUpgrade()
    {
        int count = MarketSaveManager.GetItemCount("oxygen_tube");

        if (OxygenSlider.instance != null)
        {
            float baseOxygen = 3400;
            float bonus = count * 50f;

            OxygenSlider.instance.SetMaxStamina(baseOxygen + bonus);

            Debug.Log($"🫁 Oxygen Max: {baseOxygen + bonus}");
        }
    }

    private void ApplyScoreMultiplierUpgrade()
    {
        int count = MarketSaveManager.GetItemCount("score_multiplier");

        if (ScoreManager.instance != null)
        {
            float baseMultiplier = 1.5f;
            float bonus = count * 0.5f;

            ScoreManager.instance.SetExternalMultiplier(baseMultiplier + bonus);

            Debug.Log($"✖ Score Multiplier: {baseMultiplier + bonus}");
        }
    }

    private void ApplyCoinUpgrade()
    {
        int count = MarketSaveManager.GetItemCount("coin_bonus");

        if (CoinManager.instance != null)
        {
            CoinManager.instance.SetCoinMultiplier(1 + count);
            Debug.Log($"🪙 Coin Multiplier: x{1 + count}");
        }
    }

    #endregion

    #region Consumables



    public void OnRunEnded()
    {
        if (doubleClickUsedThisRun)
        {
            MarketSaveManager.ConsumeItem("double_clicker", 1);
            doubleClickUsedThisRun = false;

            Debug.Log("🧾 Double Clicker consumed");
        }
    }

    #endregion
}
