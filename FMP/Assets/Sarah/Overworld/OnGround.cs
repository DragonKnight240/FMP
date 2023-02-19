using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGround : MonoBehaviour
{
    AnimationController Controller;

    private void Start()
    {
        Controller = GetComponent<AnimationController>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            print("Is on Ground");
            Controller.isOnGround = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            print("Not On Ground");
            Controller.isOnGround = false;
        }
    }
}
