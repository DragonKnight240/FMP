using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBase : MonoBehaviour
{
    internal int[] Position;
    public int Movement = 3;
    List<Tile> MoveableTiles;
    //Stats
    //Weapon
    //Class

    // Start is called before the first frame update
    void Start()
    {
        MoveableTiles = new List<Tile>();
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
            ResetMoveableTiles();

            return true;
            
        }

        return false;
    }

    public void MoveableArea()
    {
        MoveableTiles = new List<Tile>();
        List<GameObject> CheckingTiles = new List<GameObject>();
        CheckingTiles.Add(TileManager.Instance.Grid[Position[0],Position[1]]);

        for(int i = 0; i < Movement; i++)
        {
            CheckingTiles = CheckTiles(CheckingTiles);
        }
    }

    internal List<GameObject> CheckTiles(List<GameObject> tiles)
    {
        List<GameObject> NextLayer = new List<GameObject>();

        foreach (GameObject tile in tiles)
        {
            foreach (GameObject AdjacentTile in tile.GetComponent<Tile>().AdjacentTiles)
            {
                if (!MoveableTiles.Contains(AdjacentTile.GetComponent<Tile>()))
                {
                    MoveableTiles.Add(AdjacentTile.GetComponent<Tile>());
                    NextLayer.Add(AdjacentTile);
                    AdjacentTile.GetComponent<Tile>().Show();
                }

            }
        }

        return NextLayer;
    }

    internal void ResetMoveableTiles()
    {
        foreach (Tile tile in MoveableTiles)
        {
            tile.Hide();
        }

        MoveableTiles = new List<Tile>();
    }

    private void OnMouseEnter()
    {
        MoveableArea();
    }

    private void OnMouseExit()
    {
        if (!Interact.Instance.SelectedUnit)
        {
            ResetMoveableTiles();
        }
    }
}
