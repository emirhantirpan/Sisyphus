using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameCanvasController : MonoBehaviour
{
    [Header("Menu References")]
    [SerializeField] private PauseMenu pauseMenu;

    [Header("Button References")]
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button restartGameButton;

    private void OnEnable()
    {
        RegisterButtonListeners();
    }

    private void OnDisable()
    {
        UnregisterButtonListeners();
    }

    private void RegisterButtonListeners()
    {
        if (pauseButton != null)
            pauseButton.onClick.AddListener(OnPauseButtonClicked);

        if (restartGameButton != null)
            restartGameButton.onClick.AddListener(OnRestartButtonClicked);
    }

    private void UnregisterButtonListeners()
    {
        if (pauseButton != null)
            pauseButton.onClick.RemoveListener(OnPauseButtonClicked);

        if (restartGameButton != null)
            restartGameButton.onClick.RemoveListener(OnRestartButtonClicked);
    }

    private void OnPauseButtonClicked()
    {
        if (pauseMenu != null && pauseMenu.pausePanel != null)
        {
            pauseMenu.pausePanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    private void OnRestartButtonClicked()
    {
        if (GameStateManager.instance != null)
        {
            GameStateManager.instance.RestartGame();
        }
    }
}