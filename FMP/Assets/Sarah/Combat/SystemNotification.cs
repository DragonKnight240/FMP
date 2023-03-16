using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SystemNotification : MonoBehaviour
{
    public static SystemNotification Instance;

    public TMP_Text Text;
    public GameObject MainNotifiction;

    // Start is called before the first frame update
    void Start()
    {
        if(SystemNotification.Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        MainNotifiction.GetComponent<CanvasGroup>().alpha = 0;
        MainNotifiction.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (MainNotifiction.activeInHierarchy && !DialogueSystem.Instance.PlayingDialogue)
            {
                MainNotifiction.GetComponent<UIFade>().ToFadeOut();
            }
        }
    }

    public void ActiveNotification()
    {
        MainNotifiction.SetActive(true);
        MainNotifiction.GetComponent<UIFade>().ToFadeIn();
    }

}
