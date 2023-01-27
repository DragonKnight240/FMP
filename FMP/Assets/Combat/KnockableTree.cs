using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockableTree : InteractOnGrid
{
    public int SwordLevelMin;


    // Start is called before the first frame update
    void Start()
    {
        //Position = new int[2];

        //ActiveTiles = new List<GameObject>();
        //ActiveTiles = TileManager.Instance.Grid[Position[0], Position[1]].GetComponent<Tile>().AdjacentTiles;

        //AoETiles = new List<GameObject>();
        //AoETiles
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Special(UnitBase Unit)
    {
        if(Unit.SwordLevel >= SwordLevelMin)
        {

        }

        TileManager.Instance.Grid[Position[0], Position[1]].GetComponent<Tile>().CanMoveOn = true;
        TileManager.Instance.Grid[Position[0], Position[1]].GetComponent<Tile>().Occupied = false;
        Unit.EndTurn = true;
        Destroy(this.gameObject);

    }

    internal override void CalculateAoE()
    {
        int CheckPositionX = Position[0];
        int CheckPositionY = Position[0];

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
}
