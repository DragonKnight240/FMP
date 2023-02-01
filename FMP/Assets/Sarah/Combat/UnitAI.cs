using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAI : UnitBase
{
    UnitBase InRangeTarget;

    override public void Update()
    {
        base.Update();
    }

    public void MoveUnit()
    {
        if (!CanAttack())
        {
            int RandLocation = Random.Range(0, MoveableTiles.Count - 1);
            Move(MoveableTiles[RandLocation]);
        }
        else
        {
            Attack(InRangeTarget);
        }

        EndTurn = true;
    }

    bool CanAttack()
    {
        foreach(Tile tile in AttackTiles)
        {
            if(tile.Unit)
            {
                if(tile.Unit.CompareTag("Ally"))
                {
                    InRangeTarget = tile.Unit;
                    return true;
                }
            }
        }

        return false;
    }
}
