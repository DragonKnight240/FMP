using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueDestroyObject : MonoBehaviour
{
    public Dialogue Line;
    public GameObject ObjectToGo;
    public bool DestroyAtEnd = false;
    public PlayAfter GameManagerCheck;
    bool Pending = false;

    // Start is called before the first frame update
    void Start()
    {
        switch (GameManagerCheck)
        {
            case PlayAfter.ArcherRecruit:
                {
                    if(GameManager.Instance.ArcherRecruitComplete && GameManager.Instance.DialogueToPlay != PlayAfter.ArcherRecruit)
                    {
                        Destroy(ObjectToGo);
                        Destroy(this);
                    }
                    break;
                }
            case PlayAfter.GauntletRecruit:
                {
                    if (GameManager.Instance.GauntletRecruitComplete && GameManager.Instance.DialogueToPlay != PlayAfter.GauntletRecruit)
                    {
                        Destroy(ObjectToGo);
                        Destroy(this);
                    }
                    break;
                }
            case PlayAfter.PostDungeon1:
                {
                    if (GameManager.Instance.PostDungeon1Complete && GameManager.Instance.DialogueToPlay != PlayAfter.PostDungeon1)
                    {
                        Destroy(ObjectToGo);
                        Destroy(this);
                    }
                    break;
                }
            case PlayAfter.PostDungeon2:
                {
                    if (GameManager.Instance.PostDungeon2Complete && GameManager.Instance.DialogueToPlay != PlayAfter.PostDungeon2)
                    {
                        Destroy(ObjectToGo);
                        Destroy(this);
                    }
                    break;
                }
            case PlayAfter.PostTutorial:
                {
                    if (GameManager.Instance.CombatTutorialComplete && GameManager.Instance.DialogueToPlay != PlayAfter.PostTutorial)
                    {
                        Destroy(ObjectToGo);
                        Destroy(this);
                    }
                    break;
                }
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (DialogueSystem.Instance.CurrentLine == Line && !Pending)
        {
            Pending = true;
        }

        if ((Pending && !DialogueSystem.Instance.PlayingDialogue) || (!DestroyAtEnd && Pending))
        {
            Destroy(ObjectToGo);
            Destroy(this);
        }
    }
}
