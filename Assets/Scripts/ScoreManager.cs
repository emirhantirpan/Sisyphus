using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highscoreText;

    [Header("Score Settings")]
    [SerializeField] private float scoreMultiplier = 1.5f;

    private float score = 0;
    private float highscore = 0;
    private Transform player;

    private const string HIGHSCORE_KEY = "Highscore";

    private void Awake()
    {
        InitializeSingleton();
    }

    private void Start()
    {
        LoadHighscore();
        UpdateHighscoreText();
    }

    private void Update()
    {
        if (IsGameOver()) return;

        CachePlayerReference();
        UpdateScore();
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

    private void LoadHighscore()
    {
        highscore = PlayerPrefs.GetFloat(HIGHSCORE_KEY, 0);
    }

    private void CachePlayerReference()
    {
        if (player == null && PlayerController.instance != null)
        {
            player = PlayerController.instance.transform;
        }
    }

    private void UpdateScore()
    {
        if (player == null) return;

        float newScore = player.position.z * scoreMultiplier;
        if (newScore > score)
        {
            score = newScore;
            UpdateScoreText();
        }
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString("F0");
        }
    }

    private void UpdateHighscoreText()
    {
        if (highscoreText != null)
        {
            highscoreText.text = "Highscore: " + highscore.ToString("F0");
        }
    }

    public void ResetScore()
    {
        score = 0;
        UpdateScoreText();
    }

    public void SaveHighscore()
    {
        if (score > highscore)
        {
            highscore = score;
            PlayerPrefs.SetFloat(HIGHSCORE_KEY, score);
            PlayerPrefs.Save();
            UpdateHighscoreText();
        }
    }

    private bool IsGameOver()
    {
        return GameStateManager.instance != null && GameStateManager.instance.isGameOver;
    }

    // Getters
    public float GetScore() => score;
    public float GetHighscore() => highscore;
}