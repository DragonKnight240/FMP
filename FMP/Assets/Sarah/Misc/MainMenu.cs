using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MainMenu : MonoBehaviour
{
    public GameObject MainMenuCanvas;
    public GameObject DollyCartObject;
    CinemachineDollyCart DollyCart;
    internal float PreviousPos;
    public GameObject MainCamera;


    void Start()
    {
        DollyCart = DollyCartObject.GetComponent<CinemachineDollyCart>();
    }

    private void Update()
    {
        if(DollyCart.m_Position != 0)
        {
            if(DollyCart.m_Position <= PreviousPos)
            {
                DollyCartObject.SetActive(false);
            }

            PreviousPos = DollyCart.m_Position;
        }
    }

    public void PlayGame()
    {
        DollyCartObject.SetActive(true);
        MainMenuCanvas.SetActive(false);
    }
}
