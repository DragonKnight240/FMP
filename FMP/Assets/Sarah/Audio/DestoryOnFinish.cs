using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryOnFinish : MonoBehaviour
{
    AudioSource Audio;

    // Start is called before the first frame update
    void Start()
    {
        Audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Audio)
        {
            if(!Audio.isPlaying)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
