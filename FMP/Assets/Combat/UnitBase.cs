using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBase : MonoBehaviour
{
    public int HealthMax = 50;
    public int CurrentHealth;
    internal int[] Position;
    public int Movement = 3;

    internal List<Tile> MoveableTiles;
    internal List<Tile> AttackTiles;

    //Inventory
    public Weapon EquipedWeapon;
    public List<Item> Inventory;

    //Turn Checks
    internal bool MovedForTurn = false;
    internal bool AttackedForTurn = false;
    internal bool EndTurn = false;

    //Stats
    public int Strength = 2;

    //Weapon Proficientcy
    public float BowProficiency;
    public int BowLevel;

    public float SwordProficiency;
    public int SwordLevel;

    public float MagicProficiency;
    public int MagicLevel;

    public float FistProficiency;
    public int FistLevel;

    //Class

    // Start is called before the first frame update
    void Start()
    {
        MoveableTiles = new List<Tile>();
        AttackTiles = new List<Tile>();
        CurrentHealth = HealthMax;

        MoveableArea(false);
    }

    // Update is called once per frame
    virtual public void Update()
    {
    }

    //Moves the character from the current location to the wanted location
    internal bool Move(Tile NewTile)
    {
        if (!MovedForTurn)
        {
            if (MoveableTiles.Contains(NewTile))
            {
                MovedForTurn = true;
                TileManager.Instance.Grid[Position[0], Position[1]].GetComponent<Tile>().ChangeOccupant(null);
                transform.position = NewTile.CentrePoint.transform.position;
                Position[0] = NewTile.GridPosition[0];
                Position[1] = NewTile.GridPosition[1];
                NewTile.ChangeOccupant(this);
                ResetMoveableTiles();

                List<GameObject> Tile = new List<GameObject>();
                Tile.Add(TileManager.Instance.Grid[Position[0], Position[1]].gameObject);

                if(!AttackableArea(Tile, false))
                {
                    AttackedForTurn = true;
                }

                return true;
            }
        }

        return false;
    }
    public bool AttackableArea(List<GameObject> CheckingTiles, bool ShowTiles = true)
    {
        CheckingTiles.Add(TileManager.Instance.Grid[Position[0], Position[1]]);

        if (EquipedWeapon)
        {
            for (int i = 0; i < EquipedWeapon.Range; i++)
            {
                CheckingTiles = CheckTiles(CheckingTiles, true, ShowTiles);
            }
        }
        else
        {
            CheckingTiles = CheckTiles(CheckingTiles, true, ShowTiles);
        }

        //Checks if an enemy unit is in the attack range
        foreach(Tile tile in AttackTiles)
        {
            if(tile.Unit)
            {
                //Makes sure it's not a unit on the same team
                if (!tile.Unit.CompareTag(tag))
                {
                    return true;
                }
            }
        }

        return false;

    }

    //Finds what tiles can be moved to from the current place
    public void MoveableArea(bool ShowTiles = true)
    {
        MoveableTiles.Clear();
        AttackTiles.Clear();
        List<GameObject> CheckingTiles = new List<GameObject>();
        CheckingTiles.Add(TileManager.Instance.Grid[Position[0],Position[1]]);

        for(int i = 0; i < Movement; i++)
        {
            CheckingTiles = CheckTiles(CheckingTiles, false, ShowTiles);
        }

        AttackableArea(CheckingTiles, ShowTiles);
    }

    //Adds the adjacent tiles to moveable/attack tiles list and shows the tiles in the correct colour
    internal List<GameObject> CheckTiles(List<GameObject> tiles, bool WeaponRange = false, bool ShowTile = true)
    {
        List<GameObject> NextLayer = new List<GameObject>();

        foreach (GameObject tile in tiles)
        {
            foreach (GameObject AdjacentTile in tile.GetComponent<Tile>().AdjacentTiles)
            {
                if (!MoveableTiles.Contains(AdjacentTile.GetComponent<Tile>()))
                {
                    if (AdjacentTile.GetComponent<Tile>().CanMoveOn || AdjacentTile.GetComponent<Tile>().Unit)
                    {
                        if (!WeaponRange)
                        {
                            MoveableTiles.Add(AdjacentTile.GetComponent<Tile>());
                        }

                        NextLayer.Add(AdjacentTile);

                    }
                    AttackTiles.Add(AdjacentTile.GetComponent<Tile>());

                    if (ShowTile)
                    {
                        AdjacentTile.GetComponent<Tile>().Show(WeaponRange);
                    }
                }

            }
        }

        return NextLayer;
    }

    //Hides all changed tiles
    internal void HideAllChangedTiles()
    {
        foreach (Tile tile in AttackTiles)
        {
            if (Interact.Instance.SelectedUnit)
            {
                //if (!Interact.Instance.SelectedUnit.AttackTiles.Contains(tile))
                //{
                    tile.Hide();
                //}
            }
            else
            {
                tile.Hide();
            }
        }
    }

    internal void ShowAllInRangeTiles()
    {
        foreach (Tile tile in AttackTiles)
        {
            tile.WhichColour(this);
        }
    }

    //Hides all changed tiles and Clears both lists
    internal void ResetMoveableTiles()
    {
        HideAllChangedTiles();

        MoveableTiles.Clear();
        AttackTiles.Clear();
    }

    internal void Attack(UnitBase Enemy)
    {
        //Change later to proper logic
        Move(TileManager.Instance.Grid[Enemy.Position[0] - 1, Enemy.Position[1]].GetComponent<Tile>());
        //Damage and hit rate to be calculated and implemented later

        if (EquipedWeapon)
        {
            Enemy.CurrentHealth -= EquipedWeapon.Damage * 2;
        }
        else
        {
            Enemy.CurrentHealth -= 2;
        }

        ResetMoveableTiles();
        MovedForTurn = true;
        AttackedForTurn = true;
        EndTurn = true;
    }

    internal void CalculateDamageTaken(int Attack)
    {
        CurrentHealth -= Attack;
    }

    private void OnMouseEnter()
    {
        if (!Interact.Instance.CombatMenu.transform.GetChild(0).gameObject.activeInHierarchy)
        {
            if (!CompareTag("Enemy"))
            {
                if (!MovedForTurn)
                {
                    ShowAllInRangeTiles();
                }
                else if (!AttackedForTurn)
                {
                    List<GameObject> Tiles = new List<GameObject>();
                    Tiles.Add(TileManager.Instance.Grid[Position[0], Position[1]]);
                    AttackableArea(Tiles);
                }
            }
            else
            {
                ShowAllInRangeTiles();
            }

            TileManager.Instance.Grid[Position[0], Position[1]].GetComponent<Tile>().Show(false, true);
        }
    }

    private void OnMouseExit()
    {
        if (!Interact.Instance.CombatMenu.transform.GetChild(0).gameObject.activeInHierarchy)
        {
            if (!Interact.Instance.SelectedUnit)
            {
                HideAllChangedTiles();
            }
            else
            {
                TileManager.Instance.Grid[Position[0], Position[1]].GetComponent<Tile>().WhichColour();
            }
        }
    }

    public void TurnChange()
    {
        MovedForTurn = false;
        AttackedForTurn = false;
        EndTurn = false;

        MoveableArea(false);
    }

    internal void WaitUnit()
    {
        foreach(Tile tile in AttackTiles)
        {
            tile.Hide();
        }

        MovedForTurn = true;
        AttackedForTurn = true;
        EndTurn = true;
        Interact.Instance.CombatMenu.CombatMenuObject.SetActive(false);
    }

    internal void MoveButton()
    {

        Interact.Instance.CombatMenu.CombatMenuObject.SetActive(false);
    }

    internal void AttackButton()
    {
        List<GameObject> MainTile = new List<GameObject>();
        MainTile.Add(TileManager.Instance.Grid[Position[0], Position[1]]);
        AttackableArea(MainTile);
        Interact.Instance.CombatMenu.CombatMenuObject.SetActive(false);
    }

    internal void ItemButton()
    {
        Interact.Instance.CombatMenu.InventoryObject.SetActive(true);
    }

    internal void SpecialButton()
    {
        Interact.Instance.CombatMenu.CombatMenuObject.SetActive(false);

        foreach(GameObject tile in TileManager.Instance.Grid[Position[0], Position[1]].GetComponent<Tile>().AdjacentTiles)
        {
            if(tile.GetComponent<Tile>().Special)
            {
                tile.GetComponent<Tile>().Special.Special(this);
            }
        }
    }

}
