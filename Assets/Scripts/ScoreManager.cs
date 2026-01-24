using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highscoreText;

    [Header("Score Settings")]
    [SerializeField] private float baseScoreMultiplier = 1.5f;

    // Shop upgrade çarpanı: 1.0, 1.2, 1.4 ...
    private float upgradeMultiplier = 1f;

    private float score = 0f;
    private float highscore = 0f;
    private Transform player;

    private const string HIGHSCORE_KEY = "Highscore";

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        highscore = PlayerPrefs.GetFloat(HIGHSCORE_KEY, 0f);
        UpdateHighscoreText();
        UpdateScoreText();
    }

    private void Update()
    {
        if (IsGameOver()) return;

        if (player == null && PlayerController.instance != null)
            player = PlayerController.instance.transform;

        if (player == null) return;

        float newScore = player.position.z * baseScoreMultiplier * upgradeMultiplier;

        if (newScore > score)
        {
            score = newScore;
            UpdateScoreText();
        }
    }

    // GameplayItemApplier burayı çağıracak: 1.0 + 0.2*n
    public void SetUpgradeMultiplier(float value)
    {
        upgradeMultiplier = Mathf.Max(0f, value);
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score.ToString("F0");
    }

    private void UpdateHighscoreText()
    {
        if (highscoreText != null)
            highscoreText.text = "Highscore: " + highscore.ToString("F0");
    }

    public void ResetScore()
    {
        score = 0f;
        UpdateScoreText();
    }

    public void SaveHighscore()
    {
        if (score > highscore)
        {
            highscore = score;
            PlayerPrefs.SetFloat(HIGHSCORE_KEY, highscore);
            PlayerPrefs.Save();
            UpdateHighscoreText();
        }
    }

    private bool IsGameOver()
    {
        return GameStateManager.instance != null && GameStateManager.instance.isGameOver;
    }

    public float GetScore() => score;
    public float GetHighscore() => highscore;
}
