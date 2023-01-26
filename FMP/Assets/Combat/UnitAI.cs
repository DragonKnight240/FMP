using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAI : UnitBase
{
    internal UnitBase EnemyInRange;

    override public void Update()
    {
        base.Update();
    }

    public void MoveUnit()
    {
        MoveableArea();
        if (!CanAttack())
        {
            int RandLocation = Random.Range(0, MoveableTiles.Count - 1);
            Move(MoveableTiles[RandLocation]);
        }
        else
        {
            Attack(EnemyInRange);
        }
    }

    bool CanAttack()
    {
        foreach(Tile tile in AttackTiles)
        {
            if(tile.Unit)
            {
                if(tile.Unit.CompareTag("Ally"))
                {
                    EnemyInRange = tile.Unit;
                    return true;
                }
            }
        }

        return false;
    }
}
