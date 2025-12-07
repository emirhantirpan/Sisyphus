using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager instance;

    public bool isGameOver { get; private set; } = false;

    private void Awake()
    {
        InitializeSingleton();
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

    public void EndGame()
    {
        if (isGameOver) return;

        isGameOver = true;
        SaveGameData();
        ShowGameOverUI();
        PauseGame();

        Debug.Log("Game Over!");
    }

    private void SaveGameData()
    {
        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.SaveHighscore();
        }

        if (CoinManager.instance != null)
        {
            CoinManager.instance.SaveCoins();
        }
    }

    private void ShowGameOverUI()
    {
        if (ShopManager.instance != null)
        {
            ShopManager.instance.ShowShopPanel();
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        ResumeGame();
        ResetGameState();
        ReloadScene();
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f;
        isGameOver = false;
    }

    private void ResetGameState()
    {
        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.ResetScore();
        }

        if (CoinManager.instance != null)
        {
            CoinManager.instance.ResetSessionCoins();
        }
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}