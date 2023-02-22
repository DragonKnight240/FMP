using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public List<Dialogue> Dialogue;
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
            DialogueSystem.Instance.StartDialogue(Dialogue);

            if(GetComponent<HealUnits>())
            {
                GetComponent<HealUnits>().Heal();
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
