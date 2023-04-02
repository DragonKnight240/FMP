using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueCredits : MonoBehaviour
{
    public Dialogue ActivateDialogue;
    public GameObject CreditsMain;
    internal bool Pending = false;

    private void Start()
    {
        if (CreditsMain)
        {
            CreditsMain.GetComponent<CanvasGroup>().alpha = 0;
            CreditsMain.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (ActivateDialogue)
        {
            if (ActivateDialogue == DialogueSystem.Instance.CurrentLine && !Pending)
            {
                Pending = true;
            }

            if (Pending && !DialogueSystem.Instance.PlayingDialogue)
            {
                CreditsMain.SetActive(true);
                CreditsMain.GetComponent<UIFade>().ToFadeIn();
                CreditsMain.GetComponentInChildren<Animator>().enabled = true;
            }
        }
    }

    public void LoadMenu()
    {
        Options.Instance.LoadWebpage("https://forms.gle/s29mEgD3bTF735759");
        GameManager.Instance.ReturnToDefault();
        SceneLoader.Instance.ReloadScene();
    }
}
