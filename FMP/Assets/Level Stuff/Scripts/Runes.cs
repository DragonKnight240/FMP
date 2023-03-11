using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runes : MonoBehaviour
{
    // Start is called before the first frame update

    public Material R_Material;
    public GameObject WallRune;
    public bool isActive = false;
    internal bool InRange = false;
    public GameObject ButtonPrompt;

    private void Update()
    {
        if (Input.GetButton("Interact") && InRange)
        {
            GetComponent<Renderer>().material = R_Material;
            WallRune.GetComponent<Renderer>().material = R_Material;
            isActive = true;
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
