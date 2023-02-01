using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockableTree : InteractOnGrid
{
    public int SwordLevelMin;
    internal int damage = 2;


    // Start is called before the first frame update
    void Start()
    {
        AoETiles = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Special(UnitBase Unit)
    {
        if(Unit.SwordLevel >= SwordLevelMin)
        {
            CalculateAoE();
            //gameObject.transform.Rotate()
            DealAoEDamage();
            
        }

        TileManager.Instance.Grid[Position[0], Position[1]].GetComponent<Tile>().CanMoveOn = true;
        TileManager.Instance.Grid[Position[0], Position[1]].GetComponent<Tile>().Occupied = false;
        Unit.EndTurn = true;
        Destroy(this.gameObject);

    }

    internal override void CalculateAoE()
    {
        int CheckPositionX = Position[0];
        int CheckPositionY = Position[1];

        switch (DirectionInteraction)
        {
            case Direction.Up:
                {
                    for (int i = 0; i < HitAmountHeight - 1; i++)
                    {
                        CheckPositionY++;
                        AoETiles.Add(TileManager.Instance.Grid[CheckPositionX, CheckPositionY]);
                    }
                    break;
                }
            case Direction.Down:
                {
                    for (int i = 0; i < HitAmountHeight - 1; i++)
                    {
                        CheckPositionY--;
                        AoETiles.Add(TileManager.Instance.Grid[CheckPositionX, CheckPositionY]);
                    }

                    break;
                }
            case Direction.Left:
                {
                    for (int i = 0; i < HitAmountHeight - 1; i++)
                    {
                        CheckPositionX--;
                        AoETiles.Add(TileManager.Instance.Grid[CheckPositionX, CheckPositionY]);
                    }
                    break;
                }
            case Direction.Right:
                {
                    for (int i = 0; i < HitAmountHeight - 1; i++)
                    {
                        CheckPositionX++;
                        AoETiles.Add(TileManager.Instance.Grid[CheckPositionX, CheckPositionY]);
                    }
                    break;
                }
        }
    }

    internal void DealAoEDamage()
    {
        foreach (GameObject tile in AoETiles)
        {
            if(tile.GetComponent<Tile>().Unit)
            {
                tile.GetComponent<Tile>().Unit.DecreaseHealth(damage);
            }
        }
    }
}
