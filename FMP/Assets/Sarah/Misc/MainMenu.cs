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

    void Start()
    {
        options = GetComponent<Options>();
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
        MainCamera.SetActive(true);
        options.InGame = true;
        FindObjectOfType<PlayerOverworld>().CanMove = true;
        GameManager.Instance.StartedGame = true;
        CompassObj.SetActive(true);
        CompassObj.GetComponent<UIFade>().ToFadeIn();
        Destroy(this);
    }

    public void PlayGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        DollyCart.m_Speed = DollyCartSpeed;
        VirtualCamera.LookAt = FindObjectOfType<PlayerOverworld>().transform;
        MainMenuCanvas.SetActive(false);
    }

    public void Options()
    {
        OptionsMenu.SetActive(true);
    }

    public void CloseOptions()
    {
        OptionsMenu.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
