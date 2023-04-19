using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runes : MonoBehaviour
{
    // Start is called before the first frame update
    public PuzzleLoc RuneLocation;
    internal int RuneNumber;
    public Material R_Material;
    public GameObject WallRune;
    public bool isActive = false;
    internal bool InRange = false;
    public GameObject ButtonPrompt;

    public AudioClip ActiveSound;

    private void Start()
    {
        switch (RuneNumber)
        {
            case 1:
                {
                    switch(RuneLocation)
                    {
                        case PuzzleLoc.Cave:
                            {
                                if (GameManager.Instance.Rune1Active)
                                {
                                    isActive = true;
                                    GetComponent<Renderer>().material = R_Material;
                                    WallRune.GetComponent<Renderer>().material = R_Material;
                                }
                                break;
                            }
                        case PuzzleLoc.Desert1:
                            {
                                if (GameManager.Instance.Rune1Desert1)
                                {
                                    isActive = true;
                                    GetComponent<Renderer>().material = R_Material;
                                    WallRune.GetComponent<Renderer>().material = R_Material;
                                }
                                break;
                            }
                        case PuzzleLoc.Desert2:
                            {
                                if (GameManager.Instance.Rune1Desert2)
                                {
                                    isActive = true;
                                    GetComponent<Renderer>().material = R_Material;
                                    WallRune.GetComponent<Renderer>().material = R_Material;
                                }
                                break;
                            }
                    }
                    break;
                }
            case 2:
                {
                    switch (RuneLocation)
                    {
                        case PuzzleLoc.Cave:
                            {
                                if (GameManager.Instance.Rune2Active)
                                {
                                    isActive = true;
                                    GetComponent<Renderer>().material = R_Material;
                                    WallRune.GetComponent<Renderer>().material = R_Material;
                                }
                                break;
                            }
                        case PuzzleLoc.Desert1:
                            {
                                if (GameManager.Instance.Rune2Desert1)
                                {
                                    isActive = true;
                                    GetComponent<Renderer>().material = R_Material;
                                    WallRune.GetComponent<Renderer>().material = R_Material;
                                }
                                break;
                            }
                        case PuzzleLoc.Desert2:
                            {
                                if (GameManager.Instance.Rune2Desert2)
                                {
                                    isActive = true;
                                    GetComponent<Renderer>().material = R_Material;
                                    WallRune.GetComponent<Renderer>().material = R_Material;
                                }
                                break;
                            }
                    }
                    break;
                }
            case 3:
                {
                    switch (RuneLocation)
                    {
                        case PuzzleLoc.Cave:
                            {
                                if (GameManager.Instance.Rune3Active)
                                {
                                    isActive = true;
                                    GetComponent<Renderer>().material = R_Material;
                                    WallRune.GetComponent<Renderer>().material = R_Material;
                                }
                                break;
                            }
                        case PuzzleLoc.Desert2:
                            {
                                if (GameManager.Instance.Rune3Desert2)
                                {
                                    isActive = true;
                                    GetComponent<Renderer>().material = R_Material;
                                    WallRune.GetComponent<Renderer>().material = R_Material;
                                }
                                break;
                            }
                    }
                    break;
                }
            default:
                {
                    switch (RuneLocation)
                    {
                        case PuzzleLoc.Cave:
                            {
                                if (GameManager.Instance.Rune1Active)
                                {
                                    isActive = true;
                                    GetComponent<Renderer>().material = R_Material;
                                    WallRune.GetComponent<Renderer>().material = R_Material;
                                }
                                break;
                            }
                        case PuzzleLoc.Desert1:
                            {
                                if (GameManager.Instance.Rune1Desert1)
                                {
                                    isActive = true;
                                    GetComponent<Renderer>().material = R_Material;
                                    WallRune.GetComponent<Renderer>().material = R_Material;
                                }
                                break;
                            }
                        case PuzzleLoc.Desert2:
                            {
                                if (GameManager.Instance.Rune1Desert2)
                                {
                                    isActive = true;
                                    GetComponent<Renderer>().material = R_Material;
                                    WallRune.GetComponent<Renderer>().material = R_Material;
                                }
                                break;
                            }
                    }
                    break;
                }
        }
    }

    private void Update()
    {
        if (Input.GetButton("Interact") && InRange && !isActive)
        {
            isActive = true;
            GetComponent<Renderer>().material = R_Material;
            WallRune.GetComponent<Renderer>().material = R_Material;
            SoundManager.Instance.PlaySFX(ActiveSound);

            switch(RuneNumber)
            {
                case 1:
                    {
                        switch(RuneLocation)
                        {
                            case PuzzleLoc.Desert1:
                                {
                                    GameManager.Instance.Rune1Desert1 = true;
                                    break;
                                }
                            case PuzzleLoc.Desert2:
                                {
                                    GameManager.Instance.Rune1Desert2 = true;
                                    break;
                                }
                            case PuzzleLoc.Cave:
                                {
                                    GameManager.Instance.Rune1Active = true;
                                    break;
                                }
                        }
                        break;
                    }
                case 2:
                    {
                        switch (RuneLocation)
                        {
                            case PuzzleLoc.Desert1:
                                {
                                    GameManager.Instance.Rune2Desert1 = true;
                                    break;
                                }
                            case PuzzleLoc.Desert2:
                                {
                                    GameManager.Instance.Rune2Desert2 = true;
                                    break;
                                }
                            case PuzzleLoc.Cave:
                                {
                                    GameManager.Instance.Rune2Active = true;
                                    break;
                                }
                        }
                        break;
                    }
                case 3:
                    {
                        switch (RuneLocation)
                        {
                            case PuzzleLoc.Desert2:
                                {
                                    GameManager.Instance.Rune3Desert2 = true;
                                    break;
                                }
                            case PuzzleLoc.Cave:
                                {
                                    GameManager.Instance.Rune3Active = true;
                                    break;
                                }
                        }
                        break;
                    }
                default:
                    {
                        switch (RuneLocation)
                        {
                            case PuzzleLoc.Desert1:
                                {
                                    GameManager.Instance.Rune1Desert1 = true;
                                    break;
                                }
                            case PuzzleLoc.Desert2:
                                {
                                    GameManager.Instance.Rune1Desert2 = true;
                                    break;
                                }
                            case PuzzleLoc.Cave:
                                {
                                    GameManager.Instance.Rune1Active = true;
                                    break;
                                }
                        }
                        break;
                    }
            }
        }

        if (ButtonPrompt)
        {
            if (InRange)
            {
                ButtonPrompt.SetActive(true);
                ButtonPrompt.GetComponent<UIFade>().ToFadeIn();
            }
            else
            {
                ButtonPrompt.GetComponent<UIFade>().ToFadeOut();
            }
        }
    }
}
