using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.Rendering;
using TMPro;
using Cinemachine;

public class Options : MonoBehaviour
{
    public static Options Instance;

    public GameObject ScreenObject;
    public GameObject AccessabilityObject;
    public GameObject SoundObject;

    public GameObject QuitButton;

    public CinemachineFreeLook PersonCam;
    internal float XAxisSpeed;
    internal float YAxisSpeed;

    public Toggle InvertedMouse;
    public Slider MouseSensSlider;

    public Toggle VsyncToggle;

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
    CursorLockMode CursorMode;

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

            if (!options.Contains(option))
            {
                options.Add(option);
            }

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

        if (MainMenu)
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

        if (PersonCam)
        {
            YAxisSpeed = PersonCam.m_YAxis.m_MaxSpeed;
            XAxisSpeed = PersonCam.m_XAxis.m_MaxSpeed;
        }

        if(MainMenu)
        {
            GameManager.Instance.MouseSensitivity = MouseSensSlider.value;
            MouseSensitivity(GameManager.Instance.MouseSensitivity);
        }
        else
        {
            MouseSensitivity(GameManager.Instance.MouseSensitivity);
            MouseSensSlider.value = GameManager.Instance.MouseSensitivity;
        }

        if(MainMenu)
        {
            GameManager.Instance.MouseControlsInvert = InvertedMouse.isOn;
            InvertMouseControl(GameManager.Instance.MouseControlsInvert);
        }
        else
        {
            InvertMouseControl(GameManager.Instance.MouseControlsInvert);
            InvertedMouse.isOn = GameManager.Instance.MouseControlsInvert;
        }

        if(QualitySettings.vSyncCount == 0)
        {
            VsyncToggle.isOn = false;
        }
        else
        {
            VsyncToggle.isOn = true;
        }
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
            Cursor.lockState = CursorMode;
            OptionsMenuUI.GetComponent<UIFade>().ToFadeOut();
        }
        else
        {
            TimeScale = Time.timeScale;
            Time.timeScale = 0;
            CursorMode = Cursor.lockState;
            Cursor.lockState = CursorLockMode.Confined;

            if (GameManager.Instance.StartedGame)
            {
                QuitButton.SetActive(true);
            }
            else
            {
                QuitButton.SetActive(false);
            }

            OptionsMenuUI.SetActive(true);
            OptionsMenuUI.GetComponent<UIFade>().ToFadeIn();
        }
    }

    public void QualitySetting(int index)
    {
        QualitySettings.SetQualityLevel(index);
    }

    public void Vsync(bool vsync)
    {
        if (vsync)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }
    }

    public void AccessabilityMenu()
    {
        SoundObject.SetActive(false);
        ScreenObject.SetActive(false);
        AccessabilityObject.SetActive(true);
    }

    public void SoundMenu()
    {
        SoundObject.SetActive(true);
        ScreenObject.SetActive(false);
        AccessabilityObject.SetActive(false);
    }

    public void ScreenMenu()
    {
        SoundObject.SetActive(false);
        ScreenObject.SetActive(true);
        AccessabilityObject.SetActive(false);
    }

    public void InvertMouseControl(bool Invert)
    {
        GameManager.Instance.MouseControlsInvert = Invert;

        if (PersonCam)
        {
            PersonCam.m_YAxis.m_InvertInput = Invert;
            PersonCam.m_XAxis.m_InvertInput = Invert;
        }
    }

    public void KeepTurnUIOpen(bool UIToggle)
    {
        GameManager.Instance.KeepTurnUIUp = UIToggle;
    }

    public void MouseSensitivity(float NewSense)
    {
        GameManager.Instance.MouseSensitivity = NewSense;

        if (PersonCam)
        {
            PersonCam.m_YAxis.m_MaxSpeed = NewSense * YAxisSpeed;
            PersonCam.m_XAxis.m_MaxSpeed = NewSense * XAxisSpeed;
        }
    }

    public void QuestMarkersActive(bool ObjectiveToggle)
    {

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
