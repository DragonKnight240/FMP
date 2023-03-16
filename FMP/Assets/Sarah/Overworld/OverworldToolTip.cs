using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OverworldToolTip : MonoBehaviour
{
    public static OverworldToolTip Instance;

    internal Tutorial CurrentToolTip;
    public GameObject TooltipMain;
    public TMP_Text Text;

    private void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        TooltipMain.GetComponent<MoveToScreenLocation>().Display = false;
    }

    internal void SetTooltip(ToolTip Tip)
    {
        CurrentToolTip = Tip.tutorial;
        Text.text = Tip.Text;
        TooltipMain.GetComponent<MoveToScreenLocation>().Display = true;
        //print("Show Tooltip");
    }

    internal void UnShowToolTip()
    {
        TooltipMain.GetComponent<MoveToScreenLocation>().Display = false;
        //print("Unshow Tooltip");
    }
}
