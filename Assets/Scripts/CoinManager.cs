using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance;

    public TextMeshProUGUI coinText;
    public TextMeshProUGUI totalCoinText;

    public int sessionCoins = 0;
    public int totalCoins = 0;

    private void Awake()
    {
        if (instance == null) instance = this;
        else { Destroy(gameObject); return; }
    }

    private void Start()
    {
        totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);
        sessionCoins = 0;
        UpdateCoinText();
        UpdateTotalCoinText();
    }

    public void AddCoin(int amount)
    {
        if (GameStateManager.instance != null && GameStateManager.instance.isGameOver) return;

        sessionCoins += amount;
        totalCoins += amount;
        UpdateCoinText();
        UpdateTotalCoinText();

        SaveCoins(); 
    }

    public void SaveCoins()
    {
        PlayerPrefs.SetInt("TotalCoins", totalCoins);
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
        if (coinText != null) coinText.text = sessionCoins.ToString();
    }

    private void UpdateTotalCoinText()
    {
        if (totalCoinText != null) totalCoinText.text = totalCoins.ToString();
    }
}
