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
    bool DontShow = false;

    //Shown
    internal bool CompletedTurn1 = false;
    internal Dictionary<ToolTip, bool> Seen;

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

        Seen = new Dictionary<ToolTip, bool>();

        foreach(ToolTip Tip in Tooltips)
        {
            Seen.Add(Tip, false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        print(ToolTipObject.transform.position + " != " + ToolTipObject.GetComponent<MoveToScreenLocation>().OutSightLocation);
        //print(TurnManager.Instance.isPlayerTurn);

        if (NextTutorialOnReturn)
        {
            if (ToolTipObject.GetComponent<MoveToScreenLocation>().transform.position.ToString() == ToolTipObject.GetComponent<MoveToScreenLocation>().OutSightLocation.ToString()
                && TurnManager.Instance.isPlayerTurn)
            {
                print("OG Position");
                NextTutorialOnReturn = false;
                NewToolTip(PendingToolTip);
                PendingToolTip = null;
            }
        }

        if (CompletedTurn1)
        {
            if (UnitManager.Instance.AllyUnits[0].GetComponent<UnitBase>().CurrentHealth < UnitManager.Instance.AllyUnits[0].GetComponent<UnitBase>().HealthMax * 0.4 && !ToolTipManager.Instance.Seen[FindToolTip(Tutorial.CUseItem)])
            {
                NewToolTip(FindToolTip(Tutorial.CUseItem));
            }
        }
    }

    internal ToolTip FindToolTip(Tutorial tutorial)
    {
        ToolTip WantedTip = null;
        int Index = 0;

        foreach(ToolTip Tip in Tooltips)
        {
            if(Tip.tutorial == tutorial)
            {
                WantedTip = Tip;
                CurrentToolTipIndex = Index;
                break;
            }

            Index = Index + 1;
        }

        return WantedTip;
    }

    internal void NewToolTip(ToolTip NewToolTip = null)
    {
        if(!CompletedTurn1 && NewToolTip)
        {
            if (NewToolTip.tutorial != Tutorial.CWait && NewToolTip.tutorial != Tutorial.CUnitSelect && NewToolTip.tutorial != Tutorial.CMove)
            {
                //print("Rejected " + Tooltips[CurrentToolTipIndex].name);
                return;
            }
        }

        if(!TurnManager.Instance.isPlayerTurn)
        {
            return;
        }

        if(ToolTipObject.GetComponent<MoveToScreenLocation>().transform.position.ToString() != ToolTipObject.GetComponent<MoveToScreenLocation>().OutSightLocation.ToString())
        {
            NextTutorialOnReturn = true;
            if (NewToolTip)
            {
                PendingToolTip = NewToolTip;
                //print("Pending " + NewToolTip.tutorial);
            }
            else
            {
                CurrentToolTipIndex += 1;
                PendingToolTip = Tooltips[CurrentToolTipIndex];
                //print("Pending " + Tooltips[CurrentToolTipIndex].tutorial);
            }

            return;
        }

        if (NewToolTip)
        {
            Text.text = NewToolTip.Text;
            //print("Active " + NewToolTip.tutorial);
        }
        else
        {
            CurrentToolTipIndex += 1;
            Text.text = Tooltips[CurrentToolTipIndex].Text;
            //print("Active " + Tooltips[CurrentToolTipIndex]);
        }

        Seen[Tooltips[CurrentToolTipIndex]] = true;

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
