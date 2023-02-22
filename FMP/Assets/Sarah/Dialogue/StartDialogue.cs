using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartDialogue : MonoBehaviour
{
    public List<Dialogue> Lines;

    private void Start()
    {
        if(GameManager.Instance)
        {
            if(GameManager.Instance.StartedGame)
            {
                Destroy(this);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance)
        {
            if(GameManager.Instance.StartedGame)
            {
                DialogueSystem.Instance.StartDialogue(Lines);
                Destroy(this);
            }
        }
    }
}
