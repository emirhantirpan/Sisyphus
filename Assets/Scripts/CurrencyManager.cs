using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;

    [SerializeField] private int currency = 1000;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public bool HasEnough(int amount)
    {
        return currency >= amount;
    }

    public void Spend(int amount)
    {
        currency -= amount;
        Debug.Log("💰 Kalan para: " + currency);
    }
}