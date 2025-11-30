using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;

    public GameObject panel;
    public TextMeshProUGUI totalCoinText;
    public TextMeshProUGUI oxygenLevelText;
    public TextMeshProUGUI costText;
    public Button upgradeButton;

    public int baseCost = 10;
    public float decreaseAmount = 0.1f;
    public float minRate = 0.2f;
    public float rate = 1.5f;

    private void Awake()
    {
        if (instance == null) instance = this;
        else { Destroy(gameObject); return; }
    }

    private void Start()
    {
        rate = PlayerPrefs.GetFloat("SavedOxygenRate", rate);
        if (panel != null) panel.SetActive(false);
    }

    public void ShowShopPanel()
    {
        if (panel != null) panel.SetActive(true);
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (totalCoinText != null) totalCoinText.text = "Toplam Coin: " + CoinManager.instance.totalCoins;
        if (oxygenLevelText != null) oxygenLevelText.text = "Oxygen Speed: " + rate.ToString("F1");

        if (rate <= minRate)
        {
            if (costText != null) costText.text = "MAX LEVEL";
            if (upgradeButton != null) upgradeButton.interactable = false;
        }
        else
        {
            if (costText != null) costText.text = "Upgrade (" + baseCost + ")";
            if (upgradeButton != null) upgradeButton.interactable = (CoinManager.instance.totalCoins >= baseCost);
        }
    }

    public void Upgrade()
    {
        if (CoinManager.instance.totalCoins < baseCost) return;

        CoinManager.instance.totalCoins -= baseCost;
        CoinManager.instance.SaveCoins();

        rate -= decreaseAmount;
        rate = Mathf.Max(rate, minRate);

        PlayerPrefs.SetFloat("SavedOxygenRate", rate);
        PlayerPrefs.Save();

        UpdateUI();
    }
}
