using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnitManager : MonoBehaviour
{
    public enum UnitType
    {
        Ally,
        Enemy
    }

    [System.Serializable]
    public struct StartingPositions
    {
        public UnitType UnitType;
        public GameObject Unit;
        public int X;
        public int Y;
    }

    public static UnitManager Instance;
    public List<StartingPositions> UnitPositions;
    public List<GameObject> AllyUnits;
    internal List<GameObject> EnemyUnits;
    internal List<UnitBase> DeadEnemyUnits;
    internal List<UnitBase> DeadAllyUnits;
    public string OverWorldScene;
    internal bool SetupFinished = false;
    internal UnityEvent UnitUpdate;
    internal GameObject EnemyMoving;
    internal List<UnitAI> PendingEnemies;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        AllyUnits = new List<GameObject>();
        EnemyUnits = new List<GameObject>();
        DeadEnemyUnits = new List<UnitBase>();
        DeadAllyUnits = new List<UnitBase>();
        UnitUpdate = new UnityEvent();
        PendingEnemies = new List<UnitAI>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    void print()
    {
        print("Played");
    }

    private void Update()
    {
        if (!TurnManager.Instance.isPlayerTurn)
        {
            AIUnitMove();
        }

        if (SetupFinished)
        {
            if (DeadEnemyUnits.Count == EnemyUnits.Count)
            {
                //win
                print("Win");
                Interact.Instance.CombatMenu.DisplayVictoryScreen();
            }
            else if (DeadAllyUnits.Count == AllyUnits.Count)
            {
                //lose
                print("Lose");
                Interact.Instance.CombatMenu.DisplayDefeatScreen();
            }
        }
    }

    void AIUnitMove()
    {
        PendingEnemies.Clear();
        PendingEnemies = new List<UnitAI>();

        foreach (GameObject Enemy in EnemyUnits)
        {
            if(Enemy.GetComponent<UnitBase>().Moving || Interact.Instance.VirtualCam.transform.position != Interact.Instance.transform.position)
            {
                break;
            }

            if (!Enemy.GetComponent<UnitBase>().MovedForTurn && Enemy.GetComponent<UnitBase>().isAlive)
            {
                if (Enemy.GetComponent<ScriptedUnit>())
                {
                    Enemy.GetComponent<ScriptedUnit>().FollowScript();
                }
                else
                {
                    if (Enemy.GetComponent<UnitAI>())
                    {
                        Enemy.GetComponent<UnitAI>().PerformAction();
                        EnemyMoving = Enemy;
                    }
                    else
                    {
                        Enemy.GetComponent<UnitBase>().WaitUnit();
                    }
                    break;
                }
            }
        }

        if(PendingEnemies.Count > 0)
        {
            PendingEnemies[0].NOKAV();

            foreach(UnitAI Unit in PendingEnemies)
            {
                if (Unit.CanAttack())
                {
                    if (Unit.InRangeTargets.Contains(Unit))
                    {
                        Unit.Attack(Unit.AttackTarget);
                    }
                    else
                    {
                        Unit.MoveAsCloseTo(TileManager.Instance.Grid[Unit.AttackTarget.Position[0], Unit.AttackTarget.Position[1]].GetComponent<Tile>());
                    }
                }
                else
                {
                    Unit.MoveAsCloseTo(TileManager.Instance.Grid[Unit.AttackTarget.Position[0], Unit.AttackTarget.Position[1]].GetComponent<Tile>());
                }

                Unit.WaitUnit();
                EnemyMoving = Unit.gameObject;
            }
        }
    }

    public void NoOverKill()
    {

    }

    public void PlaceUnits()
    {
        int X;
        int Y;

        int Index = 0;
        CharacterData data;

        foreach (StartingPositions Position in UnitPositions)
        {
            X = Position.X;
            Y = Position.Y;

            if (X > TileManager.Instance.Width)
            {
                X = TileManager.Instance.Width;
            }
            else if (X < 0)
            {
                X = 0;
            }

            if (Y > TileManager.Instance.Height)
            {
                Y = TileManager.Instance.Height;
            }
            else if (Y < 0)
            {
                Y = 0;
            }

            GameObject NewUnit;
            UnitBase UnitBase;

            if (Position.UnitType == UnitType.Ally)
            {
                if (Index < GameManager.Instance.CurrentUnitNum + GameManager.Instance.NumRecruited)
                {
                    NewUnit = Instantiate(GameManager.Instance.AvailableUnits[Index], TileManager.Instance.Grid[X, Y].GetComponent<Tile>().CentrePoint.transform.position, Quaternion.identity, transform);
                    UnitBase = NewUnit.GetComponent<UnitBase>();
                    AllyUnits.Add(NewUnit);
                    TurnManager.Instance.TurnChange.AddListener(UnitBase.TurnChange);

                    if (GameManager.Instance.UnitData.Count > 0 && Index < GameManager.Instance.UnitData.Count)
                    {
                        print("Input data");
                        data = GameManager.Instance.UnitData[Index];

                        NewUnit.name = data.UnitName;
                        UnitBase.UnitName = data.UnitName;
                        UnitBase.HealthMax = data.HealthMax;
                        UnitBase.CurrentHealth = data.CurrentHealth;
                        UnitBase.Movement = data.Movement;

                        //Inventory
                        UnitBase.Inventory.Clear();
                        UnitBase.Inventory = data.Inventory;

                        //Stats
                        UnitBase.Strength = data.Strength;
                        UnitBase.Dexterity = data.Dexterity;
                        UnitBase.Magic = data.Magic;
                        UnitBase.Defence = data.Defence;
                        UnitBase.Resistance = data.Resistance;
                        UnitBase.Speed = data.Speed;
                        UnitBase.Luck = data.Luck;

                        //Weapon Proficientcy
                        UnitBase.BowProficiency = data.BowProficiency;
                        UnitBase.BowLevel = data.BowLevel;

                        UnitBase.SwordProficiency = data.SwordProficiency;
                        UnitBase.SwordLevel = data.SwordLevel;

                        UnitBase.MagicProficiency = data.MagicProficiency;
                        UnitBase.MagicLevel = data.MagicLevel;

                        UnitBase.FistProficiency = data.FistProficiency;
                        UnitBase.FistLevel = data.FistLevel;

                        //Class
                        UnitBase.Class = data.Class;

                        //Attack
                        UnitBase.UnlockedAttacks.Clear();
                        UnitBase.UnlockedAttacks = data.UnlockedAttacks;

                        if (UnitBase.CurrentHealth <= 0)
                        {
                            NewUnit.SetActive(false);
                            UnitBase.isAlive = false;
                        }

                        data = new CharacterData();
                    }
                    else
                    {
                        NewUnit.GetComponent<UnitBase>().CurrentHealth = NewUnit.GetComponent<UnitBase>().HealthMax;

                        NewUnit.GetComponent<UnitBase>().Class = Instantiate(NewUnit.GetComponent<UnitBase>().Class);

                        NewUnit.GetComponent<UnitBase>().Class.FindLevel();
                        NewUnit.GetComponent<UnitBase>().Class.AbilityUnlock(NewUnit.GetComponent<UnitBase>());
                    }

                    Index++;
                }
                else
                {
                    print("Skip Unit");
                    Index++;
                    continue;
                }
            }
            else
            {
                NewUnit = Instantiate(Position.Unit, TileManager.Instance.Grid[X, Y].GetComponent<Tile>().CentrePoint.transform.position, Quaternion.Euler(0, 180, 0), transform);
                UnitBase = NewUnit.GetComponent<UnitBase>();
                EnemyUnits.Add(NewUnit);
                TurnManager.Instance.TurnChange.AddListener(NewUnit.GetComponent<UnitBase>().TurnChange);

                NewUnit.GetComponent<UnitBase>().CurrentHealth = NewUnit.GetComponent<UnitBase>().HealthMax;

                if (NewUnit.GetComponent<UnitBase>().Class != null)
                {
                    NewUnit.GetComponent<UnitBase>().Class = Instantiate(NewUnit.GetComponent<UnitBase>().Class);

                    NewUnit.GetComponent<UnitBase>().Class.FindLevel();
                    NewUnit.GetComponent<UnitBase>().Class.AbilityUnlock(NewUnit.GetComponent<UnitBase>());
                }
            }

            UnitBase.AvailableAttacks = new List<SpecialAttacks>();

            foreach (Item item in UnitBase.WeaponsIninventory)
            {
                Weapon weapon = (Weapon)item;
                UnitBase.Inventory.Add(weapon);
                if(weapon.Special)
                {
                    if(!UnitBase.UnlockedAttacks.Contains(weapon.Special))
                    {
                        UnitBase.UnlockedAttacks.Add(weapon.Special);

                        if(UnitBase.EquipedWeapon.WeaponType == weapon.Special.WeaponType)
                        {
                            UnitBase.AvailableAttacks.Add(weapon.Special);
                        }
                    }
                }
            }

            foreach (SpecialAttacks Attack in UnitBase.UnlockedAttacks)
            {
                if(UnitBase.AvailableAttacks.Contains(Attack))
                {
                    continue;
                }

                if (UnitBase.EquipedWeapon.WeaponType == Attack.WeaponType)
                {
                    UnitBase.AvailableAttacks.Add(Attack);
                }
            }

            UnitBase.CurrentAttack = UnitBase.AvailableAttacks[0];

            if (UnitBase.WeaponsIninventory.Contains(UnitBase.BareHands))
            {
                UnitBase.WeaponsIninventory.Add(UnitBase.BareHands);
            }

            UnitUpdate.AddListener(() => { UnitBase.MoveableArea(false); });
            UnitBase.Position = new int[2];
            UnitBase.Position[0] = X;
            UnitBase.Position[1] = Y;
            TileManager.Instance.Grid[X, Y].GetComponent<Tile>().ChangeOccupant(UnitBase);

            UnitBase.UIHealth.maxValue = UnitBase.HealthMax;
            UnitBase.UIHealth.value = UnitBase.CurrentHealth;
        }

        TurnManager.Instance.TurnChange.AddListener(Interact.Instance.ResetTargets);
        TurnManager.Instance.UnitsToMove = AllyUnits.Count;

        TurnManager.Instance.Orbs = FindObjectsOfType<MagicOrb>();

        SetupFinished = true;

        GameManager.Instance.NextToolTip();
    }

    public void EndingCombat()
    {
        GameManager.Instance.UnitData.Clear();

        CharacterData data = new CharacterData();
        UnitBase Ally;

        for (int i = 0; i < AllyUnits.Count; i++)
        {
            data = new CharacterData();
            Ally = AllyUnits[i].GetComponent<UnitBase>();

            data.UnitName = Ally.UnitName;
            data.HealthMax = Ally. HealthMax;
            data.CurrentHealth = Ally.CurrentHealth;
            data.Movement = Ally.Movement;

            //Inventory
            data.Inventory = Ally.Inventory;

            //Stats
            data.Strength = Ally.Strength;
            data.Dexterity = Ally.Dexterity;
            data.Magic = Ally.Magic;
            data.Defence = Ally.Defence;
            data.Resistance = Ally.Resistance;
            data.Speed = Ally.Speed;
            data.Luck = Ally.Luck;

            //Weapon Proficientcy
            data.BowProficiency = Ally.BowProficiency;
            data.BowLevel = Ally.BowLevel;

            data.SwordProficiency = Ally.SwordProficiency;
            data.SwordLevel = Ally.SwordLevel;

            data.MagicProficiency = Ally.MagicProficiency;
            data.MagicLevel = Ally.MagicLevel;

            data.FistProficiency = Ally.FistProficiency;
            data.FistLevel = Ally.FistLevel;

            //Class
            data.Class = Ally.Class;

            //Attack
            data.UnlockedAttacks = Ally.UnlockedAttacks;

            GameManager.Instance.UnitData.Add(data);
        }

        OnCombatEndDialogue CombatScript = FindObjectOfType<OnCombatEndDialogue>();
        if(CombatScript)
        {
            CombatScript.SetDialogue();
        }

        if(!GameManager.Instance.CombatTutorialComplete)
        {
            GameManager.Instance.CurrentUnitNum = 2;
            GameManager.Instance.RecruitUnit("Magic");
            GameManager.Instance.CombatTutorialComplete = true;
        }

        GameManager.Instance.inCombat = false;

        SceneLoader.Instance.LoadNewScene(OverWorldScene);
    }
}
