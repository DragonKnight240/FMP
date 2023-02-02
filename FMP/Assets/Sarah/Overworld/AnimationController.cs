using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    Animator Anim;
    Rigidbody RB;

    // Start is called before the first frame update
    void Start()
    {
        RB = GetComponent<Rigidbody>();
        if(RB == null)
        {
            RB = GetComponentInParent<Rigidbody>();
        }

        Anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(RB.velocity != new Vector3(0,0,0))
        {
            if (RB.velocity.y < 0)
            {
                Anim.SetTrigger("Fall");
                Anim.ResetTrigger("Move");
                Anim.ResetTrigger("Idle");
            }
            else
            {
                Anim.SetTrigger("Move");
                Anim.ResetTrigger("Fall");
                Anim.ResetTrigger("Idle");
            }
        }
        else
        {
            Anim.SetTrigger("Idle");
            Anim.ResetTrigger("Fall");
            Anim.ResetTrigger("Move");
        }
    }
}
