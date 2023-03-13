using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : UnitAI
{
    public bool isMultiTile = false;
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
        List<GameObject> Tiles = new List<GameObject>();
        Tiles.Add(TileManager.Instance.Grid[Position[0], Position[1]]);

        Direction Vert;
        Direction Hori;

        if (Unit.Position[0] > Position[0])
        {
            //Right
            Hori = Direction.Right;
        }
        else
        {
            //Left
            Hori = Direction.Left;
        }
        
        if (Unit.Position[1] < Position[1])
        {
            //Down
            Vert = Direction.Down;
        }
        else
        {
            //UP
            Vert = Direction.Up;
        }

        AoEDamage(Hori, Vert);
    }

    internal void AoEDamage(Direction DirHori, Direction DirVert)
    {
        int[] CurrentPosition = new int[2];

        int[] Centering = new int[2];

        AoELocations.Add(TileManager.Instance.Grid[CurrentPosition[0], CurrentPosition[1]].GetComponent<Tile>());

        AoESpecialAttack AttackAoE = (AoESpecialAttack)CurrentAttack;

        Centering[0] = (Direction.Left == DirHori || Direction.Right == DirHori) ? Mathf.FloorToInt(AttackAoE.HorizontalRange / 2): 0;
        Centering[1] = (Direction.Up == DirVert || Direction.Down == DirVert) && Centering[0] == 0 ? Mathf.FloorToInt(AttackAoE.VerticalRange / 2): 0;



        for (int i = 0; i <= AttackAoE.HorizontalRange; i++)
        {
            switch (DirHori)
            {
                case Direction.Right:
                    {
                        CurrentPosition[0] = Position[0] + i - Centering[0];
                        break;
                    }
                case Direction.Left:
                    {
                        CurrentPosition[0] = Position[0] - i + Centering[0];
                        break;
                    }
            }

            if (CurrentPosition[0] < 0 || CurrentPosition[0] >= TileManager.Instance.Width - 1)
            {
                continue;
            }

            for (int j = 0; j < AttackAoE.VerticalRange; j++)
            {
                switch (DirVert)
                {
                    case Direction.Down:
                        {
                            CurrentPosition[1] = Position[1] - j - Centering[1];
                            break;
                        }
                    case Direction.Up:
                        {
                            CurrentPosition[1] = Position[1] + j + Centering[1];
                            break;
                        }
                }

                if (CurrentPosition[1] < 0 || CurrentPosition[1] >= TileManager.Instance.Height - 1)
                {
                    continue;
                }

                AoELocations.Add(TileManager.Instance.Grid[CurrentPosition[0], CurrentPosition[1]].GetComponent<Tile>());
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
