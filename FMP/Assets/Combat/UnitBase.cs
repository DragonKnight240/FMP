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
    public Weapon EquipedWeapon;
    internal bool MovedForTurn = false;
    //Stats
    public int Strength = 2;
    //Weapon
    //Class
    public List<Item> Inventory;

    // Start is called before the first frame update
    void Start()
    {
        MoveableTiles = new List<Tile>();
        AttackTiles = new List<Tile>();
        CurrentHealth = HealthMax;
    }

    // Update is called once per frame
    virtual public void Update()
    {
    }

    //Moves the character from the current location to the wanted location
    internal bool Move(Tile NewTile)
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

            return true;
            
        }

        return false;
    }

    //Finds what tiles can be moved to from the current place
    public void MoveableArea()
    {
        MoveableTiles = new List<Tile>();
        AttackTiles = new List<Tile>();
        List<GameObject> CheckingTiles = new List<GameObject>();
        CheckingTiles.Add(TileManager.Instance.Grid[Position[0],Position[1]]);

        for(int i = 0; i < Movement; i++)
        {
            CheckingTiles = CheckTiles(CheckingTiles);
        }

        if (EquipedWeapon)
        {
            for (int i = 0; i < EquipedWeapon.Range; i++)
            {
                CheckingTiles = CheckTiles(CheckingTiles, true);
            }
        }
        else
        {
            CheckingTiles = CheckTiles(CheckingTiles, true);
        }
    }

    //Adds the adjacent tiles to moveable/attack tiles list and shows the tiles in the correct colour
    internal List<GameObject> CheckTiles(List<GameObject> tiles, bool WeaponRange = false)
    {
        List<GameObject> NextLayer = new List<GameObject>();

        foreach (GameObject tile in tiles)
        {
            foreach (GameObject AdjacentTile in tile.GetComponent<Tile>().AdjacentTiles)
            {
                if (!MoveableTiles.Contains(AdjacentTile.GetComponent<Tile>()))
                {
                    if (!WeaponRange)
                    {
                        MoveableTiles.Add(AdjacentTile.GetComponent<Tile>());
                    }
                    AttackTiles.Add(AdjacentTile.GetComponent<Tile>());
                    NextLayer.Add(AdjacentTile);
                    AdjacentTile.GetComponent<Tile>().Show(WeaponRange);
                }

            }
        }

        return NextLayer;
    }

    //Hides all tiles
    internal void ResetMoveableTiles()
    {
        foreach (Tile tile in AttackTiles)
        {
            tile.Hide();
        }

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
    }

    private void OnMouseEnter()
    {
        MoveableArea();
        
        TileManager.Instance.Grid[Position[0], Position[1]].GetComponent<Tile>().Show(false, true);
    }

    private void OnMouseExit()
    {
        if (!Interact.Instance.SelectedUnit && !CompareTag("Enemy"))
        {
            ResetMoveableTiles();
        }
        else
        {
            TileManager.Instance.Grid[Position[0], Position[1]].GetComponent<Tile>().WhichColour();
        }
    }

    public void TurnChange()
    {
        MovedForTurn = false;
    }

    internal void WaitUnit()
    {
        MovedForTurn = true;
    }

}
