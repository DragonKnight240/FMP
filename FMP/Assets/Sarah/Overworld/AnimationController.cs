using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    Animator Anim;
    Rigidbody RB;
    public float yFallMin = 0.5f;
    public float LeftRightVelocityMin = 0.5f;
    public bool isOnGround = true;

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
            if (RB.velocity.y < -yFallMin && !isOnGround)
            {
                Anim.ResetTrigger("Move");
                Anim.ResetTrigger("Idle");
                Anim.SetTrigger("Fall");
            }
            else
            {
                print(RB.velocity);

                if (!(RB.velocity.x < LeftRightVelocityMin && -LeftRightVelocityMin < RB.velocity.x) ||
                    !(RB.velocity.y < LeftRightVelocityMin && -LeftRightVelocityMin < RB.velocity.y))
                {
                    Anim.ResetTrigger("Fall");
                    Anim.ResetTrigger("Idle");
                    Anim.SetTrigger("Move");
                }
                else
                {
                    Anim.ResetTrigger("Fall");
                    Anim.ResetTrigger("Move");
                    Anim.SetTrigger("Idle");
                }
            }
        }
        else
        {
            Anim.ResetTrigger("Fall");
            Anim.ResetTrigger("Move");
            Anim.SetTrigger("Idle");
        }
    }
}
