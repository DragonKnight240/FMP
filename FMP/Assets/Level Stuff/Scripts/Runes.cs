using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runes : MonoBehaviour
{
    // Start is called before the first frame update

    internal int RuneNumber;
    public Material R_Material;
    public GameObject WallRune;
    public bool isActive = false;
    internal bool InRange = false;
    public GameObject ButtonPrompt;

    private void Start()
    {
        switch (RuneNumber)
        {
            case 1:
                {
                    if (GameManager.Instance.Rune1Active)
                    {
                        isActive = true;
                        GetComponent<Renderer>().material = R_Material;
                        WallRune.GetComponent<Renderer>().material = R_Material;
                    }
                    break;
                }
            case 2:
                {
                    if (GameManager.Instance.Rune2Active)
                    {
                        isActive = true;
                        GetComponent<Renderer>().material = R_Material;
                        WallRune.GetComponent<Renderer>().material = R_Material;
                    }
                    break;
                }
            case 3:
                {
                    if (GameManager.Instance.Rune3Active)
                    {
                        isActive = true;
                        GetComponent<Renderer>().material = R_Material;
                        WallRune.GetComponent<Renderer>().material = R_Material;
                    }
                    break;
                }
            default:
                {
                    if (GameManager.Instance.Rune1Active)
                    {
                        isActive = true;
                        GetComponent<Renderer>().material = R_Material;
                        WallRune.GetComponent<Renderer>().material = R_Material;
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

            switch(RuneNumber)
            {
                case 1:
                    {
                        GameManager.Instance.Rune1Active = true;
                        break;
                    }
                case 2:
                    {
                        GameManager.Instance.Rune2Active = true;
                        break;
                    }
                case 3:
                    {
                        GameManager.Instance.Rune3Active = true;
                        break;
                    }
                default:
                    {
                        GameManager.Instance.Rune1Active = true;
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
