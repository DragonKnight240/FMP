using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.Rendering;
using TMPro;

public class Options : MonoBehaviour
{
    public static Options Instance;
    public AudioMixer audioMixer;
    Resolution[] resolutions;
    public TMP_Dropdown resolutionDropdown;
    int currentResolutionIndex;
    int oldResolutionIndex;
    public TextMeshProUGUI resolutionText;
    public Toggle fullscreenDropdown;
    public GameObject OptionsMenuUI;
    public bool InGame = true;
    public Slider MusicSlider;
    public Slider SFXSlider;
    public Slider AmbianceSlider;
    public Slider MasterSlider;
    public bool MainMenu = false;
    float TimeScale = 1;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;

            options.Add(option);

            if (resolutions[i].height == Screen.currentResolution.height && resolutions[i].width == Screen.currentResolution.width)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        if (Screen.fullScreen)
        {
            fullscreenDropdown.isOn = true;
        }
        else
        {
            fullscreenDropdown.isOn = false;
        }

        float Volume = 0;
        if (MainMenu)
        {
            audioMixer.GetFloat("Master", out Volume);
            GameManager.Instance.MasterSlider = Volume;

        }
        else
        {
            masterVolume(GameManager.Instance.MasterSlider);
            Volume = GameManager.Instance.MasterSlider;
        }

        MasterSlider.value = Volume;

        if (MainMenu)
        {
            audioMixer.GetFloat("SFX", out Volume);
            GameManager.Instance.SFXSlider = Volume;
        }
        else
        {
            SFXVolume(GameManager.Instance.SFXSlider);
            Volume = GameManager.Instance.SFXSlider;
        }

        SFXSlider.value = Volume;

        if (MainMenu)
        {
            audioMixer.GetFloat("Music", out Volume);
            GameManager.Instance.MusicSlider = Volume;
        }
        else
        {
            musicVolume(GameManager.Instance.MusicSlider);
            Volume = GameManager.Instance.MusicSlider;
        }

        MusicSlider.value = Volume;

        if (!MainMenu)
        {
            audioMixer.GetFloat("Ambiance", out Volume);
            GameManager.Instance.AmbianceSlider = Volume;
        }
        else
        {
            AmbianceVolume(GameManager.Instance.AmbianceSlider);
            Volume = GameManager.Instance.AmbianceSlider;
        }

        AmbianceSlider.value = Volume;
    }

    private void Update()
    {
        if (InGame)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                toggleOption();
            }
        }
    }

    public void toggleOption()
    {
        if (OptionsMenuUI.activeInHierarchy)
        {
            Time.timeScale = TimeScale;
            if (!GameManager.Instance.inCombat && InGame)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            OptionsMenuUI.SetActive(false);
        }
        else
        {
            TimeScale = Time.timeScale;
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.Confined;
            OptionsMenuUI.SetActive(true);
        }
    }

    public void masterVolume(float volume)
    {
        audioMixer.SetFloat("Master", volume);
        GameManager.Instance.MasterSlider = volume;
    }

    public void SFXVolume(float volume)
    {
        audioMixer.SetFloat("SFX", volume);
        GameManager.Instance.SFXSlider = volume;
    }

    public void musicVolume(float volume)
    {
        audioMixer.SetFloat("Music", volume);
        GameManager.Instance.MusicSlider = volume;
    }

    public void AmbianceVolume(float volume)
    {
        audioMixer.SetFloat("Ambiance", volume);
        GameManager.Instance.AmbianceSlider = volume;
    }

    public void resolution(int index)
    {
        Resolution resolution = resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

        oldResolutionIndex = currentResolutionIndex;
        currentResolutionIndex = index;

        resolutionDropdown.value = index;
        resolutionDropdown.RefreshShownValue();
    }

    public void fullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void Exit()
    {
        Application.Quit();
    }
}
