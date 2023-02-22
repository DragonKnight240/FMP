using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeCheck : MonoBehaviour
{
    public GameObject InteractPrompt;

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<NPC>())
        {
            other.GetComponent<NPC>().InRange = true;
        }
        else if(other.GetComponent<OverworldChest>())
        {
            other.GetComponent<OverworldChest>().InRange = true;
        }
        else if(GetComponent<DialogueTrigger>())
        {
            other.GetComponent<DialogueTrigger>().InRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<NPC>())
        {
            other.GetComponent<NPC>().InRange = false;
        }
        else if (other.GetComponent<OverworldChest>())
        {
            other.GetComponent<OverworldChest>().InRange = false;
        }
        else if (GetComponent<DialogueTrigger>())
        {
            other.GetComponent<DialogueTrigger>().InRange = false;
        }
    }
}
