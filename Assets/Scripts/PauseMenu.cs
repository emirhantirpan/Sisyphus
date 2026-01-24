using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance;

    [Header("Pause UI")]
    [SerializeField] private GameObject pauseCanvas;
    [SerializeField] private GameObject pausePanel;
    
    [Header("Pause Panel Button")]
    [SerializeField] private Button _resumeButton;

    [SerializeField] private Button _homeBUton;

    private bool isPaused = false;

    private void OnEnable()
    {
        _resumeButton.onClick.AddListener(() => ResumeGame());
        _homeBUton.onClick.AddListener(() => HomeButton());
    }

    private void OnDisable()
    {
        _resumeButton.onClick.RemoveAllListeners();
        _homeBUton.onClick.RemoveAllListeners();
    }

    private void HomeButton()
    {
        SceneManager.LoadScene("MainMneuScene");
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        if (pauseCanvas != null)
            pauseCanvas.SetActive(true);
    }

    public void PauseGame()
    {
        if (isPaused) return;

        isPaused = true;
        Time.timeScale = 0f;

        if (pauseCanvas != null)
            pauseCanvas.SetActive(true);

        if (pausePanel != null)
            pausePanel.SetActive(true);
    }

    public void ResumeGame()
    {
        if (!isPaused) return;

        isPaused = false;
        Time.timeScale = 1f;

        if (pausePanel != null)
            pausePanel.SetActive(false);
    }

    public void TogglePause()
    {
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }
}