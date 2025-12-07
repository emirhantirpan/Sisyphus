using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [Header("Panel Reference")]
    public GameObject pausePanel;

    [Header("Button References")]
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button homeButton;
    [SerializeField] private Button volumeButton;
    [SerializeField] private Button musicButton;

    private bool isVolumePlaying = true;
    private bool isMusicPlaying = true;

    private const string VOLUME_PREF_KEY = "VolumeOn";
    private const string MUSIC_PREF_KEY = "MusicOn";
    private const string MAIN_MENU_SCENE = "MainMenuScene";

    private void Awake()
    {
        InitializePanel();
        RegisterButtonListeners();
    }

    private void Start()
    {
        LoadAudioSettings();
    }

    private void InitializePanel()
    {
        if (PanelController.instance != null && pausePanel != null)
        {
            PanelController.instance.ClosePanel(pausePanel);
        }
    }

    private void RegisterButtonListeners()
    {
        if (resumeButton != null)
            resumeButton.onClick.AddListener(OnResumeButtonClicked);

        if (homeButton != null)
            homeButton.onClick.AddListener(OnHomeButtonClicked);

        if (volumeButton != null)
            volumeButton.onClick.AddListener(OnVolumeButtonClicked);

        if (musicButton != null)
            musicButton.onClick.AddListener(OnMusicButtonClicked);
    }

    private void OnResumeButtonClicked()
    {
        if (PanelController.instance != null && pausePanel != null)
        {
            PanelController.instance.ClosePanel(pausePanel);
            Time.timeScale = 1f;
        }
    }

    private void OnHomeButtonClicked()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(MAIN_MENU_SCENE);
    }

    private void OnVolumeButtonClicked()
    {
        isVolumePlaying = !isVolumePlaying;
        SetButtonAlpha(volumeButton, isVolumePlaying);
        SaveAudioSettings();

        // SFX kontrol kodu buraya eklenebilir
        // if (SFXPlayer.instance != null)
        //     SFXPlayer.instance.audioSourceSFX.mute = !isVolumePlaying;
    }

    private void OnMusicButtonClicked()
    {
        isMusicPlaying = !isMusicPlaying;
        SetButtonAlpha(musicButton, isMusicPlaying);
        SaveAudioSettings();

        // Müzik kontrol kodu buraya eklenebilir
        // if (SFXPlayer.instance != null)
        // {
        //     if (isMusicPlaying)
        //         PlayMusic(SFXPlayer.instance.backgroundMusic, true);
        //     else
        //         SFXPlayer.instance.audioSourceMusic.Stop();
        // }
    }

    private void SetButtonAlpha(Button button, bool isActive)
    {
        if (button == null) return;

        Color color = button.image.color;
        color.a = isActive ? 1.0f : 136f / 255f;
        button.image.color = color;
    }

    private void LoadAudioSettings()
    {
        isVolumePlaying = PlayerPrefs.GetInt(VOLUME_PREF_KEY, 1) == 1;
        isMusicPlaying = PlayerPrefs.GetInt(MUSIC_PREF_KEY, 1) == 1;

        SetButtonAlpha(volumeButton, isVolumePlaying);
        SetButtonAlpha(musicButton, isMusicPlaying);
    }

    private void SaveAudioSettings()
    {
        PlayerPrefs.SetInt(VOLUME_PREF_KEY, isVolumePlaying ? 1 : 0);
        PlayerPrefs.SetInt(MUSIC_PREF_KEY, isMusicPlaying ? 1 : 0);
        PlayerPrefs.Save();
    }
}