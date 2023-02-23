using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public List<GameObject> ControlledUnits;
    public List<CharacterData> UnitData;
    internal Vector3 PlayerReturnToOverworld;
    public int MaxUnits = 6;
    internal int NumRecruited = 0;
    public int MaxRecruitable = 2;
    public List<Item> Convoy;
    public int Money = 0;

    //Progress
    internal bool CombatTutorialComplete = false;
    internal bool OverworldTutorialComplete = false;
    internal bool ArcherRecruitComplete = false;
    internal bool GauntletRecruitComplete = false;
    internal bool PostDungeon1Complete = false;
    internal bool PostDungeon2Complete = false;

    //DialogueToShow
    internal PlayAfter DialogueToPlay = PlayAfter.None;

    //Sound
    internal float MasterSlider = 0;
    internal float SFXSlider = 0;
    internal float MusicSlider = 0;
    internal float AmbianceSlider = 0;

    //Timer
    public bool inCombat = false;
    public bool StartedGame = false;


    // Start is called before the first frame update
    void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        UnitData = new List<CharacterData>();
    }


    internal bool NextToolTip()
    {
        if(ToolTipManager.Instance)
        {
            ToolTipManager.Instance.NewToolTip();
            return true;
        }

        return false;
    }

    internal void ToolTipCheck(Tutorial Type)
    {
        if (ToolTipManager.Instance)
        {
            if (!CombatTutorialComplete && ToolTipManager.Instance.ToolTipObject.activeInHierarchy)
            {
                if (ToolTipManager.Instance.PendingToolTip)
                {
                    if (Type == ToolTipManager.Instance.PendingToolTip.tutorial || Type == ToolTipManager.Instance.Tooltips[ToolTipManager.Instance.CurrentToolTipIndex].tutorial)
                    {
                        ToolTipManager.Instance.CompleteToolTip(Type == Tutorial.CUnitSelect || Type == Tutorial.CMove || Type == Tutorial.CUseItem ? true : false);
                    }
                }
                else
                {
                    if (Type == ToolTipManager.Instance.Tooltips[ToolTipManager.Instance.CurrentToolTipIndex].tutorial)
                    {
                        ToolTipManager.Instance.CompleteToolTip(Type == Tutorial.CUnitSelect || Type == Tutorial.CMove || Type == Tutorial.CUseItem ? true : false);
                    }
                }
            }
        }
    }

    internal void ReturnToDefault()
    {
        CombatTutorialComplete = false;
        OverworldTutorialComplete = false;
        ArcherRecruitComplete = false;
        GauntletRecruitComplete = false;
        PostDungeon1Complete = false;
        PostDungeon2Complete = false;

        DialogueToPlay = PlayAfter.None;

        Convoy.Clear();
        Convoy = new List<Item>();

        Money = 0;

        NumRecruited = 0;

        UnitData.Clear();
        UnitData = new List<CharacterData>();

        inCombat = false;
        StartedGame = false;
    }
}
