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

    void Start()
    {
        options = GetComponent<Options>();
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
                MainCamera.SetActive(true);
                options.InGame = true;
                FindObjectOfType<PlayerOverworld>().CanMove = true;
                Destroy(this);
            }

            PreviousPos = DollyCart.m_Position;
        }
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
