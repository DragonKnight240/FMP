using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCombatEndDialogue : MonoBehaviour
{
    public PlayAfter DialogueToPlayAfter;

    internal void SetDialogue()
    {
        GameManager.Instance.DialogueToPlay = DialogueToPlayAfter;
    }
}
