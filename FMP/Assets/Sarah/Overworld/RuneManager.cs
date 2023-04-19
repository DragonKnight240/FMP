using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public enum PuzzleLoc
{
    Desert1,
    Desert2,
    Cave
}

public class RuneManager : MonoBehaviour
{
    public PuzzleLoc PuzzleLocation;
    Runes[] Runes;
    public MoveObject ObjectOpen;
    public AudioClip AllRunesActiveSound;

    // Start is called before the first frame update
    void Start()
    {
        Runes = FindObjectsOfType<Runes>();

        for(int i = 0; i < Runes.Length; i++)
        {
            if (Runes[i].RuneLocation == PuzzleLocation)
            {
                Runes[i].RuneNumber = i + 1;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach(Runes Rune in Runes)
        {
            if(!Rune.isActive)
            {
                return;
            }
        }

        OpenDoor();
    }

    void OpenDoor()
    {
        ObjectOpen.Activate = true;
        SoundManager.Instance.PlaySFX(AllRunesActiveSound);
    }
}
