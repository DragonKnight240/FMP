using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Actions
{
    Move,
    Attack,
    Item,
    Special,
    Stationary
}

public enum Triggers
{
    Health,
    EnemyUnitPosition,
    InRange,
    None
}

[System.Serializable]
public struct Commands
{
    public Actions Action;
    public Triggers Trigger;
    public List<int> Location;
    public float Amount;
}

public class ScriptedUnit : MonoBehaviour
{
    public List<Commands> ScriptedActions;
    int CurrentIndex = 0;
    UnitBase Unit;
    internal Commands NextAction;
    bool NoMove = false;

    private void Start()
    {
        Unit = GetComponent<UnitBase>();
    }

    private void Update()
    {
        if(Unit == null)
        {
            Unit = GetComponent<UnitBase>();
        }
    }

    internal void FollowScript()
    {

        NoMove = true;

        foreach(Commands Command in ScriptedActions)
        {
            switch(Command.Trigger)
            {
                case Triggers.EnemyUnitPosition:
                    {
                        if(UnitManager.Instance.AllyUnits[0].GetComponent<UnitBase>().Position[0] == Command.Location[0] ||
                            UnitManager.Instance.AllyUnits[0].GetComponent<UnitBase>().Position[1] == Command.Location[1])
                        {
                            NextAction = Command;
                            NoMove = false;
                        }

                        break;
                    }
                case Triggers.Health:
                    {
                        if (GetComponent<UnitBase>().CurrentHealth <= GetComponent<UnitBase>().HealthMax * Command.Amount)
                        {
                            NextAction = Command;
                            NoMove = false;
                        }
                        break;
                    }
                case Triggers.InRange:
                    {
                        if(GetComponent<UnitAI>().CanAttack())
                        {
                            NextAction = Command;
                            NoMove = false;
                        }

                        break;
                    }
                case Triggers.None:
                    {
                        NextAction = Command;
                        NoMove = false;
                        break;
                    }
                default:
                    {
                        NoMove = true;
                        break;
                    }
            }

            if(!NoMove)
            {
                //print(Command.Action);
                break;
            }
        }

        if(NoMove)
        {
            return;
        }

        switch(NextAction.Action)
        {
            case Actions.Move:
                {
                    Unit.GetComponent<UnitAI>().MoveAsCloseTo(TileManager.Instance.Grid[NextAction.Location[0], NextAction.Location[1]].GetComponent<Tile>());
                    Unit.WaitUnit();
                    break;
                }
            case Actions.Attack:
                {
                    Unit.Attack(UnitManager.Instance.AllyUnits[0].GetComponent<UnitBase>());
                    UnitManager.Instance.EnemyMoving = gameObject;
                    break;
                }
            case Actions.Item:
                {
                    Unit.WaitUnit();
                    break;
                }
            case Actions.Special:
                {
                    Unit.GetComponent<UnitControlled>().SpecialButton();
                    break;
                }
            case Actions.Stationary:
                {
                    Unit.WaitUnit();
                    break;
                }
            default:
                {
                    Unit.WaitUnit();
                    break;
                }
        }
    }
}
