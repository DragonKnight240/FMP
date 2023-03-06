using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    internal Item LinkedItem;
    //public 

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (LinkedItem.Type == ItemTypes.Weapon)
        {
            Shop.Instance.UpdateItemDetails((Weapon)LinkedItem);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Shop.Instance.CloseItemDetails();
    }
}
