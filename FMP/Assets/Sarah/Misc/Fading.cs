using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fading : MonoBehaviour
{
    public bool SingleTarget = true;
    public List<GameObject> Parents;
    internal Renderer FadingObject;
    public List<Material> TransMaterials;
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
            if (SingleTarget)
            {
                print("fading in");
                Color Colour = FadingObject.material.color;
                Colour = new Color(Colour.r, Colour.g, Colour.r, Colour.a + (FadeSpeed * Time.deltaTime));

                FadingObject.material.color = Colour;

                if (Colour.a > 1)
                {
                    FadeIn = false;
                }
            }
            else
            {
                multipleRenders(true);
            }
        }
        else if(FadeOut)
        {
            if (SingleTarget)
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
            else
            {
                multipleRenders(false);
            }
        }
    }

    public void ChangeMaterial()
    {
        foreach (GameObject Parent in Parents)
        {
            foreach (Transform Child in Parent.transform)
            {
                if (Child.GetComponent<Renderer>())
                {
                    for (int i = 0; i < TransMaterials.Count; i++)
                    {
                        string Trans = TransMaterials[i].name;

                        int Total = Child.GetComponent<Renderer>().material.name.Length;
                        Total -= 11;

                        print(Child.GetComponent<Renderer>().material.name.Remove(Total, 11));

                        if (TransMaterials[i].name.Contains(Child.GetComponent<Renderer>().material.name.Remove(Total, 11)))
                        {
                            Child.GetComponent<Renderer>().material = TransMaterials[i];
                            break;
                        }
                    }
                }
            }
        }
    }

    void multipleRenders(bool increase)
    {
        foreach (GameObject Parent in Parents)
        {
            foreach (Transform Child in Parent.transform)
            {
                if (Child.GetComponent<Renderer>())
                {
                    Color Colour = Child.GetComponent<Renderer>().material.color;

                    if (increase)
                    {
                        Colour = new Color(Colour.r, Colour.g, Colour.r, Colour.a + (FadeSpeed * Time.deltaTime));
                    }
                    else
                    {
                        Colour = new Color(Colour.r, Colour.g, Colour.r, Colour.a - (FadeSpeed * Time.deltaTime));
                    }

                    Child.GetComponent<Renderer>().material.color = Colour;

                    if (increase)
                    {
                        if (Colour.a > 1)
                        {
                            FadeIn = false;
                        }
                    }
                    else
                    {
                        if (Colour.a < 0)
                        {
                            FadeOut = false;
                            gameObject.SetActive(false);
                        }
                    }
                }
            }
        }
    }

}

