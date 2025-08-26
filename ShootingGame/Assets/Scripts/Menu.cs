using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject mainMenuHolder;
    public GameObject optionsMenuHolder;

    public Slider[] volumeSliders;
    public Toggle[] resolutionToggles;
    public Toggle fullscreenToggle;
    public int[] screenWidths;
    int activeScreenResIndex;

    [Header("BGM")]
    [SerializeField] private AudioClip menuBgm; // 메뉴 배경음악

    private void Start()
    {
        activeScreenResIndex = PlayerPrefs.GetInt("screen res index");//Session에 저장한 값 불러오기
        bool isFullscreen = (PlayerPrefs.GetInt("fullscreen") == 1) ? true : false;

        if (menuBgm != null)
        {
            AudioManager.instance.PlayMusic(menuBgm);
        }
        //volumeSliders[0].value = AudioManager.instance.masterVolumePercent;
        //volumeSliders[1].value = AudioManager.instance.musicVolumePercent;
        //volumeSliders[2].value = AudioManager.instance.sfxVolumePercent;

        volumeSliders[0].SetValueWithoutNotify(AudioManager.instance.masterVolumePercent);
        volumeSliders[1].SetValueWithoutNotify(AudioManager.instance.musicVolumePercent);
        volumeSliders[2].SetValueWithoutNotify(AudioManager.instance.sfxVolumePercent);


        for (int i = 0; i < resolutionToggles.Length; i++)
        {
            resolutionToggles[i].isOn = i == activeScreenResIndex;//==먼저 수행, 그 다음에 = 대입
        }

        fullscreenToggle.isOn = isFullscreen;
    }
    public void Play()
    {
        SceneManager.LoadScene("Game");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void OptionsMenu()
    {
        mainMenuHolder.SetActive(false);
        optionsMenuHolder.SetActive(true);
    }

    public void MainMenu()
    {
        mainMenuHolder.SetActive(true);
        optionsMenuHolder.SetActive(false);
    }

    public void SetScreenResolution(int i)
    {
        if (resolutionToggles[i].isOn)
        {
            activeScreenResIndex = i;
            float aspectRatio = 16 / 9f;
            Screen.SetResolution(screenWidths[i], (int)(screenWidths[i] / aspectRatio), false);
            PlayerPrefs.SetInt("screen res index", activeScreenResIndex);
        }
    }

    public void SetFullScreen(bool isFullscreen)
    {
        for (int i = 0; i < resolutionToggles.Length; i++)
        {
            resolutionToggles[i].interactable = !isFullscreen;
        }
        if (isFullscreen)
        {
            Resolution[] allResolutions = Screen.resolutions;
            Resolution maxResolution = allResolutions[allResolutions.Length - 1];
            Screen.SetResolution(maxResolution.width, maxResolution.height, true);
        }
        else
        {
            SetScreenResolution(activeScreenResIndex);
        }
        PlayerPrefs.SetInt("fullscreen", ((isFullscreen) ? 1 : 0));
        PlayerPrefs.Save();

    }
    public void SetMasterVolume(float value)
    {
        if (value <= 0.001f) return; //클릭만으로 0 찍힌 이벤트 무시

        AudioManager.instance.SetVolume(value, AudioManager.AudioChannel.Master);
    }
    public void SetMusicVolume(float value)
    {
        if (value <= 0.001f) return; // 🔥 클릭만으로 0 찍힌 이벤트 무시

        AudioManager.instance.SetVolume(value, AudioManager.AudioChannel.Music);

    }
    public void SetSfxVolume(float value)
    {
        if (value <= 0.001f) return; // 🔥 클릭만으로 0 찍힌 이벤트 무시

        AudioManager.instance.SetVolume(value, AudioManager.AudioChannel.Sfx);

    }
}
