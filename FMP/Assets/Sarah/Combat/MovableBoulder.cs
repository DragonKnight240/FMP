using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableBoulder : InteractOnGrid
{
    public int GauntletLevelMin;
    public int Damage = 2;
    public float MoveSpeed = 50;
    public float RotateSpeed = 100;
    internal List<GameObject> AoETiles;
    List<Tile> Path;
    bool Moving = false;
    Tile LastTile;

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
                    GetComponent<Fading>().FadeOut = false;
                    if(LastTile.Unit)
                    {
                        LastTile.Unit.ShowLongDistanceDamageNumbers(Damage);
                    }
                    return;
                }
            }

            transform.Rotate(new Vector3(0, 0, RotateSpeed * Time.deltaTime));

            transform.position = Vector3.MoveTowards(transform.position, new Vector3(Path[0].transform.position.x, transform.position.y, Path[0].transform.position.z), MoveSpeed * Time.deltaTime);
        }
    }

    public override void Special(UnitBase Unit)
    {
        if (Unit.FistLevel >= GauntletLevelMin)
        {
            CalculateAoE(InteractLocations[TileManager.Instance.Grid[Unit.Position[0], Unit.Position[1]].GetComponent<Tile>()]);
            //CalculateMoveTo();
        }
    }

    internal override void CalculateAoE(Direction DirectionInteraction)
    {
        bool Next = true;
        int[] CurrentTilePosition = new int[2];
        CurrentTilePosition[0] = Position[0];
        CurrentTilePosition[1] = Position[1];

        print("Calculate AOE");

        switch (DirectionInteraction)
        {
            case Direction.Up:
                {
                    print("Moving Down");
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

                        if(TileManager.Instance.Grid[Position[0], Position[1]].GetComponent<Tile>().CanMoveOn)
                        {
                            Path.Add(TileManager.Instance.Grid[Position[0], Position[1]].GetComponent<Tile>());
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
                            Next = false;
                        }
                    } while (Next);
                    break;
                }
            case Direction.Down:
                {
                    print("Move UP");

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
                            Next = false;
                        }
                    } while (Next);

                    break;
                }
            case Direction.Right:
                {
                    print("Move Left");
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
                            Next = false;
                        }
                    } while (Next);
                    break;
                }
            case Direction.Left:
                {
                    print("Moving Right");

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
                            Next = false;
                        }
                    } while (Next);
                    break;
                }
        }

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
