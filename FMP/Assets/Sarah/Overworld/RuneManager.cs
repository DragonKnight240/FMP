using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneManager : MonoBehaviour
{
    Runes[] Runes;
    public MoveObject ObjectOpen;


    // Start is called before the first frame update
    void Start()
    {
        Runes = FindObjectsOfType<Runes>();
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
    }
}
