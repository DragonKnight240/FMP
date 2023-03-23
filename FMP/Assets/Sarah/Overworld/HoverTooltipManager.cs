using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class HoverTooltipManager : MonoBehaviour
{
    public TextMeshProUGUI Text;
    public LayoutElement Layout;
    public RectTransform tipObject;
    public float CharacterLimit = 100;

    public static Action<string, Vector2, Vector2> OnMouseOver;
    public static Action OnMouseLoseFocus;

    // Start is called before the first frame update
    void Start()
    {
        HideTip();
    }

    void ShowTip(string tip, Vector2 MousePos, Vector2 Pivot)
    {
        Text.text = tip;

        Layout.enabled = (Text.text.Length > CharacterLimit? true: false);

        tipObject.pivot = Pivot;

        tipObject.gameObject.SetActive(true);
        tipObject.transform.position = new Vector2(MousePos.x, MousePos.y);
    }

    void HideTip()
    {
        Text.text = default;
        tipObject.gameObject.SetActive(false);
        //print("Hide tooltip");
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
