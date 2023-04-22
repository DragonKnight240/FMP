using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldChest : MonoBehaviour
{
    public Item Item;
    internal bool InRange = false;
    Animator Anim;
    public GameObject ButtonPrompt;

    private void Start()
    {
        Anim = GetComponent<Animator>();
        if(Anim)
        {
            Anim.speed = 0;
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Interact") && InRange)
        {
            OpenChest();
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

    internal void OpenChest()
    {
        SoundManager.Instance.PlaySFX(GameManager.Instance.OpenChestSound);
        if(Anim)
        {
            Anim.speed = 1;
        }
        GameManager.Instance.Convoy.Add(Item);
        InRange = false;
        ButtonPrompt.GetComponent<UIFade>().ToFadeOut();
        SystemNotification.Instance.Text.text = "Obtained: " + Item.Name;
        SystemNotification.Instance.ActiveNotification();
        Destroy(this);
    }
}
