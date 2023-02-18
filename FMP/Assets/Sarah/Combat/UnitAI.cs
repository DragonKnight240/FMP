using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAI : UnitBase
{
    UnitBase InRangeTarget;

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

    public void MoveUnit()
    {
        if (!CanAttack())
        {
            int RandLocation;
            do
            {
              RandLocation = Random.Range(0, MoveableTiles.Count - 1);
            } while (Move(MoveableTiles[RandLocation]));
            
           ;
        }
        else
        {
            AttackTarget = InRangeTarget;
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
