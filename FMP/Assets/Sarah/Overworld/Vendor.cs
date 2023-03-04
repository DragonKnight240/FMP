using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vendor : MonoBehaviour
{
    public List<ForSale> ToSellItems;
    internal List<ForSale> CurrentStock;
    internal bool Pending = false;

    private void Start()
    {
        CurrentStock = new List<ForSale>();

        CurrentStock = ToSellItems;
    }

    private void Update()
    {
        if(Pending)
        {
            if(!DialogueSystem.Instance.PlayingDialogue)
            {
                Pending = false;
                Shop.Instance.OpenShop();
            }
        }
    }
}
