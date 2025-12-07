using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;

    [Header("UI References")]
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI totalCoinText;
    [SerializeField] private TextMeshProUGUI sessionCoinText;
    [SerializeField] private TextMeshProUGUI oxygenLevelText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private Button upgradeButton;

    [Header("Upgrade Settings")]
    [SerializeField] private int baseCost = 10;
    [SerializeField] private float decreaseAmount = 0.1f;
    [SerializeField] private float minRate = 0.2f;
    [SerializeField] private float startingRate = 1.5f;

    private float currentRate;
    private const string OXYGEN_RATE_KEY = "SavedOxygenRate";

    private void Awake()
    {
        InitializeSingleton();
    }

    private void Start()
    {
        LoadOxygenRate();
        HidePanel();
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

    private void LoadOxygenRate()
    {
        currentRate = PlayerPrefs.GetFloat(OXYGEN_RATE_KEY, startingRate);
    }

    public void ShowShopPanel()
    {
        if (panel != null)
        {
            panel.SetActive(true);
            UpdateUI();
        }
    }

    public void HidePanel()
    {
        if (panel != null)
        {
            panel.SetActive(false);
        }
    }

    private void UpdateUI()
    {
        UpdateCoinDisplay();
        UpdateOxygenDisplay();
        UpdateUpgradeButton();
    }

    private void UpdateCoinDisplay()
    {
        if (CoinManager.instance == null) return;

        if (totalCoinText != null)
        {
            totalCoinText.text = "Total Coins: " + CoinManager.instance.GetTotalCoins();
        }

        if (sessionCoinText != null)
        {
            sessionCoinText.text = "Session Coins: " + CoinManager.instance.GetSessionCoins();
        }
    }

    private void UpdateOxygenDisplay()
    {
        if (oxygenLevelText != null)
        {
            oxygenLevelText.text = "Oxygen Speed: " + currentRate.ToString("F1");
        }
    }

    private void UpdateUpgradeButton()
    {
        bool isMaxLevel = currentRate <= minRate;
        bool canAfford = CoinManager.instance != null &&
                         CoinManager.instance.GetTotalCoins() >= baseCost;

        if (costText != null)
        {
            costText.text = isMaxLevel ? "MAX LEVEL" : "Upgrade (" + baseCost + ")";
        }

        if (upgradeButton != null)
        {
            upgradeButton.interactable = !isMaxLevel && canAfford;
        }
    }

    public void Upgrade()
    {
        if (!CanUpgrade()) return;

        ProcessUpgrade();
        SaveUpgrade();
        UpdateUI();
    }

    private bool CanUpgrade()
    {
        return CoinManager.instance != null &&
               CoinManager.instance.GetTotalCoins() >= baseCost &&
               currentRate > minRate;
    }

    private void ProcessUpgrade()
    {
        CoinManager.instance.SpendCoins(baseCost);
        currentRate -= decreaseAmount;
        currentRate = Mathf.Max(currentRate, minRate);
    }

    private void SaveUpgrade()
    {
        PlayerPrefs.SetFloat(OXYGEN_RATE_KEY, currentRate);
        PlayerPrefs.Save();
    }

    // Getter
    public float GetCurrentRate() => currentRate;
}