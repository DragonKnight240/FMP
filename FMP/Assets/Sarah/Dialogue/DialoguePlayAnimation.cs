using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialoguePlayAnimation : MonoBehaviour
{
    public Dialogue Line;
    public Animator Animator;
    internal AnimationController NewAnimControl;
    internal AnimationController OGAnimControl;
    public string TriggerName;


    // Start is called before the first frame update
    void Start()
    {
        Animator = GetComponent<Animator>();
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
