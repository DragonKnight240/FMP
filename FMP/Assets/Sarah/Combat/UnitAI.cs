using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AttackStats
{
    public UnitBase AttackingUnit;
    public Dictionary<UnitBase, int> TargetUnit;
    public float AttackValue;
}


public class UnitAI : UnitBase
{
    public enum BehaviourTypes
    {
        NOKAV,
        AttackValue,
        Patient,
        Inpatient
    }

    UnitBase InRangeTarget;
    public BehaviourTypes Behaviour;
    internal List<AttackStats> AttackProfiles;
    internal AttackStats CurrentAttackStats;

    override internal void Start()
    {
        base.Start();
        AttackProfiles = new List<AttackStats>();
    }


    override public void Update()
    {
        base.Update();
        
        if(!Moving)
        {
            if(UnitManager.Instance.EnemyMoving == this)
            {
                UnitManager.Instance.EnemyMoving = null;
            }
        }
        else
        {
            CameraMove.Instance.FollowTarget = transform;
        }
    }

    internal void MoveAsCloseTo(Tile TargetTile)
    {
        MovedForTurn = true;

        Path = new List<Tile>(FindRouteTo(TargetTile));

        int Index = 0;

        foreach(Tile tile in Path)
        {
            if(MoveableTiles.Contains(tile))
            {
                Index++;
                continue;
            }

            break;
        }

        Path.RemoveRange(Index, Path.Count - Index);

        Moving = true;

        if (Path.Count > 0)
        {
            Position[0] = Path[Path.Count - 1].GridPosition[0];
            Position[1] = Path[Path.Count - 1].GridPosition[1];
        }

        if (Path.Count > 0)
        {
            Path[Path.Count - 1].ChangeOccupant(this);
        }

        ResetMoveableTiles();
        UnitManager.Instance.UnitUpdate.Invoke();
        List<GameObject> Tile = new List<GameObject>();
        Tile.Add(TileManager.Instance.Grid[Position[0], Position[1]].gameObject);
    }

    public void PerformAction()
    {
        switch(Behaviour)
        {
            case BehaviourTypes.NOKAV:
                {
                    AttackValue();
                    UnitManager.Instance.PendingEnemies.Add(this);
                    break;
                }
            case BehaviourTypes.AttackValue:
                {
                    AttackValue();
                    UnitManager.Instance.PendingEnemies.Add(this);
                    break;
                }
            case BehaviourTypes.Patient:
                {
                    Patient();
                    WaitUnit();
                    break;
                }
            case BehaviourTypes.Inpatient:
                {
                    Inpatient();
                    WaitUnit();
                    break;
                }

        }
    }

    //Ignores possible move locations
    internal void MoveAnywhere(Tile Target)
    {
        Move(Target, false, true);
        UnitManager.Instance.EnemyMoving = gameObject;
    }

    //Finds a random location to move to
    internal void RandomMovement()
    {
        int RandLocation;
        do
        {
            RandLocation = Random.Range(0, MoveableTiles.Count - 1);
        } while (Move(MoveableTiles[RandLocation]));
    }

    internal bool CanAttack()
    {
        InRangeTargets.Clear();
        InRangeTargets = new List<UnitBase>();

        foreach(Tile tile in AttackTiles)
        {
            if(tile.Unit)
            {
                if(tile.Unit.CompareTag("Ally"))
                {
                    InRangeTargets.Add(tile.Unit);
                }
            }
        }

        if(InRangeTargets.Count > 0)
        {
            return true;
        }

        return false;
    }

    //Compares current attack target to next unit
    public void FindLowestDefence(UnitBase Unit)
    {
        if (Unit.isAlive)
        {
            if (AttackTarget == null)
            {
                AttackTarget = Unit;
            }

            if (EquipedWeapon.WeaponType == WeaponType.Staff)
            {
                if (AttackTarget.CalculateMagicDefence(WeaponType.Staff) > Unit.CalculateMagicDefence(WeaponType.Staff))
                {
                    AttackTarget = Unit;
                }
            }
            else
            {
                if (AttackTarget.CalculatePhysicalDefence(EquipedWeapon.WeaponType) > Unit.CalculatePhysicalDefence(EquipedWeapon.WeaponType))
                {
                    AttackTarget = Unit;
                }
            }
        }
    }

    //Attacks lowest defence target if in range otherwise does nothing
    internal void Patient()
    {
        if(CanAttack())
        {
            print("Can Attack");
            foreach(UnitBase Target in InRangeTargets)
            {
                FindLowestDefence(Target);
            }
        }

        if(AttackTarget != null)
        {
            Attack(AttackTarget);
        }
    }

    //Attacks lowest defence (to equiped weapon)  and sets that as the target
    internal void Inpatient()
    {
        UnitBase Unit;
        foreach (GameObject UnitObject in UnitManager.Instance.AllyUnits)
        {
            Unit = UnitObject.GetComponent<UnitBase>();
            FindLowestDefence(Unit);
        }

        if (AttackTarget != null)
        {
            FindInRangeTargets();
            if (InRangeTargets.Contains(AttackTarget))
            {
                Attack(AttackTarget);
            }
            else
            {
                MoveAsCloseTo(TileManager.Instance.Grid[AttackTarget.Position[0], AttackTarget.Position[1]].GetComponent<Tile>());
            }

        }
        else
        {
            RandomMovement();
        }
    }

    //Attack Value 
    internal void AttackValue()
    {
        UnitBase Unit;
        AttackStats Stats;
        int TopAttackValue = 0;

        AttackProfiles.Clear();
        AttackProfiles = new List<AttackStats>();

        foreach (GameObject UnitObject in UnitManager.Instance.AllyUnits)
        {
            Stats = new AttackStats();
            Stats.TargetUnit = new Dictionary<UnitBase, int>();

            Unit = UnitObject.GetComponent<UnitBase>();
            if (Unit.isAlive)
            {
                Stats.TargetUnit.Add(Unit, CalculateDamage(Unit));
                Stats.AttackingUnit = this;
                Stats.AttackValue = (float)CalculateDamage(Unit) / (float)Unit.CurrentHealth;

                if (Stats.TargetUnit[Unit] > TopAttackValue)
                {
                    TopAttackValue = Stats.TargetUnit[Unit];
                    AttackTarget = Unit;
                    CurrentAttackStats = Stats;
                }

                print(Unit + " - " + Stats.AttackValue);

                AttackProfiles.Add(Stats);
            }
        }

        AttackProfiles.Sort((x, y) => x.AttackValue.CompareTo(y.AttackValue));

        foreach(AttackStats Stat in AttackProfiles)
        {
            print(Stat.AttackValue);
        }
    }

    //No over kill - Attack Value
    internal void NOKAV()
    {
        UnitAI Unit;
        bool NoChanges = true;

        if(UnitManager.Instance.DeadAllyUnits.Count == UnitManager.Instance.AllyUnits.Count -1)
        {
            return;
        }

        do
        {
            NoChanges = false;
            foreach (GameObject UnitObject in UnitManager.Instance.EnemyUnits)
            {
                Unit = GetComponent<UnitAI>();

                if (Unit.AttackTarget == AttackTarget)
                {
                    if ((AttackTarget.CurrentHealth - CurrentAttackStats.TargetUnit[AttackTarget] - Unit.CurrentAttackStats.TargetUnit[AttackTarget]) > 0)
                    {
                        if (CurrentAttackStats.AttackValue > Unit.CurrentAttackStats.AttackValue)
                        {
                            if (AttackProfiles.Count > 1)
                            {
                                AttackProfiles.RemoveAt(0);
                                CurrentAttackStats = AttackProfiles[0];
                                NoChanges = true;
                            }
                        }
                    }
                }
            }
        } while (NoChanges);
    }
}
