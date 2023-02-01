using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatAnimControl : MonoBehaviour
{
    Animator Anim;
    UnitBase Unit;

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
