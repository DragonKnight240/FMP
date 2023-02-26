using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayAfter
{
    PostTutorial,
    ArcherRecruit,
    GauntletRecruit,
    PostDungeon1,
    PostDungeon2,
    None
}

[System.Serializable]
public struct LinesToPlayAfter
{
    public PlayAfter DialogueAfter;
    public List<Dialogue> Lines;
}

public class SceneLoadDialogue : MonoBehaviour
{
    public List<LinesToPlayAfter> AfterEventDialogue;

    // Start is called before the first frame update
    void Start()
    {
        if(GameManager.Instance)
        {
            if(GameManager.Instance.DialogueToPlay == PlayAfter.None)
            {
                Destroy(this);
            }
        }
        else
        {
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch(GameManager.Instance.DialogueToPlay)
        {
            case PlayAfter.GauntletRecruit:
                {
                    GameManager.Instance.RecruitUnit("Fist");
                    break;
                }
            case PlayAfter.ArcherRecruit:
                {
                    GameManager.Instance.RecruitUnit("Bow");
                    break;
                }
        }

        if(GameManager.Instance)
        {
            if(GameManager.Instance.DialogueToPlay != PlayAfter.None)
            {
                foreach(LinesToPlayAfter Events in AfterEventDialogue)
                {
                    if(Events.DialogueAfter == GameManager.Instance.DialogueToPlay)
                    {
                        DialogueSystem.Instance.StartDialogue(Events.Lines);
                        break;
                    }
                }

                Destroy(this);
            }
        }
    }
}
