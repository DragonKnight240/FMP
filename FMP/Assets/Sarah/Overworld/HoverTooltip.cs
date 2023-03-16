using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string TipToShow;
    bool Hovering = false;
    float TimeToShow = 0.5f;
    float TimerShow = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Hovering)
        {
            TimerShow += Time.unscaledDeltaTime;
            if(TimerShow > TimeToShow)
            {
                TimerShow = 0;
                HoverTooltipManager.OnMouseOver(TipToShow, Input.mousePosition);
                Hovering = false;
            }
        }
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
}
