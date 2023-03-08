using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialoguePlayAnimation : MonoBehaviour
{
    public Dialogue Line;
    public Animator Animator;
    public string TriggerName;


    // Start is called before the first frame update
    void Start()
    {
        if (Animator == null)
        {
            Animator = GetComponent<Animator>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (DialogueSystem.Instance.CurrentLine == Line)
        {
            Animator.SetTrigger(TriggerName);
            Destroy(this);
        }
    }
}
