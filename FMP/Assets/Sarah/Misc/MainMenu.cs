using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MainMenu : MonoBehaviour
{
    public GameObject MainMenuCanvas;
    public GameObject OptionsMenu;
    float DollyCartSpeed;
    public CinemachineDollyCart DollyCart;
    internal float PreviousPos;
    public GameObject MainCamera;
    public Options options;
    public CinemachineVirtualCamera VirtualCamera;
    public GameObject CompassObj;

    internal SetCameraLocation[] CameraLocations;

    [Header("Sound Effects")]
    public AudioClip ButtonPress;

    void Start()
    {
        options = GetComponent<Options>();

        CameraLocations = FindObjectsOfType<SetCameraLocation>();

        if (GameManager.Instance)
        {
            if (GameManager.Instance.StartedGame)
            {
                MainMenuCanvas.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                ControlToPlayer();
                return;
            }
        }

        DollyCartSpeed = DollyCart.m_Speed;
        DollyCart.m_Speed = 0;
        Cursor.lockState = CursorLockMode.Confined;
        FindObjectOfType<PlayerOverworld>().CanMove = false;
    }

    private void Update()
    {
        if(DollyCart.m_Position != 0)
        {
            if(DollyCart.m_Position <= PreviousPos)
            {
                ControlToPlayer();
            }

            PreviousPos = DollyCart.m_Position;
        }

        if(GameManager.Instance)
        {
            if(GameManager.Instance.StartedGame)
            {
                MainMenuCanvas.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                ControlToPlayer();
            }
        }
    }

    internal void ControlToPlayer()
    {
        VirtualCamera.gameObject.SetActive(false);

        bool CamReturn = true;

        foreach(SetCameraLocation CamLoc in CameraLocations)
        {
            if(GameManager.Instance.DialogueToPlay == CamLoc.ActiveOnReturn)
            {
                CamReturn = false;
                break;
            }
        }

        if (GameManager.Instance.DialogueToPlay == PlayAfter.None || CamReturn)
        {
            MainCamera.SetActive(true);

        }
        options.InGame = true;
        GameManager.Instance.StartedGame = true;
        FindObjectOfType<PlayerOverworld>().CanMove = true;
        CompassObj.SetActive(true);
        CompassObj.GetComponent<UIFade>().ToFadeIn();
        Destroy(this);
    }

    public void PlayGame()
    {
        SoundManager.Instance.PlaySFX(ButtonPress);
        Cursor.lockState = CursorLockMode.Locked;
        DollyCart.m_Speed = DollyCartSpeed;
        VirtualCamera.LookAt = FindObjectOfType<PlayerOverworld>().transform;
        MainMenuCanvas.SetActive(false);
    }

    public void Option()
    {
        SoundManager.Instance.PlaySFX(ButtonPress);
        if (!GameManager.Instance.StartedGame)
        {
            Options.Instance.QuitButton.SetActive(false);
        }
        else
        {
            Options.Instance.QuitButton.SetActive(true);
        }

        OptionsMenu.SetActive(true);
    }

    public void CloseOptions()
    {
        SoundManager.Instance.PlaySFX(ButtonPress);
        OptionsMenu.SetActive(false);
    }

    public void Quit()
    {
        SoundManager.Instance.PlaySFX(ButtonPress);
        Application.Quit();
    }
}
