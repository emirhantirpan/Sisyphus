using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;

    [SerializeField] private Button _playButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _quitButton;
    [SerializeField] private Button _volumeButton;
    [SerializeField] private Button _musicButton;
    [SerializeField] private Button _backButton;

    private bool _isVolumePlaying = true;
    private bool _isMusicPlaying = true;

    private const string VolumePrefKey = "VolumeOn";
    private const string MusicPrefKey = "MusicOn";

    private void Start()
    {
        PanelController.instance.OpenPanel(mainMenuPanel);
        PanelController.instance.ClosePanel(settingsPanel);

        _playButton.onClick.AddListener(PlayButton);
        _settingsButton.onClick.AddListener(SettingsButton);
        _quitButton.onClick.AddListener(QuitButton);
        _volumeButton.onClick.AddListener(VolumeButton);
        _musicButton.onClick.AddListener(MusicButton);
        _backButton.onClick.AddListener(BackButton);

        //LoadAudioSettings();
    }

    private void PlayButton()
    {
        //SFXPlayer.instance.audioSourceSFX.PlayOneShot(SFXPlayer.instance.buttonClick);
        SceneManager.LoadScene("Sample Scene");
    }

    private void SettingsButton()
    {
        //SFXPlayer.instance.audioSourceSFX.PlayOneShot(SFXPlayer.instance.buttonClick);
        PanelController.instance.ClosePanel(mainMenuPanel);
        PanelController.instance.OpenPanel(settingsPanel);
    }

    private void QuitButton()
    {
        //SFXPlayer.instance.audioSourceSFX.PlayOneShot(SFXPlayer.instance.buttonClick);
        Application.Quit();
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
        if (source.clip == clip && source.isPlaying) return;

        source.clip = clip;
        source.loop = loop;
        source.Play();
    }*/

    private void SetButtonAlpha(Button button, bool isActive)
    {
        Color color = button.image.color;
        color.a = isActive ? 1f : 136f / 255f;
        button.image.color = color;
    }

    private void BackButton()
    {
        PanelController.instance.ClosePanel(settingsPanel);
        PanelController.instance.OpenPanel(mainMenuPanel);
        //SFXPlayer.instance.audioSourceSFX.PlayOneShot(SFXPlayer.instance.buttonClick);
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