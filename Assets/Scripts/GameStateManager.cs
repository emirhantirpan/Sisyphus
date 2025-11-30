using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager instance;

    public bool isGameOver = false;

    private void Awake()
    {
        if (instance == null) instance = this;
        else { Destroy(gameObject); return; }
    }

    public void EndGame()
    {
        if (isGameOver) return;

        isGameOver = true;

        if (ScoreManager.instance != null) ScoreManager.instance.SaveHighscore();
        if (CoinManager.instance != null) CoinManager.instance.SaveCoins();
        if (ShopManager.instance != null) ShopManager.instance.ShowShopPanel();

        Time.timeScale = 0f; 
        Debug.Log("Game Over!");
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        isGameOver = false;

        if (ScoreManager.instance != null) ScoreManager.instance.ResetScore();
        if (CoinManager.instance != null) CoinManager.instance.ResetSessionCoins();

        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }
}
