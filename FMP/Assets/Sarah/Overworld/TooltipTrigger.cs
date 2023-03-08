using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TooltipTrigger : MonoBehaviour
{
    public ToolTip Tooltip;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            OverworldToolTip.Instance.SetTooltip(Tooltip);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            OverworldToolTip.Instance.UnShowToolTip();
        }
    }
}
