using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public bool PlayOnEnter = true;
    public List<Dialogue> Lines;
    internal bool InRange;
    public bool OnlyPlayOnce = false;
    internal PlayAfter DestroyIf = PlayAfter.None;

    private void Start()
    {
        switch(DestroyIf)
        {
            case PlayAfter.GauntletRecruit:
                {
                    if(GameManager.Instance.GauntletRecruitComplete)
                    {
                        Destroy(this);
                    }

                    break;
                }
            case PlayAfter.ArcherRecruit:
                {
                    if (GameManager.Instance.ArcherRecruitComplete)
                    {
                        Destroy(this);
                    }

                    break;
                }
            case PlayAfter.PostDungeon1:
                {
                    if (GameManager.Instance.PostDungeon1Complete)
                    {
                        Destroy(this);
                    }

                    break;
                }
            case PlayAfter.PostDungeon2:
                {
                    if (GameManager.Instance.PostDungeon2Complete)
                    {
                        Destroy(this);
                    }

                    break;
                }
            case PlayAfter.PostTutorial:
                {
                    if (GameManager.Instance.CombatTutorialComplete)
                    {
                        Destroy(this);
                    }

                    break;
                }
        }
    }

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
