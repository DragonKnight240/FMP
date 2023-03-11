using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public List<GameObject> ControlledUnits;
    public  List<GameObject> AvailableUnits;
    public List<CharacterData> UnitData;
    internal Vector3 PlayerReturnToOverworld;
    internal Quaternion PlayerReturnRotation;
    internal int CurrentUnitNum = 1;
    internal int NumRecruited = 0;
    public int MaxRecruitable = 2;
    public List<Item> Convoy;
    public int Money = 0;

    internal int OverworldLevelID;

    //Progress
    internal bool CombatTutorialComplete = false;
    internal bool OverworldTutorialComplete = false;
    internal bool OverworldMoveTutorialComplete = false;
    internal bool OverworldEnemyTutorialComplete = false;
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

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        UnitData = new List<CharacterData>();

        CurrentUnitNum = AvailableUnits.Count;

        if (FindObjectOfType<PlayerOverworld>())
        {
            PlayerReturnToOverworld = FindObjectOfType<PlayerOverworld>().transform.position;
            PlayerReturnRotation = FindObjectOfType<PlayerOverworld>().transform.rotation;
        }
    }


    // Start is called before the first frame update
    void Start()
    {

    }


    internal bool NextToolTip(ToolTip Tip = null)
    {
        if(ToolTipManager.Instance)
        {
            ToolTipManager.Instance.NewToolTip(Tip);
            return true;
        }

        return false;
    }

    internal void RecruitUnit(string RecruitName)
    {
        foreach (GameObject Unit in ControlledUnits)
        {
            if (Unit.name.Contains(RecruitName))
            {
                print("Recruited");
                GameObject TempUnit = Instantiate(Unit, new Vector3(0, 0, 0), Quaternion.identity);
                TempUnit.SetActive(false);
                SetUpUnit(TempUnit.GetComponent<UnitBase>());
                AddCharacterData(TempUnit.GetComponent<UnitBase>());
                AvailableUnits.Add(Unit);

                Destroy(TempUnit);
                break;
            }
        }
    }

    internal void AddCharacterData(UnitBase Unit)
    {
        CharacterData data = new CharacterData();

        data.UnitName = Unit.UnitName;
        data.HealthMax = Unit.HealthMax;
        data.CurrentHealth = Unit.CurrentHealth;
        data.Movement = Unit.Movement;

        //Inventory
        data.Inventory = Unit.Inventory;

        //Stats
        data.Level = Unit.Level;
        data.EXP = Unit.EXP;
        data.Strength = Unit.Strength;
        data.Dexterity = Unit.Dexterity;
        data.Magic = Unit.Magic;
        data.Defence = Unit.Defence;
        data.Resistance = Unit.Resistance;
        data.Speed = Unit.Speed;
        data.Luck = Unit.Luck;

        //Weapon Proficientcy
        data.BowProficiency = Unit.BowProficiency;
        data.BowLevel = Unit.BowLevel;

        data.SwordProficiency = Unit.SwordProficiency;
        data.SwordLevel = Unit.SwordLevel;

        data.MagicProficiency = Unit.MagicProficiency;
        data.MagicLevel = Unit.MagicLevel;

        data.FistProficiency = Unit.FistProficiency;
        data.FistLevel = Unit.FistLevel;

        //Class
        data.Class = Unit.Class;

        //Attack
        data.UnlockedAttacks = Unit.UnlockedAttacks;

        //Support
        data.Supports = Unit.SupportsWith;

        GameManager.Instance.UnitData.Add(data);
    }

    void SetUpUnit(UnitBase Unit)
    {
        Unit.CurrentHealth = Unit.HealthMax;

        if (Unit.Class != null)
        {
            Unit.Class = Instantiate(Unit.Class);

            Unit.Class.FindLevel();
            Unit.Class.AbilityUnlock(Unit);
        }
    }

    internal void ToolTipCheck(Tutorial Type)
    {
        //print("Checking");
        if (ToolTipManager.Instance)
        {
            if (!CombatTutorialComplete && ToolTipManager.Instance.ToolTipObject.activeInHierarchy)
            {
                if(ToolTipManager.Instance.CurrentToolTipIndex < 0)
                {
                    ToolTipManager.Instance.CurrentToolTipIndex = 0;
                }
                else if(ToolTipManager.Instance.CurrentToolTipIndex > ToolTipManager.Instance.Tooltips.Count - 1)
                {
                    ToolTipManager.Instance.CurrentToolTipIndex = ToolTipManager.Instance.Tooltips.Count - 1;
                }

                if (ToolTipManager.Instance.PendingToolTip)
                {
                    //print("Pending: Checking " + Type + " - Current: " + ToolTipManager.Instance.PendingToolTip.tutorial);
                    //print("Active: Checking " + Type + " - Current: " + ToolTipManager.Instance.Tooltips[ToolTipManager.Instance.CurrentToolTipIndex].tutorial);
                    if (Type == ToolTipManager.Instance.PendingToolTip.tutorial || Type == ToolTipManager.Instance.Tooltips[ToolTipManager.Instance.CurrentToolTipIndex].tutorial)
                    {
                        ToolTipManager.Instance.CompleteToolTip(Type == Tutorial.CUnitSelect || Type == Tutorial.CMove || Type == Tutorial.CChangeWeapon ? true : false);
                    }
                }
                else
                {
                    //print("Active: Checking " + Type + " - Current: " + ToolTipManager.Instance.Tooltips[ToolTipManager.Instance.CurrentToolTipIndex].tutorial);
                    if (Type == ToolTipManager.Instance.Tooltips[ToolTipManager.Instance.CurrentToolTipIndex].tutorial)
                    {
                        //print("Complete");
                        ToolTipManager.Instance.CompleteToolTip(Type == Tutorial.CUnitSelect || Type == Tutorial.CMove || Type == Tutorial.CChangeWeapon ? true : false);
                    }
                }
            }
        }
    }

    internal void ReturnToDefault()
    {
        CurrentUnitNum = 1;
        if (AvailableUnits.Count > 1)
        {
            AvailableUnits.RemoveRange(1, AvailableUnits.Count - 1);
        }

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
