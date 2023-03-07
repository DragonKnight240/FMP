using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableBoulder : InteractOnGrid
{
    public int Damage = 2;
    public float MoveSpeed = 50;
    public float RotateSpeed = 100;
    internal List<GameObject> AoETiles;
    List<Tile> Path;
    bool Moving = false;
    Tile LastTile;
    UnitBase Target;
    Direction RollDirection;

    // Start is called before the first frame update
    void Start()
    {
        Path = new List<Tile>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Moving)
        {
            if (Path.Count <= 0)
            {
                return;
            }

            if (new Vector3(Path[0].CentrePoint.transform.position.x, transform.position.y, Path[0].CentrePoint.transform.position.z) ==
                new Vector3(transform.position.x, transform.position.y, transform.position.z))
            {
                Path.RemoveAt(0);
                if (Path.Count <= 0)
                {
                    Moving = false;
                    if(Target)
                    {
                        Target.ShowLongDistanceDamageNumbers(Damage + UnitToActiveIt.RankBonus[UnitToActiveIt.FistLevel]);
                        Target = null;
                    }

                    CameraMove.Instance.FollowTarget = null;
                    CameraMove.Instance.Override = false;
                    ResetSpecial();
                    return;
                }
            }

            switch(RollDirection)
            {
                case Direction.Down:
                    {
                        transform.Rotate(new Vector3(-RotateSpeed * Time.deltaTime, 0, 0));
                        break;
                    }
                case Direction.Up:
                    {
                        transform.Rotate(new Vector3(RotateSpeed * Time.deltaTime, 0, 0));
                        break;
                    }
                case Direction.Right:
                    {
                        transform.Rotate(new Vector3(0, 0, -RotateSpeed * Time.deltaTime));
                        break;
                    }
                case Direction.Left:
                    {
                        transform.Rotate(new Vector3(0, 0, RotateSpeed * Time.deltaTime));
                        break;
                    }
            }

            transform.position = Vector3.MoveTowards(transform.position, new Vector3(Path[0].transform.position.x, transform.position.y, Path[0].transform.position.z), MoveSpeed * Time.deltaTime);
        }
    }

    public void ResetSpecial()
    {
        TileSetter(TileManager.Instance.Grid[Position[0],Position[1]].GetComponent<Tile>().AdjacentTiles);
    }

    public override void Special(UnitBase Unit)
    {
        UnitToActiveIt = Unit;
        CalculateAoE(InteractLocations[TileManager.Instance.Grid[Unit.Position[0], Unit.Position[1]].GetComponent<Tile>()]);
    }

    internal override void CalculateAoE(Direction DirectionInteraction)
    {
        bool Next = true;
        int[] CurrentTilePosition = new int[2];
        CurrentTilePosition[0] = Position[0];
        CurrentTilePosition[1] = Position[1];

        switch (DirectionInteraction)
        {
            case Direction.Up:
                {
                    do
                    {
                        if(Position[0] == CurrentTilePosition[0] && Position[1] == CurrentTilePosition[1])
                        {
                            if (CurrentTilePosition[1] > 0)
                            {
                                CurrentTilePosition[1] -= 1;
                            }
                            else
                            {
                                Next = false;
                            }
                            continue;
                        }

                        if(TileManager.Instance.Grid[CurrentTilePosition[0], CurrentTilePosition[1]].GetComponent<Tile>().CanMoveOn)
                        {
                            Path.Add(TileManager.Instance.Grid[CurrentTilePosition[0], CurrentTilePosition[1]].GetComponent<Tile>());
                            if (CurrentTilePosition[1] > 0)
                            {
                                CurrentTilePosition[1] -= 1;
                            }
                            else
                            {
                                Next = false;
                            }
                        }
                        else
                        {
                            if (TileManager.Instance.Grid[CurrentTilePosition[0], CurrentTilePosition[1]].GetComponent<Tile>().Unit)
                            {
                                if (UnitToActiveIt.FistLevel == 5)
                                {
                                    if (TileManager.Instance.Grid[CurrentTilePosition[0], CurrentTilePosition[1]].GetComponent<Tile>().Unit.CompareTag(UnitToActiveIt.tag))
                                    {
                                        if (CurrentTilePosition[1] > 0)
                                        {
                                            CurrentTilePosition[1] -= 1;
                                        }
                                        else
                                        {
                                            Target = TileManager.Instance.Grid[CurrentTilePosition[0], CurrentTilePosition[1]].GetComponent<Tile>().Unit;
                                            Next = false;
                                        }
                                        continue;
                                    }
                                }
                                else
                                {
                                    Target = TileManager.Instance.Grid[CurrentTilePosition[0], CurrentTilePosition[1]].GetComponent<Tile>().Unit;
                                }
                            }

                            Next = false;
                        }
                    } while (Next);

                    RollDirection = Direction.Down;
                    break;
                }
            case Direction.Down:
                {
                    do
                    {
                        if (Position[0] == CurrentTilePosition[0] && Position[1] == CurrentTilePosition[1])
                        {
                            if (CurrentTilePosition[1] < TileManager.Instance.Height - 1)
                            {
                                CurrentTilePosition[1] += 1;
                            }
                            else
                            {
                                Next = false;
                            }
                            continue;
                        }

                        if (TileManager.Instance.Grid[CurrentTilePosition[0], CurrentTilePosition[1]].GetComponent<Tile>().CanMoveOn)
                        {
                            Path.Add(TileManager.Instance.Grid[CurrentTilePosition[0], CurrentTilePosition[1]].GetComponent<Tile>());
                            if (CurrentTilePosition[1] < TileManager.Instance.Height - 1)
                            {
                                CurrentTilePosition[1] += 1;
                            }
                            else
                            {
                                Next = false;
                            }
                        }
                        else
                        {
                            if (TileManager.Instance.Grid[CurrentTilePosition[0], CurrentTilePosition[1]].GetComponent<Tile>().Unit)
                            {
                                if (UnitToActiveIt.FistLevel == 5)
                                {
                                    if (TileManager.Instance.Grid[CurrentTilePosition[0], CurrentTilePosition[1]].GetComponent<Tile>().Unit.CompareTag(UnitToActiveIt.tag))
                                    {
                                        if (CurrentTilePosition[1] < TileManager.Instance.Height - 1)
                                        {
                                            CurrentTilePosition[1] += 1;
                                        }
                                        else
                                        {
                                            Target = TileManager.Instance.Grid[CurrentTilePosition[0], CurrentTilePosition[1]].GetComponent<Tile>().Unit;
                                            Next = false;
                                        }
                                        continue;
                                    }
                                }
                                else
                                {
                                    Target = TileManager.Instance.Grid[CurrentTilePosition[0], CurrentTilePosition[1]].GetComponent<Tile>().Unit;
                                }
                            }

                            Next = false;
                        }
                    } while (Next);
                    RollDirection = Direction.Up;

                    break;
                }
            case Direction.Right:
                {
                    do
                    {
                        if (Position[0] == CurrentTilePosition[0] && Position[1] == CurrentTilePosition[1])
                        {
                            if (CurrentTilePosition[0] > 0)
                            {
                                CurrentTilePosition[0] -= 1;
                            }
                            else
                            {
                                Next = false;
                            }
                            continue;
                        }

                        if (TileManager.Instance.Grid[CurrentTilePosition[0], CurrentTilePosition[1]].GetComponent<Tile>().CanMoveOn)
                        {
                            Path.Add(TileManager.Instance.Grid[CurrentTilePosition[0], CurrentTilePosition[1]].GetComponent<Tile>());
                            if (CurrentTilePosition[0] > 0)
                            {
                                CurrentTilePosition[0] -= 1;
                            }
                            else
                            {
                                Next = false;
                            }
                        }
                        else
                        {
                            if (TileManager.Instance.Grid[CurrentTilePosition[0], CurrentTilePosition[1]].GetComponent<Tile>().Unit)
                            {
                                if (UnitToActiveIt.FistLevel == 5)
                                {
                                    if (TileManager.Instance.Grid[CurrentTilePosition[0], CurrentTilePosition[1]].GetComponent<Tile>().Unit.CompareTag(UnitToActiveIt.tag))
                                    {
                                        if (CurrentTilePosition[0] > 0)
                                        {
                                            CurrentTilePosition[0] -= 1;
                                        }
                                        else
                                        {
                                            Target = TileManager.Instance.Grid[CurrentTilePosition[0], CurrentTilePosition[1]].GetComponent<Tile>().Unit;
                                            Next = false;
                                        }
                                        continue;
                                    }
                                }
                                else
                                {
                                    Target = TileManager.Instance.Grid[CurrentTilePosition[0], CurrentTilePosition[1]].GetComponent<Tile>().Unit;
                                }
                            }

                            Next = false;
                        }
                    } while (Next);

                    RollDirection = Direction.Left;
                    break;
                }
            case Direction.Left:
                {
                    do
                    {
                        if (Position[0] == CurrentTilePosition[0] && Position[1] == CurrentTilePosition[1])
                        {
                            if (CurrentTilePosition[0] < TileManager.Instance.Width - 1)
                            {
                                CurrentTilePosition[0] += 1;
                            }
                            else
                            {
                                Next = false;
                            }
                            continue;
                        }

                        if (TileManager.Instance.Grid[CurrentTilePosition[0], CurrentTilePosition[1]].GetComponent<Tile>().CanMoveOn)
                        {
                            Path.Add(TileManager.Instance.Grid[CurrentTilePosition[0], CurrentTilePosition[1]].GetComponent<Tile>());
                            if (CurrentTilePosition[0] < TileManager.Instance.Width - 1)
                            {
                                CurrentTilePosition[0] += 1;
                            }
                            else
                            {
                                Next = false;
                            }
                        }
                        else
                        {
                            if (TileManager.Instance.Grid[CurrentTilePosition[0], CurrentTilePosition[1]].GetComponent<Tile>().Unit)
                            {
                                if (UnitToActiveIt.FistLevel == 5)
                                {
                                    if (TileManager.Instance.Grid[CurrentTilePosition[0], CurrentTilePosition[1]].GetComponent<Tile>().Unit.CompareTag(UnitToActiveIt.tag))
                                    {
                                        if (CurrentTilePosition[0] < TileManager.Instance.Width - 1)
                                        {
                                            CurrentTilePosition[0] += 1;
                                        }
                                        else
                                        {
                                            Target = TileManager.Instance.Grid[CurrentTilePosition[0], CurrentTilePosition[1]].GetComponent<Tile>().Unit;
                                            Next = false;
                                        }
                                        continue;
                                    }
                                }
                                else
                                {
                                    Target = TileManager.Instance.Grid[CurrentTilePosition[0], CurrentTilePosition[1]].GetComponent<Tile>().Unit;
                                }
                            }
                            Next = false;
                        }
                    } while (Next);
                    RollDirection = Direction.Right;
                    break;
                }
        }

        if (Target)
        {
            CameraMove.Instance.FollowTarget = TileManager.Instance.Grid[Target.Position[0], Target.Position[1]].transform;
        }
        else
        {
            CameraMove.Instance.FollowTarget = transform;
        }

        CameraMove.Instance.Override = true;
        Moving = true;
        LastTile = Path[Path.Count - 1];
    }

    private void OnMouseEnter()
    {
        if (!UnitManager.Instance.SetupFinished || !Interact.Instance.VirtualCam.activeInHierarchy || Options.Instance.OptionsMenuUI.activeInHierarchy
    || Interact.Instance.CombatMenu.VictoryScreen.activeInHierarchy || Interact.Instance.CombatMenu.DefeatScreen.activeInHierarchy)
        {
            return;
        }

        TileManager.Instance.Grid[Position[0], Position[1]].GetComponent<Tile>().Show(false, true);
    }

    private void OnMouseExit()
    {
        if (!UnitManager.Instance.SetupFinished)
        {
            return;
        }

        if (!Interact.Instance.CombatMenu.CombatMenuObject.gameObject.activeInHierarchy
            && !Interact.Instance.CombatMenu.AttackMenuObject.gameObject.activeInHierarchy
            && Interact.Instance.SelectedUnit == null)
        {
            TileManager.Instance.Grid[Position[0], Position[1]].GetComponent<Tile>().Hide();
        }
    }
}
