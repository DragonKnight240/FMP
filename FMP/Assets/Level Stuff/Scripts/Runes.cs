using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runes : MonoBehaviour
{
    // Start is called before the first frame update

    public Material R_Material;

  

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            GetComponent<Renderer>().material = R_Material;
        }
    }
        }
