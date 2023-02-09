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
    internal List<GameObject> AllyUnits;
    internal List<GameObject> EnemyUnits;
    internal List<UnitBase> DeadEnemyUnits;
    internal List<UnitBase> DeadAllyUnits;
    public string OverWorldScene;
    internal bool SetupFinished = false;
    internal UnityEvent UnitUpdate;
    internal GameObject EnemyMoving;

    // Start is called before the first frame update
    void Start()
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
                EndingCombat();
            }
            else if (DeadAllyUnits.Count == AllyUnits.Count)
            {
                //lose
                print("Lose");
                RestartCombat();
            }
        }
    }

    void AIUnitMove()
    {
        foreach (GameObject Enemy in EnemyUnits)
        {
            if(Enemy.GetComponent<UnitAI>().Moving)
            {
                break;
            }

            if (!Enemy.GetComponent<UnitAI>().MovedForTurn && Enemy.GetComponent<UnitAI>().isAlive)
            {
                Enemy.GetComponent<UnitAI>().MoveUnit();
                EnemyMoving = Enemy;
                break;
            }
        }
    }

    public void PlaceUnits()
    {
        int X;
        int Y;

        int Index = 0;

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

            if (Position.UnitType == UnitType.Ally)
            {
                if (Index <= GameManager.Instance.MaxUnits + GameManager.Instance.NumRecruited)
                {
                    NewUnit = Instantiate(GameManager.Instance.ControlledUnits[Index], TileManager.Instance.Grid[X, Y].GetComponent<Tile>().CentrePoint.transform.position, Quaternion.identity, transform);
                    Index++;
                    AllyUnits.Add(NewUnit);
                    TurnManager.Instance.TurnChange.AddListener(NewUnit.GetComponent<UnitBase>().TurnChange);

                    if (GameManager.Instance.UnitData.Count > 0 && Index < GameManager.Instance.UnitData.Count)
                    {
                        CharacterData data = GameManager.Instance.UnitData[Index];

                        NewUnit.GetComponent<UnitBase>().UnitName = data.UnitName;
                        NewUnit.GetComponent<UnitBase>().HealthMax = data.HealthMax;
                        NewUnit.GetComponent<UnitBase>().CurrentHealth = data.CurrentHealth;
                        NewUnit.GetComponent<UnitBase>().Movement = data.Movement;

                        //Inventory
                        NewUnit.GetComponent<UnitBase>().Inventory = data.Inventory;

                        //Stats
                        NewUnit.GetComponent<UnitBase>().Strength = data.Strength;
                        NewUnit.GetComponent<UnitBase>().Dexterity = data.Dexterity;
                        NewUnit.GetComponent<UnitBase>().Magic = data.Magic;
                        NewUnit.GetComponent<UnitBase>().Defence = data.Defence;
                        NewUnit.GetComponent<UnitBase>().Resistance = data.Resistance;
                        NewUnit.GetComponent<UnitBase>().Speed = data.Speed;
                        NewUnit.GetComponent<UnitBase>().Luck = data.Luck;

                        //Weapon Proficientcy
                        NewUnit.GetComponent<UnitBase>().BowProficiency = data.BowProficiency;
                        NewUnit.GetComponent<UnitBase>().BowLevel = data.BowLevel;

                        NewUnit.GetComponent<UnitBase>().SwordProficiency = data.SwordProficiency;
                        NewUnit.GetComponent<UnitBase>().SwordLevel = data.SwordLevel;

                        NewUnit.GetComponent<UnitBase>().MagicProficiency = data.MagicProficiency;
                        NewUnit.GetComponent<UnitBase>().MagicLevel = data.MagicLevel;

                        NewUnit.GetComponent<UnitBase>().FistProficiency = data.FistProficiency;
                        NewUnit.GetComponent<UnitBase>().FistLevel = data.FistLevel;

                        //Class
                        NewUnit.GetComponent<UnitBase>().Class = data.Class;

                        //Attack
                        NewUnit.GetComponent<UnitBase>().UnlockedAttacks = data.UnlockedAttacks;
                    }
                }
                else
                {
                    print("Skip Unit");
                    continue;
                }
            }
            else
            {
                NewUnit = Instantiate(Position.Unit, TileManager.Instance.Grid[X, Y].GetComponent<Tile>().CentrePoint.transform.position, Quaternion.Euler(0,180,0), transform);
                EnemyUnits.Add(NewUnit);
                TurnManager.Instance.TurnChange.AddListener(NewUnit.GetComponent<UnitAI>().TurnChange);
            }

            foreach (Item item in NewUnit.GetComponent<UnitBase>().WeaponsIninventory)
            {
                NewUnit.GetComponent<UnitBase>().Inventory.Add((Weapon)item);
            }

            UnitUpdate.AddListener(() => { NewUnit.GetComponent<UnitBase>().MoveableArea(false); });
            NewUnit.GetComponent<UnitBase>().Position = new int[2];
            NewUnit.GetComponent<UnitBase>().Position[0] = X;
            NewUnit.GetComponent<UnitBase>().Position[1] = Y;
            TileManager.Instance.Grid[X, Y].GetComponent<Tile>().ChangeOccupant(NewUnit.GetComponent<UnitBase>());
        }

       TurnManager.Instance.TurnChange.AddListener(Interact.Instance.ResetTargets);

       SetupFinished = true;
    }

    internal void RestartCombat()
    {
        SceneLoader.Instance.ReloadScene();
    }

    internal void EndingCombat()
    {
        GameManager.Instance.UnitData.Clear();

        CharacterData data = new CharacterData();

        for (int i = 0; i < AllyUnits.Count; i++)
        {
            UnitBase Ally = AllyUnits[i].GetComponent<UnitBase>();

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

        SceneLoader.Instance.LoadNewScene(OverWorldScene);
    }
}
