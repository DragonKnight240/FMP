using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockableTree : InteractOnGrid
{
    public int SwordLevelMin;
    internal int damage = 2;
    internal bool isFalling = false;
    internal Quaternion RotateTo;
    public float Speed = 50;


    // Start is called before the first frame update
    void Start()
    {
        AoETiles = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isFalling)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, RotateTo, Time.deltaTime * Speed);

            if(transform.rotation == RotateTo)
            {
                isFalling = false;
                Destroy(this.gameObject, 5);
            }
        }
    }

    public override void Special(UnitBase Unit)
    {
        if(Unit.SwordLevel >= SwordLevelMin)
        {
            //Tile tile = TileManager.Instance.Grid[Unit.Position[0], Unit.Position[1]].GetComponent<Tile>();

            print(InteractLocations.Count);
            CalculateAoE(InteractLocations[TileManager.Instance.Grid[Unit.Position[0], Unit.Position[1]].GetComponent<Tile>()]);
            DealAoEDamage();
            
        }

        TileManager.Instance.Grid[Position[0], Position[1]].GetComponent<Tile>().CanMoveOn = true;
        TileManager.Instance.Grid[Position[0], Position[1]].GetComponent<Tile>().Occupied = false;
        Unit.EndTurn = true;
    }

    internal override void CalculateAoE(Direction DirectionInteraction)
    {
        int CheckPositionX = Position[0];
        int CheckPositionY = Position[1];

        switch (DirectionInteraction)
        {
            case Direction.Up:
                {
                    //Down
                    for (int i = 0; i < HitAmountHeight - 1; i++)
                    {
                        CheckPositionY--;
                        if (CheckPositionY < TileManager.Instance.Height - 1 && CheckPositionY >= 0)
                        {
                            AoETiles.Add(TileManager.Instance.Grid[CheckPositionX, CheckPositionY]);
                        }
                    }
                    RotateTo = Quaternion.Euler(-90, transform.rotation.y, transform.rotation.z);
                    isFalling = true;
                    //Rotate x -
                    break;
                }
            case Direction.Down:
                {
                    //Up
                    for (int i = 0; i < HitAmountHeight - 1; i++)
                    {
                        CheckPositionY++;
                        if (CheckPositionY < TileManager.Instance.Height - 1 && CheckPositionY >= 0)
                        {
                            AoETiles.Add(TileManager.Instance.Grid[CheckPositionX, CheckPositionY]);
                        }

                    }
                    RotateTo = Quaternion.Euler(90, transform.rotation.y, transform.rotation.z);
                    isFalling = true;
                    //Rotate x +
                    break;
                }
            case Direction.Left:
                {
                    //Right
                    for (int i = 0; i < HitAmountHeight - 1; i++)
                    {
                        CheckPositionX++;
                        if (CheckPositionX < TileManager.Instance.Width - 1 && CheckPositionX >= 0)
                        {
                            AoETiles.Add(TileManager.Instance.Grid[CheckPositionX, CheckPositionY]);
                        }
                    }
                    RotateTo = Quaternion.Euler(transform.rotation.x, transform.rotation.y, -90);
                    isFalling = true;
                    //Rotate z -
                    break;
                }
            case Direction.Right:
                {
                    //Left
                    for (int i = 0; i < HitAmountHeight - 1; i++)
                    {
                        CheckPositionX--;
                        if (CheckPositionX < TileManager.Instance.Width - 1 && CheckPositionX >= 0)
                        {
                            AoETiles.Add(TileManager.Instance.Grid[CheckPositionX, CheckPositionY]);
                        }
                    }
                    RotateTo = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 90);
                    isFalling = true;
                    //Rotate z +
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
