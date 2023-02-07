using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fading : MonoBehaviour
{
    internal Renderer FadingObject;
    public float FadeSpeed = 10;
    internal bool FadeIn = false;
    internal bool FadeOut = false;

    // Start is called before the first frame update
    void Start()
    {
        FadingObject = gameObject.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(FadeIn)
        {
            print("fading in");
            Color Colour = FadingObject.material.color;
            Colour = new Color(Colour.r, Colour.g, Colour.r, Colour.a + (FadeSpeed * Time.deltaTime));

            FadingObject.material.color = Colour;

            if(Colour.a > 1)
            {
                FadeIn = false;
            }
        }
        else if(FadeOut)
        {
            print("fading out");
            Color Colour = FadingObject.material.color;
            Colour = new Color(Colour.r, Colour.g, Colour.r, Colour.a - (FadeSpeed * Time.deltaTime));

            FadingObject.material.color = Colour;

            if (Colour.a < 0)
            {
                FadeOut = false;
                gameObject.SetActive(false);
            }
        }
    }
}
