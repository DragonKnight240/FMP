using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MusicSwitch : MonoBehaviour
{
    public AudioClip BiomeMusic;

    public Camera cam;

    void Start()
    {
  
    }

    // Update is called once per frame
    
        void OnTriggerEnter(Collider other)
        {
            if (other.transform.tag == "Player")
            {
               AudioSource audio = cam.GetComponent < AudioSource > ();


            if (audio.clip != BiomeMusic) {

                audio.clip = BiomeMusic;
                audio.Play();
            }

            
            }

        }
    }

