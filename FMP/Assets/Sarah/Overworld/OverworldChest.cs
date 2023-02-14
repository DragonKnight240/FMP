using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldChest : MonoBehaviour
{
    public Item Item;
    Animator Anim;

    private void Start()
    {
        Anim = GetComponent<Animator>();
        if(Anim)
        {
            Anim.speed = 0;
        }
    }

    private void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Q))
        //{
        //    OpenChest();
        //}
    }

    internal void OpenChest()
    {
        if(Anim)
        {
            Anim.speed = 1;
        }
        GameManager.Instance.Convoy.Add(Item);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            OpenChest();
        }
    }
}
