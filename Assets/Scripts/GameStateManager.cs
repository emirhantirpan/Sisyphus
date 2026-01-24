using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager instance;

    public bool isGameOver { get; private set; } = false;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void EndGame()
    {
        if (isGameOver) return;

        isGameOver = true;

        // Kaydet
        ScoreManager.instance?.SaveHighscore();
        CoinManager.instance?.SaveLastRunCoins();


        // GameOver UI (ShopManager projende varsa)
        TryShowGameOverUI();

        // Pause
        Time.timeScale = 0f;

        Debug.Log("Game Over!");
    }

    private void TryShowGameOverUI()
    {
        // ShopManager sınıfı projende yoksa compile bile etmez.
        // O yüzden bu fonksiyonu SENİN PROJENDE ShopManager varsa açacağız.
        // Şimdilik boş bırakıyorum ki hata vermesin.
        //
        // Eğer ShopManager sende VARSA: aşağıdaki satırları geri koy:
        // if (ShopManager.instance != null) ShopManager.instance.ShowShopPanel();
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        isGameOver = false;

        ScoreManager.instance?.ResetScore();
        CoinManager.instance?.ResetSessionCoins();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}