using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockableTree : InteractOnGrid
{
    public int Damage = 2;
    internal bool isFalling = false;
    internal Quaternion RotateTo;
    public float Speed = 50;
    public int HitAmountWidth;
    public int HitAmountHeight;
    internal List<GameObject> AoETiles;
    internal AudioSource FallingSource;
    public AudioClip TreeFallingSound;
    public AudioClip TreeHitingGroundSound;


    // Start is called before the first frame update
    override internal void Start()
    {
        base.Start();
        AoETiles = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isFalling)
        {
            if(FallingSource == null && TreeFallingSound != null)
            {
                //print("Falling sound");
                FallingSource = SoundManager.Instance.PlaySFX(TreeFallingSound);
            }

            transform.rotation = Quaternion.Lerp(transform.rotation, RotateTo, Time.deltaTime * Speed);

            if(transform.rotation.ToString() == RotateTo.ToString())
            {
                if(FallingSource)
                {
                    FallingSource.Stop();
                }

                if (TreeHitingGroundSound)
                {
                    //print("Hiting sounds");
                    SoundManager.Instance.PlaySFX(TreeHitingGroundSound);
                }

                isFalling = false;
                gameObject.SetActive(false);
                TileManager.Instance.Grid[Position[0], Position[1]].GetComponent<Tile>().Special = null;
                UnitManager.Instance.UnitUpdate.Invoke();
                TurnManager.Instance.PendingAction = false;
            }
        }
    }

    public override void Special(UnitBase Unit)
    {
        if (Unit.Class.Name == ClassNeeded.Name)
        {
            TurnManager.Instance.PendingAction = true;
            UnitToActiveIt = Unit;
            CalculateAoE(InteractLocations[TileManager.Instance.Grid[Unit.Position[0], Unit.Position[1]].GetComponent<Tile>()]);
            DealAoEDamage();

            TileManager.Instance.Grid[Position[0], Position[1]].GetComponent<Tile>().CanMoveOn = true;
            TileManager.Instance.Grid[Position[0], Position[1]].GetComponent<Tile>().Occupied = false;
        }
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

                if(tile.GetComponent<Tile>().Unit.CurrentHealth <=0)
                {
                    UnitManager.Instance.PendingDeath.Add(tile.GetComponent<Tile>().Unit);
                }
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
