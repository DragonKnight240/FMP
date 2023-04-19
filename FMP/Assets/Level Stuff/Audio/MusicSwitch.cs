using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MusicSwitch : MonoBehaviour
{
    public AudioClip BiomeMusic;

    public Camera cam;

    public Zone Zone;

    internal bool PendingChange;
    internal bool PendingIncrease;
    AudioSource Audio;
    public float SpeedChange = 0.05f;
    internal float StartVolume;

    void Start()
    {
        Audio = cam.GetComponent<AudioSource>();
        StartVolume = Audio.volume;
    }

    private void Update()
    {
        if(PendingChange)
        {
            Audio.volume = Mathf.Lerp(Audio.volume, 0, SpeedChange);

            if (Audio.volume <= 0.05)
            {
                PendingChange = false;
                PendingIncrease = true;
                Audio.clip = BiomeMusic;
                Audio.Play();
            }
        }
        else if(PendingIncrease)
        {
            Audio.volume = Mathf.Lerp(Audio.volume, StartVolume, SpeedChange);

            if(Audio.volume >= StartVolume - 0.05)
            {
                Audio.volume = StartVolume;
                PendingIncrease = false;
            }
        }
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            if (Audio.clip != BiomeMusic)
            {
                Compass.Instance.ChangeMarkerLocation(Zone);
                PendingChange = true;
            }
        }
    }
}

