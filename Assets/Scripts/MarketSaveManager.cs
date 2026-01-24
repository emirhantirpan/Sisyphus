using UnityEngine;

public static class MarketSaveManager
{
    private const string ITEM_KEY_PREFIX = "MARKET_ITEM_";

    public static int GetItemCount(string itemID)
        => PlayerPrefs.GetInt(ITEM_KEY_PREFIX + itemID, 0);

    public static bool IsPurchased(string itemID)
        => GetItemCount(itemID) > 0;

    public static bool CanBuyMore(string itemID, int maxCount)
        => GetItemCount(itemID) < maxCount;

    public static bool TryAddItem(string itemID, bool consumable, int maxCount)
    {
        string key = ITEM_KEY_PREFIX + itemID;
        int current = PlayerPrefs.GetInt(key, 0);

        if (current >= maxCount) return false;

        if (consumable)
            PlayerPrefs.SetInt(key, current + 1);
        else
            PlayerPrefs.SetInt(key, current + 1); // upgrade stack (non-consumable ama sayısı var)

        PlayerPrefs.Save();
        return true;
    }

    public static bool TryConsumeItem(string itemID, int amount = 1)
    {
        string key = ITEM_KEY_PREFIX + itemID;
        int current = PlayerPrefs.GetInt(key, 0);
        if (current <= 0) return false;

        current -= amount;
        if (current < 0) current = 0;

        PlayerPrefs.SetInt(key, current);
        PlayerPrefs.Save();
        return true;
    }
}
