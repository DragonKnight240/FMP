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
    internal int CurrentUnitNum = 0;
    internal int NumRecruited = 0;
    public int MaxRecruitable = 2;
    public List<Item> Convoy;
    public int Money = 0;

    internal bool SwordInventoryFull = false;
    internal bool GauntletInventoryFull = false;
    internal bool MageInventoryFull = false;
    internal bool ArcherInventoryFull = false;

    internal Vector3 StartLocation;

    internal Dictionary<string, bool> TriggerDialogue = new Dictionary<string, bool>();

    public float PPCombatSpeed = 1;

    //Progress
    internal bool CombatTutorialComplete = false;
    internal bool OverworldTutorialComplete = false;
    internal bool OverworldInventoryComplete = false;
    internal bool OverworldMoveTutorialComplete = false;
    internal bool OverworldEnemyTutorialComplete = false;
    internal bool ArcherRecruitComplete = false;
    internal bool GauntletRecruitComplete = false;
    internal bool PostDungeon1Complete = false;
    internal bool PostDungeon2Complete = false;

    //DialogueToShow
    internal PlayAfter DialogueToPlay = PlayAfter.None;

    //Options
    //Sounds
    internal float MasterSlider = 0;
    internal float SFXSlider = 0;
    internal float MusicSlider = 0;
    internal float AmbianceSlider = 0;

    //Accessability
    internal bool KeepTurnUIUp = false;
    internal bool MouseControlsInvert = true;
    internal float MouseSensitivity = 100;


    //Timer
    public bool inCombat = false;
    public bool StartedGame = false;

    public bool GodMode = true;
    internal string CurrentInput = "";

    internal int MainObjectNum = 0;

    //Runes
    internal bool Rune1Active;
    internal bool Rune2Active;
    internal bool Rune3Active;

    //DesertPuzzle1Rune
    internal bool Rune1Desert1;
    internal bool Rune2Desert1;

    //DesertPuzzleRunes
    internal bool Rune1Desert2;
    internal bool Rune2Desert2;
    internal bool Rune3Desert2;

    [Header("Sound Effects")]
    public AudioClip OpenChestSound;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);

            UnitData = new List<CharacterData>();

            CurrentUnitNum = AvailableUnits.Count;

            if (FindObjectOfType<PlayerOverworld>())
            {
                StartLocation = FindObjectOfType<PlayerOverworld>().transform.position;
                PlayerReturnToOverworld = FindObjectOfType<PlayerOverworld>().transform.position;
                PlayerReturnRotation = FindObjectOfType<PlayerOverworld>().transform.rotation;
            }

            foreach (GameObject Unit in AvailableUnits)
            {
                if (Unit.name.Contains("Sword"))
                {
                    //print("Sword Already in");
                    return;
                }
            }

            //print("Recruiting Sword");
            RecruitUnit("Sword");
        }
        else
        {
            Destroy(gameObject);
        }

    }


    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {
        if (GodMode)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                //foreach (CharacterData Data in UnitData)
                //{
                //    print(Data.EXP);
                //}
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                if (UnitManager.Instance)
                {
                    UnitManager.Instance.EndingCombat();
                }
            }
            else if (Input.GetKeyDown(KeyCode.L))
            {
                if (UnitManager.Instance)
                {
                    foreach (GameObject Unit in UnitManager.Instance.AllyUnits)
                    {
                        Unit.GetComponent<UnitBase>().CurrentHealth = 1;
                    }
                }
            }
            else if(Input.GetKeyDown(KeyCode.H))
            {
                if (UnitManager.Instance)
                {
                    foreach (GameObject Unit in UnitManager.Instance.AllyUnits)
                    {
                        Unit.GetComponent<UnitBase>().CurrentHealth = Unit.GetComponent<UnitBase>().HealthMax;
                    }
                }
            }
            else if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                CombatTutorialComplete = true;
                OverworldMoveTutorialComplete = true;
                OverworldEnemyTutorialComplete = true;
                OverworldTutorialComplete = true;
            }
            else if(Input.GetKeyDown(KeyCode.Alpha2))
            {
                Money += 1000;
            }
            else if(Input.GetKeyDown(KeyCode.Alpha3))
            {
                if(UnitManager.Instance)
                {
                    foreach(GameObject Unit in UnitManager.Instance.EnemyUnits)
                    {
                        Unit.GetComponent<UnitBase>().CurrentHealth = 1;
                    }
                }
            }
        }
        else
        {
            if (Input.anyKeyDown)
            {
                if (Input.GetKeyDown(KeyCode.K) && CurrentInput == "" )
                {
                    CurrentInput += "K";
                }
                else if (Input.GetKeyDown(KeyCode.O) && CurrentInput == ("K"))
                {
                    CurrentInput += "O";
                }
                else if (Input.GetKeyDown(KeyCode.S) && CurrentInput == ("KO"))
                {
                    CurrentInput += "S";
                }
                else if (Input.GetKeyDown(KeyCode.T) && CurrentInput == ("KOS"))
                {
                    CurrentInput += "T";
                }
                else if (Input.GetKeyDown(KeyCode.A) && CurrentInput == ("KOST"))
                {
                    CurrentInput += "A";
                }
                else if (Input.GetKeyDown(KeyCode.S) && CurrentInput == ("KOSTA"))
                {
                    GodMode = !GodMode;
                    //print(GodMode);
                }
                else
                {
                    //print("RESET");
                    CurrentInput = "";
                }
            }
        }

        foreach(CharacterData Data in UnitData)
        {
            if(Data.Inventory.Count >= 6)
            {
                if (Data.Class.Name == "Soldier")
                {
                    SwordInventoryFull = true;
                }
                else if(Data.Class.Name == "Mage")
                {
                    MageInventoryFull = true;
                }
                else if (Data.Class.Name == "Grappler")
                {
                    GauntletInventoryFull = true;
                }
                else if (Data.Class.Name == "Archer")
                {
                    ArcherInventoryFull = true;
                }
            }
            else
            {
                if (Data.Class.Name == "Soldier")
                {
                    SwordInventoryFull = false;
                }
                else if (Data.Class.Name == "Mage")
                {
                    MageInventoryFull = false;
                }
                else if (Data.Class.Name == "Grappler")
                {
                    GauntletInventoryFull = false;
                }
                else if (Data.Class.Name == "Archer")
                {
                    ArcherInventoryFull = false;
                }
            }
        }
    }

    internal void PostProcessingComabt(PlayerOverworld Player, string NewScene)
    {
        //print(Player.PPVolume.weight);

        Player.PPVolume.weight = Mathf.Lerp(Player.PPVolume.weight, 1, PPCombatSpeed);
        Player.FreelookCam.m_Lens.FieldOfView = Mathf.Lerp(Player.FreelookCam.m_Lens.FieldOfView, 25, PPCombatSpeed);

        if(Player.PPVolume.weight >= 0.95)
        {
            SceneLoader.Instance.LoadNewScene(NewScene);
            //print("New Scene");
        }
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
                //print("Recruited");
                GameObject TempUnit = Instantiate(Unit, new Vector3(0, 0, 0), Quaternion.identity);
                TempUnit.SetActive(false);
                SetUpUnit(TempUnit.GetComponent<UnitBase>());
                AddCharacterData(TempUnit.GetComponent<UnitBase>());
                AvailableUnits.Add(Unit);
                GameManager.Instance.CurrentUnitNum++;

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

        data.Setup = Unit.Setup;

        //Inventory
        data.Inventory = Unit.Inventory;

        data.UnitImage = Unit.UnitImage;

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

        if (Unit.SupportsWith != null)
        {
            List<UnitSupports> SupportList = new List<UnitSupports>();

            foreach (UnitSupports Support in Unit.SupportsWith)
            {
                SupportList.Add(Instantiate(Support));
            }

            Unit.SupportsWith = SupportList;
        }

        Unit.AvailableAttacks = new List<SpecialAttacks>();

        List<Weapon> WeaponInventory = new List<Weapon>();
        List<Item> InventoryInstances = new List<Item>();

        foreach (Item item in Unit.Inventory)
        {
            //print(item.Name + " " + UnitBase.gameObject);

            if (item == null)
            {
                continue;
            }

            //print(item.Type);

            if (item.Type == ItemTypes.Weapon)
            {
                Weapon weapon = (Weapon)item;

                if (!Unit.Setup)
                {
                    InventoryInstances.Add(Instantiate(weapon));
                    WeaponInventory.Add((Weapon)InventoryInstances[InventoryInstances.Count - 1]);
                }
                else
                {
                    WeaponInventory.Add((Weapon)item);
                }

                WeaponInventory[WeaponInventory.Count - 1].CurrentDurablity = WeaponInventory[WeaponInventory.Count - 1].Durablity;

                if (weapon.Special)
                {
                    if (!Unit.UnlockedAttacks.Contains(weapon.Special))
                    {
                        Unit.UnlockedAttacks.Add(weapon.Special);

                        if (Unit.EquipedWeapon.WeaponType == weapon.Special.WeaponType)
                        {
                            Unit.AvailableAttacks.Add(weapon.Special);
                        }
                    }
                }
            }
            else if(!Unit.Setup)
            {
                InventoryInstances.Add(Instantiate(item));
            }
        }

        Unit.WeaponsIninventory = WeaponInventory;

        if(!Unit.Setup)
        {
            Unit.Inventory = InventoryInstances;

            Unit.Setup = true;
        }

        foreach (SpecialAttacks Attack in Unit.UnlockedAttacks)
        {
            if (Unit.AvailableAttacks.Contains(Attack))
            {
                continue;
            }

            if (Unit.EquipedWeapon.WeaponType == Attack.WeaponType)
            {
                Unit.AvailableAttacks.Add(Attack);
            }
        }

        Unit.CurrentAttack = Unit.AvailableAttacks[0];

        if (Unit.WeaponsIninventory.Contains(Unit.BareHands))
        {
            Unit.WeaponsIninventory.Add(Instantiate(Unit.BareHands));
            //Unit.WeaponsIninventory.Add(Unit.BareHands);
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
                        ToolTipManager.Instance.CompleteToolTip(Type == Tutorial.CUnitSelect|| Type == Tutorial.CMoveCamera || Type == Tutorial.CMove || Type == Tutorial.CChangeWeapon ? true : false);
                    }
                }
                else
                {
                    //print("Active: Checking " + Type + " - Current: " + ToolTipManager.Instance.Tooltips[ToolTipManager.Instance.CurrentToolTipIndex].tutorial);
                    if (Type == ToolTipManager.Instance.Tooltips[ToolTipManager.Instance.CurrentToolTipIndex].tutorial)
                    {
                        //print("Complete");
                        ToolTipManager.Instance.CompleteToolTip(Type == Tutorial.CUnitSelect || Type == Tutorial.CMoveCamera || Type == Tutorial.CMove || Type == Tutorial.CChangeWeapon ? true : false);
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
        OverworldMoveTutorialComplete = false;
        OverworldEnemyTutorialComplete = false;
        ArcherRecruitComplete = false;
        GauntletRecruitComplete = false;
        PostDungeon1Complete = false;
        PostDungeon2Complete = false;

        Rune1Active = false;
        Rune2Active = false;
        Rune3Active = false;

        DialogueToPlay = PlayAfter.None;

        Convoy.Clear();
        Convoy = new List<Item>();

        Money = 0;

        NumRecruited = 0;

        UnitData.Clear();
        UnitData = new List<CharacterData>();

        inCombat = false;
        StartedGame = false;

        PlayerReturnToOverworld = StartLocation;
    }
}
