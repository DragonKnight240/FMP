using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string TipToShow;
    internal bool Hovering = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Hovering)
        {
            Vector2 Position = Input.mousePosition;

            float pivotX = Position.x / Screen.width;
            float pivotY = Position.y / Screen.height;

            HoverTooltipManager.OnMouseOver(TipToShow, Position, new Vector2(pivotX,pivotY));
        }
    }

    public void ChangeText(string NewText)
    {
        TipToShow = NewText;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Hovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Hovering = false;
        HoverTooltipManager.OnMouseLoseFocus();
    }

    public void OnMouseEnter()
    {
        Hovering = true;
    }

    public void OnMouseExit()
    {
        Hovering = false;
        HoverTooltipManager.OnMouseLoseFocus();
    }
}
