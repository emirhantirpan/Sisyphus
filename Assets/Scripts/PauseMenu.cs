using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance;

    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private Button _pauseButton;
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _homeButton;
    [SerializeField] private Button _volumeButton;
    [SerializeField] private Button _musicButton;

    private bool _isVolumePlaying;
    private bool _isMusicPlaying;

    private const string VolumePrefKey = "VolumeOn";
    private const string MusicPrefKey = "MusicOn";

    private void Awake()
    {
        instance = this;

        PanelController.instance.ClosePanel(_pausePanel);

        _pauseButton.onClick.AddListener(PauseButton);
        _resumeButton.onClick.AddListener(ResumeButton);
        _homeButton.onClick.AddListener(HomeButton);
        _volumeButton.onClick.AddListener(VolumeButton);
        _musicButton.onClick.AddListener(MusicButton);
    }

    private void Start()
    {
        //LoadAudioSettings();
    }

    public void PauseButton()
    {
       
            _pausePanel.SetActive(true);
            Time.timeScale = 0f;
        
        
    }

    public void ResumeButton()
    {
        PanelController.instance.ClosePanel(_pausePanel);
        Time.timeScale = 1f;
        //SFXPlayer.instance.audioSourceSFX.PlayOneShot(SFXPlayer.instance.buttonClick);
    }

    public void HomeButton()
    {
        //SFXPlayer.instance.audioSourceSFX.PlayOneShot(SFXPlayer.instance.buttonClick);
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    private void VolumeButton()
    {
        _isVolumePlaying = !_isVolumePlaying;

        //SFXPlayer.instance.audioSourceSFX.mute = !_isVolumePlaying;

        SetButtonAlpha(_volumeButton, _isVolumePlaying);
        //SaveAudioSettings();
    }

    private void MusicButton()
    {
        _isMusicPlaying = !_isMusicPlaying;

        if (_isMusicPlaying)
        {
            //PlayMusic(SFXPlayer.instance.backgroundMusic, true);
        }
        else
        {
            //SFXPlayer.instance.audioSourceMusic.Stop();
        }

        SetButtonAlpha(_musicButton, _isMusicPlaying);
        //SaveAudioSettings();
    }

    /*private void PlayMusic(AudioClip clip, bool loop)
    {
        var source = SFXPlayer.instance.audioSourceMusic;

        if (source.clip == clip && source.isPlaying)
            return;

        source.clip = clip;
        source.loop = loop;
        source.Play();
    }*/

    private void SetButtonAlpha(Button button, bool isActive)
    {
        Color color = button.image.color;
        color.a = isActive ? 1.0f : 136f / 255f;
        button.image.color = color;
    }

    /*private void LoadAudioSettings()
    {
        _isVolumePlaying = PlayerPrefs.GetInt(VolumePrefKey, 1) == 1;
        _isMusicPlaying = PlayerPrefs.GetInt(MusicPrefKey, 1) == 1;

        SFXPlayer.instance.audioSourceSFX.mute = !_isVolumePlaying;

        if (_isMusicPlaying)
            PlayMusic(SFXPlayer.instance.backgroundMusic, true);

        SetButtonAlpha(_volumeButton, _isVolumePlaying);
        SetButtonAlpha(_musicButton, _isMusicPlaying);
    }*/

    /*private void SaveAudioSettings()
    {
        PlayerPrefs.SetInt(VolumePrefKey, _isVolumePlaying ? 1 : 0);
        PlayerPrefs.SetInt(MusicPrefKey, _isMusicPlaying ? 1 : 0);
        PlayerPrefs.Save();
    }*/
}