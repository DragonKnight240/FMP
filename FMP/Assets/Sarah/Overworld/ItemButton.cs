using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    internal Item LinkedItem;
    public bool Inventory = false;
    //public 

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(LinkedItem == null)
        {
            return;
        }

        if (LinkedItem.Type == ItemTypes.Weapon)
        {
            if(Inventory)
            {
                OverworldMenu.Instance.ItemDisplay((Weapon)LinkedItem);
            }
            else 
            {
                Shop.Instance.UpdateItemDetails((Weapon)LinkedItem);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //if(Inventory)
        //{
        //    OverworldMenu.Instance.ItemUndisplay();
        //}
        //else
        //{
        //    Shop.Instance.CloseItemDetails();
        //}
    }
}
