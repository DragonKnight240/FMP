using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : UnitAI
{
    public bool isMultiTile = false;
    public int MutiTileAmount = 3;
    internal List<int[]> MultiPosition;
    public List<Tile> AoELocations;
    internal int TurnCount;
    internal int CurrentTurns;
    internal bool PendingAttack = false;
    internal bool PendingDamage = false;
    internal bool ToAoEAttack;
    internal bool ToTaunt = false;
    public AudioClip TauntSound;

    override public void Update()
    {
        base.Update();

        ReturnAttackPossible = false;

        if (PendingDamage && Interact.Instance.VirtualCam.activeInHierarchy)
        {
            AoEAttack();
        }

        if(!Moving && ToTaunt)
        {
            ZoomTaunt();
        }

        if(ToTaunt && AttackCamera.transform.position == Interact.Instance.transform.position)
        {
            ToTaunt = false;
            HideAllChangedTiles();

            PlayTauntAnim();
        }
    }

    public void ZoomTaunt()
    {
        AttackCamera.SetActive(true);
        Interact.Instance.VirtualCam.SetActive(false);
    }

    public void PlayTauntAnim()
    {
        AnimControl.ChangeAnim("Taunt", CombatAnimControl.AnimParameters.Attack);
        SoundManager.Instance.PlaySFX(TauntSound);

        ResetMoveableTiles();

        ToTaunt = false;
    }

    public void AoEAttack()
    {
        foreach (Tile tile in AoELocations)
        {
            if (tile.Unit)
            {
                if (tile.Unit != this)
                {
                    AttackTarget = tile.Unit;
                    tile.Unit.AttackTarget = this;
                    tile.Unit.ShowLongDistanceDamageNumbers(CalculateDamage());
                }
            }
        }

        PendingDamage = false;
    }

    internal void AoEPatient()
    {
        AttackTarget = null;

        if (PendingAttack)
        {
            IncreaseTurn();
        }
        else
        {
            if (CanAttack())
            {
                CheckCurrentRange();
            }

            if (AttackTarget != null)
            {
                MoveAsCloseTo(TileManager.Instance.Grid[AttackTarget.Position[0], AttackTarget.Position[1]].GetComponent<Tile>());
                AoEDirection(AttackTarget);

                ToTaunt = true;
            }
        }

        WaitUnit();
    }


    internal void IncreaseTurn()
    {
        CurrentTurns++;

        if(CurrentTurns >= TurnCount)
        {
            CurrentTurns = 0;
            PendingDamage = true;
            ToAttack = true;
        }
        else
        {
            ToTaunt = true;
        }
    }

    internal void AoEDirection(UnitBase Unit)
    {
        Dictionary<Tile, float> Tiles = new Dictionary<Tile, float>();
        Tile Closest = null;

        Tile CurrentTile;
        float Distance;

        foreach (GameObject tile in TileManager.Instance.Grid[Position[0], Position[1]].GetComponent<Tile>().AdjacentTiles)
        {
            CurrentTile = tile.GetComponent<Tile>();

            Distance = Mathf.Sqrt((CurrentTile.GridPosition[1] - Unit.Position[1]) ^ 2 + (CurrentTile.GridPosition[0] - Unit.Position[0]) ^ 2);

            Tiles.Add(CurrentTile, Distance);

            if (Closest == null)
            {
                Closest = tile.GetComponent<Tile>();
            }
            else
            {
                if (Tiles[Closest] > Distance)
                {
                    Closest = CurrentTile;
                }
            }
        }

        Direction Dir;
        

        if (Position[1] == Closest.GridPosition[1] )
        {
            if (Closest.GridPosition[0] > Position[0])
            {
                Dir = Direction.Right;
            }
            else
            {
                Dir = Direction.Left;
            }
        }
        else
        {
            if (Closest.GridPosition[1] > Position[1])
            {
                Dir = Direction.Up;
            }
            else
            {
                Dir = Direction.Down;
            }
        }

        //print(Dir);
        //print(Closest);
        
        AoEDamage(Dir);
    }

    internal void AoEDamage(Direction Dir)
    {
        int[] CurrentPosition = new int[2];

        int[] Centering = new int[2];

        Centering[0] = 0;
        Centering[1] = 0;

        AoESpecialAttack AttackAoE = (AoESpecialAttack)CurrentAttack;

        //Centering[0] = (Direction.Down == Dir || Direction.Up == Dir) ? Mathf.FloorToInt(AttackAoE.HorizontalRange / 2) : 0;
        //Centering[1] = (Direction.Right == Dir || Direction.Left == Dir) && Centering[0] == 0 ? Mathf.FloorToInt(AttackAoE.VerticalRange / 2) : 0;

        switch (Dir)
        {
            case Direction.Up:
                {
                    Centering[0] = -Mathf.FloorToInt(AttackAoE.HorizontalRange / 2);
                    Centering[1] = AttackAoE.VerticalRange - 1;
                    break;
                }
            case Direction.Down:
                {
                    Centering[0] = -Mathf.FloorToInt(AttackAoE.HorizontalRange / 2);
                    break;
                }
            case Direction.Left:
                {
                    Centering[0] = -Mathf.FloorToInt(AttackAoE.HorizontalRange / 2);
                    Centering[1] = -Mathf.FloorToInt(AttackAoE.VerticalRange / 2);
                    break;
                }
            case Direction.Right:
                {
                    Centering[0] = Mathf.FloorToInt(AttackAoE.HorizontalRange / 2);
                    break;
                }
        }

        for (int i = 0; i <= AttackAoE.VerticalRange; i++)
        {
            CurrentPosition[0] = Position[0] + i + Centering[0];

            if (CurrentPosition[0] < 0 || CurrentPosition[0] >= TileManager.Instance.Width - 1)
            {
                continue;
            }

            for (int j = 0; j < AttackAoE.HorizontalRange; j++)
            {

                CurrentPosition[1] = Position[1] + j + Centering[1];

                if (CurrentPosition[1] < 0 || CurrentPosition[1] >= TileManager.Instance.Height - 1)
                {
                    continue;
                }

                if (!AoELocations.Contains(TileManager.Instance.Grid[CurrentPosition[0], CurrentPosition[1]].GetComponent<Tile>()))
                {
                    AoELocations.Add(TileManager.Instance.Grid[CurrentPosition[0], CurrentPosition[1]].GetComponent<Tile>());
                }
            }
        }

        CurrentTurns = 1;
        TurnCount = AttackAoE.TurnTilDamage;
        PendingAttack = true;
    }

    internal void ShowDamageRange()
    {
        foreach(Tile tile in AoELocations)
        {
            tile.Show(true);
        }
    }
}
