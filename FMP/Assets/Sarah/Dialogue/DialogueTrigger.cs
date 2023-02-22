using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public bool PlayOnEnter = true;
    public List<Dialogue> Lines;
    internal bool InRange;
    public bool OnlyPlayOnce = false;

    private void Update()
    {
        if(!PlayOnEnter && InRange)
        {
            if (!DialogueSystem.Instance.DialoguePrompt.activeInHierarchy)
            {
                DialogueSystem.Instance.DialoguePrompt.SetActive(true);
                DialogueSystem.Instance.DialoguePrompt.GetComponent<UIFade>().ToFadeIn();
            }

            if (Input.GetButtonDown("Interact"))
            {
                DialogueSystem.Instance.StartDialogue(Lines);

                if (OnlyPlayOnce)
                {
                    PlayOnce();
                }
            }
        }
    }

    void PlayOnce()
    {
        Destroy(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (PlayOnEnter)
            {
                DialogueSystem.Instance.DialoguePrompt.GetComponent<UIFade>().ToFadeOut();
                DialogueSystem.Instance.StartDialogue(Lines);
                if(OnlyPlayOnce)
                {
                    PlayOnce();
                }
            }
        }
    }
}
