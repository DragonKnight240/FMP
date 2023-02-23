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

[System.Serializable]
public struct ScritpedActions
{
    public Actions Action;
    public List<int> Location;
}

public class ScriptedUnit : MonoBehaviour
{
    public List<ScritpedActions> ScriptedActions;
    int CurrentIndex = 0;
    UnitBase Unit;

    private void Start()
    {
        Unit = GetComponent<UnitBase>();
    }

    private void Update()
    {
        if(CurrentIndex >= ScriptedActions.Count)
        {
            Destroy(this);
        }
    }

    internal void FollowScript()
    {
        switch(ScriptedActions[CurrentIndex].Action)
        {
            case Actions.Move:
                {
                    Unit.Move(TileManager.Instance.Grid[ScriptedActions[CurrentIndex].Location[0], ScriptedActions[CurrentIndex].Location[1]].GetComponent<Tile>());
                    Unit.WaitUnit();
                    CurrentIndex++;
                    break;
                }
            case Actions.Attack:
                {
                    Unit.Attack(UnitManager.Instance.AllyUnits[0].GetComponent<UnitBase>());
                    CurrentIndex++;
                    break;
                }
            case Actions.Item:
                {
                    Unit.WaitUnit();
                    CurrentIndex++;
                    break;
                }
            case Actions.Special:
                {
                    Unit.GetComponent<UnitControlled>().SpecialButton();
                    CurrentIndex++;
                    break;
                }
            case Actions.Stationary:
                {
                    Unit.WaitUnit();
                    CurrentIndex++;
                    break;
                }
            default:
                {
                    Unit.WaitUnit();
                    CurrentIndex++;
                    break;
                }
        }
    }
}
