using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    public static DialogueSystem Instance;
    internal Queue<Dialogue> CurrentDialogue;
    internal string ToDisplayText = "";
    public GameObject TextBox;
    public TMP_Text Text;
    public TMP_Text SpeakerName;

    public GameObject DialoguePrompt;

    internal float LetterDiplayTimer = 0;
    public float LetterDiplayTime = 0.2f;
    int CurrentIndex = 0;
    internal bool PlayingDialogue = false;
    internal Dialogue CurrentLine;

    private void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        CurrentDialogue = new Queue<Dialogue>();
        TextBox.GetComponent<CanvasGroup>().alpha = 0;
        TextBox.SetActive(false);

        DialoguePrompt.GetComponent<CanvasGroup>().alpha = 0;
        DialoguePrompt.SetActive(false);
    }

    private void Update()
    {
        UpdateText();
    }

    internal void UpdateText()
    {
        if (TextBox.activeInHierarchy)
        {
            if (!CurrentLine.SystemNotification)
            {
                if (ToDisplayText != Text.text)
                {
                    LetterDiplayTimer += Time.unscaledDeltaTime;

                    if (LetterDiplayTimer >= LetterDiplayTime)
                    {
                        LetterDiplayTimer = 0;
                        NextLetter();
                    }
                }
            }

            if (Input.GetButtonDown("Fire1"))
            {
                if (!CurrentLine.SystemNotification)
                {
                    if (ToDisplayText == Text.text)
                    {
                        NextLine();
                    }
                    else
                    {
                        CurrentIndex = ToDisplayText.Length - 1;
                        Text.text = ToDisplayText;
                    }
                }
                else
                {
                    SystemNotification.Instance.MainNotifiction.GetComponent<UIFade>().ToFadeOut();
                    NextLine();
                }
            }
        }
    }

    public void StartDialogue(List<Dialogue> Dialogue)
    {
        Time.timeScale = 0;
        PlayingDialogue = true;
        CurrentDialogue.Clear();

        foreach(Dialogue Line in Dialogue)
        {
            CurrentDialogue.Enqueue(Line);
        }

        TextBox.SetActive(true);
        TextBox.GetComponent<UIFade>().ToFadeIn();
        NextLine();
    }

    internal void NextLetter()
    {
        Text.text += ToDisplayText[CurrentIndex];
        CurrentIndex++;
    }

    internal void NextLine()
    {
        if(CurrentDialogue.Count == 0)
        {
            EndDialogue();
        }
        else
        {
            CurrentLine = CurrentDialogue.Dequeue();

            if (!CurrentLine.SystemNotification)
            {
                Text.text = "";
                CurrentIndex = 0;
                SpeakerName.text = CurrentLine.Speaker;

                if (SpeakerName.text == "")
                {
                    SpeakerName.transform.parent.gameObject.SetActive(false);
                }
                else
                {
                    SpeakerName.transform.parent.gameObject.SetActive(true);
                }

                ToDisplayText = CurrentLine.Text;
            }
            else
            {
                SystemNotification.Instance.Text.text = CurrentLine.Text;

                SystemNotification.Instance.ActiveNotification();
            }
        }
    }

    public void EndDialogue()
    {
        TextBox.GetComponent<UIFade>().ToFadeOut();
        PlayingDialogue = false;
        Time.timeScale = 1;
    }
}
