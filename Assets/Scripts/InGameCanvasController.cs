using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InGameCanvasController : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button restartButton;

    [Header("Texts")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private TMP_Text highscoreText;

    [Header("GameOver Panel")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text totalCoinsText;
    [SerializeField] private TMP_Text currentOxygenLevelText;

    private bool gameOverShown;

    private void Start()
    {
        BindButtons();
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    private void Update()
    {
        UpdateLiveUI();
        CheckGameOverState();
    }

    private void UpdateLiveUI()
    {
        if (ScoreManager.instance != null && scoreText != null)
            scoreText.text = ScoreManager.instance.GetScore().ToString("F0");

        if (CoinManager.instance != null && coinText != null)
            coinText.text = CoinManager.instance.GetSessionCoins().ToString();

        if (ScoreManager.instance != null && highscoreText != null)
            highscoreText.text = ScoreManager.instance.GetHighscore().ToString("F0");
    }

    private void CheckGameOverState()
    {
        if (GameStateManager.instance == null) return;

        if (GameStateManager.instance.isGameOver && !gameOverShown)
            ShowGameOver();
    }

    private void ShowGameOver()
    {
        gameOverShown = true;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        if (CoinManager.instance != null && totalCoinsText != null)
            totalCoinsText.text = CoinManager.instance.GetSessionCoins().ToString();

        if (OxygenSlider.instance != null && currentOxygenLevelText != null)
            currentOxygenLevelText.text =
                OxygenSlider.instance.GetStamina().ToString("F0");
    }

    private void BindButtons()
    {
        pauseButton?.onClick.AddListener(() => PauseMenu.Instance?.PauseGame());
        restartButton?.onClick.AddListener(() => GameStateManager.instance?.RestartGame());
    }
}

