using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueDestroyObject : MonoBehaviour
{
    public Dialogue Line;
    public GameObject ObjectToGo;
    bool Pending = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (DialogueSystem.Instance.CurrentLine == Line && !Pending)
        {
            Pending = true;
        }

        if (Pending && !DialogueSystem.Instance.PlayingDialogue)
        {
            Destroy(ObjectToGo);
        }
    }
}
