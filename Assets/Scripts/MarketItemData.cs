using UnityEngine;

public enum MarketItemType
{
    OxygenTube,
    ScoreMultiplier,
    DoubleClicker
}

[CreateAssetMenu(menuName = "Marketplace/Item")]
public class MarketItemData : ScriptableObject
{
    [Header("Identity")]
    public string itemID;
    public string itemName;
    public MarketItemType itemType;

    [Header("UI")]
    public Sprite icon;
    [TextArea] public string description;

    [Header("Economy")]
    public int price;
    public bool isConsumable;

    [Header("Gameplay Values")]
    public int value; 
}
