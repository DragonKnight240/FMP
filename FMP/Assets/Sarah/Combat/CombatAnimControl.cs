using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatAnimControl : MonoBehaviour
{
    internal enum AnimParameters
    {
        Move,
        Idle,
        Attack
    }

    Animator Anim;
    UnitBase Unit;
    internal AnimParameters CurrentAnimation = AnimParameters.Idle;

    // Start is called before the first frame update
    void Start()
    {
        Anim = GetComponent<Animator>();
        Unit = GetComponent<UnitBase>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Unit.Moving)
        {
            if (CurrentAnimation != AnimParameters.Move)
            {
                CurrentAnimation = AnimParameters.Move;
                Anim.SetTrigger("Move");
                //ResetAllTriggers("Move");
            }
        }
        else if(CurrentAnimation != AnimParameters.Attack)
        {
            if (CurrentAnimation != AnimParameters.Idle)
            {
                CurrentAnimation = AnimParameters.Idle;
                Anim.SetTrigger("Idle");
                //ResetAllTriggers("Idle");
            }
        }
    }

    void ResetAllTriggers(string Exception = "")
    {
        foreach(UnityEngine.AnimatorControllerParameter Paramater in Anim.parameters)
        {
            if (Paramater.ToString() != Exception)
            {
                Anim.ResetTrigger(Paramater.ToString());
            }
        }
    }
}
