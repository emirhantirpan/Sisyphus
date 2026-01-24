using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI coinText;       // Game: SessionCoins | Menu: LastRunCoins
    [SerializeField] private TextMeshProUGUI totalCoinText;  // Total coins (her sahnede)

    [Header("Scene Mode")]
    [SerializeField] private bool isMenuScene = false; // Menü sahnesindeki CoinManager'da TRUE yap

    private int sessionCoins = 0;
    private int totalCoins = 0;

    private float coinMultiplier = 1f;
    private bool doubleClickActive = false;

    private const string TOTAL_COINS_KEY = "TotalCoins";
    private const string LAST_RUN_COINS_KEY = "LastRunCoins";

    private void Awake()
    {
        InitializeSingleton();
    }

    private void Start()
    {
        LoadCoins();

        if (isMenuScene)
        {
            UpdateMenuTexts();
        }
        else
        {
            ResetSessionCoins();
            UpdateTotalCoinText();
        }
    }

    private void InitializeSingleton()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void LoadCoins()
    {
        totalCoins = PlayerPrefs.GetInt(TOTAL_COINS_KEY, 0);
    }

    // --- MARKET / UPGRADE API ---
    public bool HasEnough(int amount) => totalCoins >= amount;

    public void SpendCoins(int amount)
    {
        if (totalCoins < amount) return;

        totalCoins -= amount;
        UpdateTotalCoinText();
        SaveTotalCoins();
    }

    public void SetCoinMultiplier(float value) => coinMultiplier = Mathf.Max(0f, value);
    public void SetDoubleClickActive(bool active) => doubleClickActive = active;

    // --- GAMEPLAY ---
    public void AddCoin(int amount)
    {
        if (IsGameOver()) return;

        float mult = coinMultiplier * (doubleClickActive ? 2f : 1f);
        int finalAmount = Mathf.RoundToInt(amount * mult);

        sessionCoins += finalAmount;
        totalCoins += finalAmount;

        UpdateGameTexts();
        SaveTotalCoins();
    }

    /// <summary>
    /// Run bittiğinde (GameOver) bir kere çağır: menüde göstermek için "son run coin" kaydeder.
    /// </summary>
    public void SaveLastRunCoins()
    {
        PlayerPrefs.SetInt(LAST_RUN_COINS_KEY, sessionCoins);
        PlayerPrefs.Save();
    }

    private void SaveTotalCoins()
    {
        PlayerPrefs.SetInt(TOTAL_COINS_KEY, totalCoins);
        PlayerPrefs.Save();
    }

    public void ResetSessionCoins()
    {
        sessionCoins = 0;
        UpdateCoinText(sessionCoins); // Game sahnesinde session gösteriyoruz
    }

    // --- UI ---
    private void UpdateGameTexts()
    {
        // Game sahnesi: coinText = session
        UpdateCoinText(sessionCoins);
        UpdateTotalCoinText();
    }

    private void UpdateMenuTexts()
    {
        // Menu sahnesi: coinText = last run
        int lastRunCoins = PlayerPrefs.GetInt(LAST_RUN_COINS_KEY, 0);
        UpdateCoinText(lastRunCoins);
        UpdateTotalCoinText();
    }

    private void UpdateCoinText(int value)
    {
        if (coinText != null)
            coinText.text = value.ToString();
    }

    private void UpdateTotalCoinText()
    {
        if (totalCoinText != null)
            totalCoinText.text = totalCoins.ToString();
    }

    private bool IsGameOver()
    {
        return GameStateManager.instance != null && GameStateManager.instance.isGameOver;
    }

    // Getters
    public int GetSessionCoins() => sessionCoins;
    public int GetTotalCoins() => totalCoins;
}
