using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameCanvasController : MonoBehaviour
{
    [SerializeField] private PauseMenu _pauseMenu;

    [SerializeField] private Button _pauseButton;
    [SerializeField] private Button _restartGameButton;

    private void Start()
    {
    }
    private void OnEnable()
    {
        _pauseButton.onClick.AddListener(PauseButton);
        _restartGameButton.onClick.AddListener(RestartGameButton);
    }
    private void OnDisable()
    {
        _pauseButton.onClick.RemoveListener(PauseButton);
        _restartGameButton.onClick.RemoveListener(RestartGameButton);
    }

    public void PauseButton()
    {
        _pauseMenu.pausePanel.SetActive(true);
        Time.timeScale = 0f;
    }
    public void RestartGameButton()
    {
        GameStateManager.instance.isGameOver = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
