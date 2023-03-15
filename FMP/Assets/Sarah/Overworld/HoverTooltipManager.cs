using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;


public class HoverTooltipManager : MonoBehaviour
{
    public TextMeshProUGUI Text;
    public RectTransform tipObject;
    public float OffsetX = 5;

    public static Action<string, Vector2> OnMouseOver;
    public static Action OnMouseLoseFocus;

    // Start is called before the first frame update
    void Start()
    {
        HideTip();
    }

    void ShowTip(string tip, Vector2 MousePos)
    {
        Text.text = tip;
        tipObject.sizeDelta = new Vector2(Text.preferredWidth > 200 ? 200 : Text.preferredWidth, Text.preferredHeight);

        tipObject.gameObject.SetActive(true);
        tipObject.transform.position = new Vector2(MousePos.x + OffsetX, MousePos.y);
    }

    void HideTip()
    {
        Text.text = default;
        tipObject.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        OnMouseOver += ShowTip;
        OnMouseLoseFocus += HideTip;
    }

    private void OnDisable()
    {
        OnMouseOver -= ShowTip;
        OnMouseLoseFocus -= HideTip;
    }
}
