using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject levelSelectionPanel;
    public GameObject settingsPanel;

    // 🔊 Audio Sources
    public AudioSource bgMusicSource;
    public AudioSource sfxSource;

    // 🎵 Audio Clips
    public AudioClip menuMusic;
    public AudioClip buttonClickSound;

    // 🎚️ Volume Slider
    public Slider volumeSlider;

    void Start()
    {
        menuPanel.SetActive(true);
        levelSelectionPanel.SetActive(false);
        settingsPanel.SetActive(false);

        // ▶️ Play Menu Music (ONLY THIS SCENE)
        if (bgMusicSource != null && menuMusic != null)
        {
            bgMusicSource.clip = menuMusic;
            bgMusicSource.loop = true;
            bgMusicSource.Play();
        }

        // 🎚️ Load Volume
        float savedVolume = PlayerPrefs.GetFloat("GameVolume", 1f);
        bgMusicSource.volume = savedVolume;
        sfxSource.volume = savedVolume;
        volumeSlider.value = savedVolume;

        // 🎚️ Listen to Slider
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    public void StartGame()
    {
        PlayClickSound();

        menuPanel.SetActive(false);
        levelSelectionPanel.SetActive(true);
    }

    public void BackToMenu()
    {
        PlayClickSound();

        menuPanel.SetActive(true);
        levelSelectionPanel.SetActive(false);
    }

    public void OpenSettings()
    {
        PlayClickSound();
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        PlayClickSound();
        settingsPanel.SetActive(false);
    }

    public void ExitGame()
    {
        PlayClickSound();
        Application.Quit();
    }

    // 🔘 INTERNAL SOUND (used by code buttons)
    void PlayClickSound()
    {
        if (sfxSource != null && buttonClickSound != null)
        {
            sfxSource.PlayOneShot(buttonClickSound);
        }
    }

    // 🎯 PUBLIC FUNCTION (FOR UNITY BUTTON ONCLICK)
    public void PlayButtonClick()
    {
        if (sfxSource != null && buttonClickSound != null)
        {
            sfxSource.PlayOneShot(buttonClickSound);
        }
    }

    // 🎚️ Volume Control
    public void SetVolume(float volume)
    {
        bgMusicSource.volume = volume;
        sfxSource.volume = volume;

        PlayerPrefs.SetFloat("GameVolume", volume);
    }
}