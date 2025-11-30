using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highscoreText;

    public float scoreMultiplier = 1.5f;
    private float score = 0;
    private float highscore = 0;

    private Transform player;

    private void Awake()
    {
        if (instance == null) instance = this;
        else { Destroy(gameObject); return; }
    }

    private void Start()
    {
        highscore = PlayerPrefs.GetFloat("Highscore", 0);
        if (highscoreText != null) highscoreText.text = "Highscore: " + highscore.ToString("F0");
    }

    private void Update()
    {
        if (GameStateManager.instance != null && GameStateManager.instance.isGameOver) return;

        if (player == null && PlayerController.instance != null)
            player = PlayerController.instance.transform;

        if (player != null)
        {
            float newScore = player.position.z * scoreMultiplier;
            if (newScore > score) score = newScore;

            if (scoreText != null) scoreText.text = "Score: " + score.ToString("F0");
        }
    }

    public void ResetScore()
    {
        score = 0;
        if (scoreText != null) scoreText.text = "Score: 0";
    }

    public void SaveHighscore()
    {
        if (score > highscore)
        {
            PlayerPrefs.SetFloat("Highscore", score);
            PlayerPrefs.Save();
        }
    }
}
