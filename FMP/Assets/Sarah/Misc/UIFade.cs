using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFade : MonoBehaviour
{
    internal bool FadeIn = false;
    internal bool FadeOut = false;
    public float FadeSpeed = 10;
    CanvasGroup FadingObject;

    // Start is called before the first frame update
    void Start()
    {
        FadingObject = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        if (FadeIn)
        {
            FadingObject.alpha = (FadingObject.alpha + (FadeSpeed * Time.unscaledDeltaTime));

            if (FadingObject.alpha >= 1)
            {
                FadeIn = false;
            }
        }
        else if(FadeOut)
        {
            FadingObject.alpha = (FadingObject.alpha - (FadeSpeed * Time.unscaledDeltaTime));

            if (FadingObject.alpha <= 0)
            {
                FadeOut = false;
                gameObject.SetActive(false);
            }
        }

    }

    internal void ToFadeIn()
    {
        FadeOut = false;
        FadeIn = true;
    }

    internal void ToFadeOut()
    {
        FadeIn = false;
        FadeOut = true;
    }
}
