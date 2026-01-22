using UnityEngine;

public static class MarketSaveManager
{
    private const string ITEM_KEY_PREFIX = "MARKET_ITEM_";

    public static bool IsPurchased(string itemID)
    {
        return PlayerPrefs.GetInt(ITEM_KEY_PREFIX + itemID, 0) > 0;
    }

    public static int GetItemCount(string itemID)
    {
        return PlayerPrefs.GetInt(ITEM_KEY_PREFIX + itemID, 0);
    }

    public static void AddItem(string itemID, bool consumable)
    {
        string key = ITEM_KEY_PREFIX + itemID;

        if (consumable)
        {
            int current = PlayerPrefs.GetInt(key, 0);
            PlayerPrefs.SetInt(key, current + 1);
        }
        else
        {
            PlayerPrefs.SetInt(key, 1); // Non-consumable
        }

        PlayerPrefs.Save();
    }
    public static void ConsumeItem(string itemID, int amount = 1)
    {
        string key = "MARKET_ITEM_" + itemID;
        int current = PlayerPrefs.GetInt(key, 0);

        current -= amount;
        current = Mathf.Max(0, current);

        PlayerPrefs.SetInt(key, current);
        PlayerPrefs.Save();

        Debug.Log($"🧾 {itemID} consumed. Remaining: {current}");
    }
}