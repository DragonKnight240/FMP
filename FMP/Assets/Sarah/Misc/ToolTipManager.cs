using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ToolTipManager : MonoBehaviour
{
    public static ToolTipManager Instance;
    public List<ToolTip> Tooltips;
    public bool InCombat;
    public GameObject ToolTipObject;
    public TMP_Text Text;
    internal int CurrentToolTipIndex = -1;
    internal bool NextTutorialOnReturn = false;
    internal ToolTip PendingToolTip = null;
    bool ShownAllToolTips = false;

    // Start is called before the first frame update
    void Start()
    {
        if(GameManager.Instance)
        {
            if(GameManager.Instance.CombatTutorialComplete)
            {
                Destroy(this.gameObject);
            }
        }

        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        ShownAllToolTips = CurrentToolTipIndex >= Tooltips.Count ? true : false;

        if (!ShownAllToolTips)
        {
            if (NextTutorialOnReturn)
            {
                if (ToolTipObject.GetComponent<MoveToScreenLocation>().transform.position == ToolTipObject.GetComponent<MoveToScreenLocation>().OutSightLocation)
                {
                    NextTutorialOnReturn = false;
                    NewToolTip(PendingToolTip);
                    PendingToolTip = null;
                }
            }
        }
        else
        {
            Interact.Instance.CombatMenu.DisplayVictoryScreen();
        }
    }

    internal void NewToolTip(ToolTip NewToolTip = null)
    {
        if(ToolTipObject.GetComponent<MoveToScreenLocation>().transform.position != ToolTipObject.GetComponent<MoveToScreenLocation>().OutSightLocation)
        {
            NextTutorialOnReturn = true;
            CurrentToolTipIndex += 1;
            PendingToolTip = NewToolTip ? NewToolTip : Tooltips[CurrentToolTipIndex];

            return;
        }

        if (NewToolTip)
        {
            Text.text = NewToolTip.Text;
        }
        else
        {
            CurrentToolTipIndex += 1;
            Text.text = Tooltips[CurrentToolTipIndex].Text;
        }

        ToolTipObject.GetComponent<MoveToScreenLocation>().Display = true;
    }

    internal void CompleteToolTip(bool DisplayNextTutorial = false)
    {
        ToolTipObject.GetComponent<MoveToScreenLocation>().Display = false;
        NextTutorialOnReturn = DisplayNextTutorial;

        if (NextTutorialOnReturn)
        {
            NewToolTip();
        }
    }
}
