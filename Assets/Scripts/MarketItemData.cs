using UnityEngine;

public enum MarketItemType
{
    OxygenUpgrade,
    CoinMultiplier,
    ScoreMultiplier,
    DoubleClicker
}

[CreateAssetMenu(menuName = "Marketplace/Item")]
public class MarketItemData : ScriptableObject
{
    [Header("Identity")]
    public string itemID;      // örn: oxygen_tube
    public string itemName;    // UI adı
    public MarketItemType itemType;

    [Header("UI")]
    public Sprite icon;
    [TextArea] public string description;

    [Header("Economy")]
    public int price;
    public bool isConsumable;
    public int maxPurchases = 1;

    [Header("Gameplay Values")]
    [Tooltip("OxygenUpgrade için: her satın alım başına artış (örn 20).")]
    public int intValue = 0;

    [Tooltip("Multiplier'lar için: her satın alım başına artış (örn 0.2).")]
    public float floatValue = 0f;
}