using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public List<Dialogue> PreTutorialDialogue;
    public List<Dialogue> PostTutorialDialogue;
    internal bool InRange = false;
    public GameObject ButtonPrompt;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButton("Interact") && InRange)
        {
            if (!GameManager.Instance.CombatTutorialComplete)
            {
                DialogueSystem.Instance.StartDialogue(PreTutorialDialogue);
            }
            else
            {
                DialogueSystem.Instance.StartDialogue(PostTutorialDialogue);
            }

            if(GetComponent<HealUnits>())
            {
                GetComponent<HealUnits>().Heal();
            }
            else if (GetComponent<Vendor>())
            {
                Shop.Instance.ItemsForSale = GetComponent<Vendor>().CurrentStock;
                GetComponent<Vendor>().Pending = true;
            }

        }

        if (ButtonPrompt)
        {
            if (InRange)
            {
                ButtonPrompt.SetActive(true);
                ButtonPrompt.GetComponent<UIFade>().ToFadeIn();
            }
            else
            {
                ButtonPrompt.GetComponent<UIFade>().ToFadeOut();
            }
        }
    }


}
