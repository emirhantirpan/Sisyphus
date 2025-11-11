using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static bool isGameOver = false;

    public Transform player;
    public Transform cameraTransform;
    private Rigidbody playerRb;

    public float gracePeriod = 0.1f;
    public float raycastDistance = 25f;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highscoreText;
    public float scoreMultiplier = 1.5f;

    private float timeSpentUnseen = 0.0f;
    private float score = 0.0f;
    private float highscore = 0.0f;

    public TextMeshProUGUI coinText;
    private int sessionCoins = 0;
    private int totalCoins = 0;

    public GameObject gameOverShopPanel;
    public TextMeshProUGUI panel_TotalCoinText;
    public TextMeshProUGUI panel_CurrentOxygenLevelText;
    public TextMeshProUGUI panel_UpgradeCostText;
    public Button upgradeButton;

    public int baseUpgradeCost = 10;
    public float oxygenDecreaseAmount = 0.1f;
    public float minOxygenRate = 0.2f;
    public float baseOxygenRate = 1.5f;
    private float currentOxygenRate;

    
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        isGameOver = false;
        Time.timeScale = 1;
        timeSpentUnseen = 0.0f;
        score = 0.0f;
        sessionCoins = 0;
        
        highscore = PlayerPrefs.GetFloat("Highscore", 0);
        totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);
        currentOxygenRate = PlayerPrefs.GetFloat("SavedOxygenRate", baseOxygenRate);

        cameraTransform = Camera.main.transform;

        if (PlayerController.instance != null )
        {
            player = PlayerController.instance.transform;
            playerRb = PlayerController.instance.rb;
        }
               
        if(highscoreText != null )
        {
            highscoreText.text = "Highscore: " + highscore.ToString("F0");
        }                
        UpdateCoinText();

        if (gameOverShopPanel != null)
        {
            gameOverShopPanel.SetActive(false);
        }
    }

    private void Update()
    {
        
        if (isGameOver || player == null || cameraTransform == null)
        {
            return;
        }

        if (playerRb == null)
        {
            if (PlayerController.instance != null && PlayerController.instance.rb != null)
            {
                playerRb = PlayerController.instance.rb;
            }
            else
            {
                return; 
            }
        }
        
        CheckIfPlayerIsVisible();
            
        if (timeSpentUnseen == 0.0f)
        {         
            float newScore = player.position.z * scoreMultiplier;
           
            if (newScore > score)
            {
                score = newScore;
            }
        }
       
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString("F0");
        }
    }

    private void CheckIfPlayerIsVisible()
    {
        Vector3 targetPos = player.position + (Vector3.up * 0.5f);
        Vector3 direction = (targetPos- cameraTransform.position).normalized;

        RaycastHit hit;

        if (Physics.Raycast(cameraTransform.position, direction, out hit, raycastDistance))
        {
            if (hit.collider.CompareTag("Player"))
            {
                timeSpentUnseen = 0.0f;
            }
            else
            {
                timeSpentUnseen += Time.deltaTime;
            }
        }
        else
        {
            timeSpentUnseen += Time.deltaTime;
        }
        if(timeSpentUnseen > gracePeriod)
        {
            EndGame();
        }
    }

    public void EndGame()
    {
        if (isGameOver) return;
        isGameOver = true;
        Debug.Log("biti");
        

        if(score > highscore)
        {
            highscore = score;
            PlayerPrefs.SetFloat("Highscore", highscore);
            PlayerPrefs.Save();
        }
        totalCoins += sessionCoins;
        PlayerPrefs.SetInt("TotalCoins", totalCoins);
        PlayerPrefs.Save();

        if (highscoreText != null)
        {
            highscoreText.text = "Highscore: " + highscore.ToString("F0");
        }
        if(scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString("F0");
        }
        
        if(gameOverShopPanel !=null)
        {
            gameOverShopPanel.SetActive(true);
            UpdateShopUI();
        }

    }
    public void AddCoin(int amount)
    {
        if(isGameOver) return;

        sessionCoins += amount;

        UpdateCoinText();
    }

    private void UpdateCoinText()
    {
        if (coinText != null)
        {
            coinText.text = sessionCoins.ToString();
        }
    }
    private void UpdateShopUI()
    {
        panel_TotalCoinText.text = "Toplam Coin: " + totalCoins.ToString();
        panel_CurrentOxygenLevelText.text = "Oxygen Speed: " + currentOxygenRate.ToString("F1");

        int currentUpgradeCost = baseUpgradeCost;

        if (currentUpgradeCost <= minOxygenRate)
        {
            panel_UpgradeCostText.text = "MAX LEVEL";
            upgradeButton.interactable = false;
        }
        else
        {
            panel_UpgradeCostText.text = "Upgrade (" + currentUpgradeCost + "Para)";
            upgradeButton.interactable = (totalCoins >= currentUpgradeCost);
        }
    }

    public void BuyOxygenUpgrade()
    {
        int currentUpgradeCost = baseUpgradeCost;

        if (totalCoins >= currentUpgradeCost && currentOxygenRate > minOxygenRate)
        {
            totalCoins -= currentUpgradeCost;
            currentOxygenRate -= oxygenDecreaseAmount;
            currentOxygenRate = Mathf.Max(currentOxygenRate, minOxygenRate);

            PlayerPrefs.SetInt("TotalCoins", totalCoins);
            PlayerPrefs.SetFloat("SavedOxygenRate", currentOxygenRate);
            PlayerPrefs.Save();

            UpdateShopUI();
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
