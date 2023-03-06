using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockableTree : InteractOnGrid
{
    public int SwordLevelMin;
    public int Damage = 2;
    internal bool isFalling = false;
    internal Quaternion RotateTo;
    public float Speed = 50;
    public int HitAmountWidth;
    public int HitAmountHeight;
    internal List<GameObject> AoETiles;


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
                //GetComponent<Fading>().ChangeMaterial();
                //GetComponent<Fading>().FadeOut = true;
                gameObject.SetActive(false);
            }
        }
    }

    public override void Special(UnitBase Unit)
    {
        UnitToActiveIt = Unit;
        CalculateAoE(InteractLocations[TileManager.Instance.Grid[Unit.Position[0], Unit.Position[1]].GetComponent<Tile>()]);
        DealAoEDamage();

        TileManager.Instance.Grid[Position[0], Position[1]].GetComponent<Tile>().CanMoveOn = true;
        TileManager.Instance.Grid[Position[0], Position[1]].GetComponent<Tile>().Occupied = false;
        Unit.WaitUnit();
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
                tile.GetComponent<Tile>().Unit.ShowLongDistanceDamageNumbers(Damage);
            }
        }
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
