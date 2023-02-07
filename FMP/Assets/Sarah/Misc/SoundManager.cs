using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public GameObject SFXPrefab;
    public GameObject MusicPrefab;
    public GameObject AmbiancePrefab;

    // Start is called before the first frame update
    void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void PlaySFX(AudioClip Audio)
    {
        GameObject SFX = Instantiate(SFXPrefab);
        SFX.GetComponent<AudioSource>().clip = Audio;
    }

    public void PlayAmbiance(AudioClip Audio)
    {
        GameObject Ambiance = Instantiate(AmbiancePrefab);
        Ambiance.GetComponent<AudioSource>().clip = Audio;
    }

    public void PlayMusic(AudioClip Audio)
    {
        GameObject Music = Instantiate(MusicPrefab);
        Music.GetComponent<AudioSource>().clip = Audio;
    }
}
