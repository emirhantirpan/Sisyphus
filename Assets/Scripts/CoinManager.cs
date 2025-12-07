using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI totalCoinText;

    private int sessionCoins = 0;
    private int totalCoins = 0;

    private const string TOTAL_COINS_KEY = "TotalCoins";

    private void Awake()
    {
        InitializeSingleton();
    }

    private void Start()
    {
        LoadCoins();
        ResetSessionCoins();
        UpdateTotalCoinText();
    }

    private void InitializeSingleton()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadCoins()
    {
        totalCoins = PlayerPrefs.GetInt(TOTAL_COINS_KEY, 0);
    }

    public void AddCoin(int amount)
    {
        if (IsGameOver()) return;

        sessionCoins += amount;
        totalCoins += amount;

        UpdateCoinText();
        UpdateTotalCoinText();
        SaveCoins();
    }

    public void SpendCoins(int amount)
    {
        if (totalCoins >= amount)
        {
            totalCoins -= amount;
            UpdateTotalCoinText();
            SaveCoins();
        }
    }

    public void SaveCoins()
    {
        PlayerPrefs.SetInt(TOTAL_COINS_KEY, totalCoins);
        PlayerPrefs.Save();
        Debug.Log("Coins saved: " + totalCoins);
    }

    public void ResetSessionCoins()
    {
        sessionCoins = 0;
        UpdateCoinText();
    }

    private void UpdateCoinText()
    {
        if (coinText != null)
        {
            coinText.text = sessionCoins.ToString();
        }
    }

    private void UpdateTotalCoinText()
    {
        if (totalCoinText != null)
        {
            totalCoinText.text = totalCoins.ToString();
        }
    }

    private bool IsGameOver()
    {
        return GameStateManager.instance != null && GameStateManager.instance.isGameOver;
    }

    // Getters
    public int GetSessionCoins() => sessionCoins;
    public int GetTotalCoins() => totalCoins;
}