using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBase : MonoBehaviour
{
    public int HealthMax = 50;
    public int CurrentHealth;
    internal int[] Position;
    public int Movement = 3;
    List<Tile> MoveableTiles;
    internal List<Tile> AttackTiles;
    public Weapon EquipedWeapon;
    //Stats
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
    void Update()
    {
        
    }

    internal bool Move(Tile NewTile)
    {
        if (MoveableTiles.Contains(NewTile))
        {
            transform.position = NewTile.CentrePoint.transform.position;
            Position[0] = NewTile.GridPosition[0];
            Position[1] = NewTile.GridPosition[1];
            NewTile.ChangeOccupant(this);
            ResetMoveableTiles();

            return true;
            
        }

        return false;
    }

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

        for(int i = 0; i < EquipedWeapon.Range; i++)
        {
            CheckingTiles = CheckTiles(CheckingTiles, true);
        }
    }

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

    internal void ResetMoveableTiles()
    {
        foreach (Tile tile in AttackTiles)
        {
            tile.Hide();
        }

        MoveableTiles = new List<Tile>();
        AttackTiles = new List<Tile>();
    }

    internal void Attack(UnitBase Enemy)
    {
        //Change later to proper logic
        Move(TileManager.Instance.Grid[Enemy.Position[0] - 1, Enemy.Position[1]].GetComponent<Tile>());
        //Damage and hit rate to be calculated and implemented later
        Enemy.CurrentHealth -= EquipedWeapon.Damage;

        ResetMoveableTiles();
    }

    private void OnMouseEnter()
    {
        if (!CompareTag("Enemy"))
        {
            MoveableArea();
        }
    }

    private void OnMouseExit()
    {
        if (!Interact.Instance.SelectedUnit && !CompareTag("Enemy"))
        {
            ResetMoveableTiles();
        }
    }
}
