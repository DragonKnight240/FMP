using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TooltipTrigger : MonoBehaviour
{
    public ToolTip Tooltip;
    public bool OnlyOnce = false;

    private void Start()
    {

    }

    private void Update()
    {
        switch (Tooltip.tutorial)
        {
            case Tutorial.OMove:
                {
                    if (GameManager.Instance.OverworldMoveTutorialComplete)
                    {
                        Destroy(this.gameObject);
                    }
                    break;
                }
            case Tutorial.OEnterCombat:
                {
                    if(GameManager.Instance.OverworldEnemyTutorialComplete)
                    {
                        Destroy(this.gameObject);
                    }
                    break;
                }
        }

    }

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
            
            switch(Tooltip.tutorial)
            {
                case Tutorial.OEnterCombat:
                    {
                        GameManager.Instance.OverworldEnemyTutorialComplete = true;
                        break;
                    }
                case Tutorial.OMove:
                    {
                        GameManager.Instance.OverworldMoveTutorialComplete = true;
                        break;
                    }
            }

            if(OnlyOnce)
            {
                Destroy(this);
            }
        }
    }
}
