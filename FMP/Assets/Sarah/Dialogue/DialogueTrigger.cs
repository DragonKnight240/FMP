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
    bool Checking = true;

    private void Start()
    {
        Checking = true;

        switch (DestroyIf)
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

        if (OnlyPlayOnce)
        {
            if (GameManager.Instance.TriggerDialogue.ContainsKey(this.name))
            {
                if (GameManager.Instance.TriggerDialogue[this.name])
                {
                    //if (DialogueSystem.Instance.PlayingDialogue)
                    //{
                    //    if (DialogueSystem.Instance.ToDisplayText == Lines[0].Text)
                    //    {
                    //        print("Same");
                    //        DialogueSystem.Instance.CurrentDialogue = null;
                    //        DialogueSystem.Instance.PlayingDialogue = false;
                    //    }
                    //}
                    //print("Destory");
                    Destroy(this);
                }
            }
            else
            {
                //print("Add " + GameManager.Instance.TriggerDialogue.Count);
                GameManager.Instance.TriggerDialogue.Add(this.name, false);
            }
        }

        Checking = false;

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
        GameManager.Instance.TriggerDialogue[this.name] = true;
        Destroy(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !Checking)
        {
            if (PlayOnEnter)
            {
                if (GetComponent<Objectives>())
                {
                    print("Objective");
                    GetComponent<Objectives>().InRange = true;
                }

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
