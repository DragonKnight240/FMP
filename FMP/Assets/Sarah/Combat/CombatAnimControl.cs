using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatAnimControl : MonoBehaviour
{
    Animator Anim;
    Rigidbody RB;

    // Start is called before the first frame update
    void Start()
    {
        Anim = GetComponent<Animator>();
        RB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (RB.velocity != new Vector3(0, 0, 0))
        {
            Anim.SetTrigger("Move");
            ResetAllTriggers();
        }
    }

    void ResetAllTriggers()
    {
        foreach(UnityEngine.AnimatorControllerParameter Paramater in Anim.parameters)
        {
            Anim.ResetTrigger(Paramater.ToString());
        }
    }
}
