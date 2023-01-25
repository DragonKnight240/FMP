using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool CanMoveOn;
    internal UnitBase Unit;
    internal bool Occupied;
    public GameObject CentrePoint;
    internal List<GameObject> AdjacentTiles;
    internal int[] GridPosition;

    public Material InRangeMaterial; //Temp
    public Material WeaponRangeMaterial; //Temp
    Material OGMaterial;

    // Start is called before the first frame update
    void Start()
    {
        if(CanMoveOn || Unit)
        {
            Occupied = true;
        }

        OGMaterial = GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Show(bool WeaponRange = false)
    {
        if (WeaponRange)
        {
            GetComponent<MeshRenderer>().material = WeaponRangeMaterial;
        }
        else
        {
            GetComponent<MeshRenderer>().material = InRangeMaterial;
        }
    }

    public void ChangeOccupant(UnitBase NewUnit)
    {
        Unit = NewUnit;
        Occupied = true;
        CanMoveOn = false;
    }

    public void Hide()
    {
        GetComponent<MeshRenderer>().material = OGMaterial;
    }

    internal void FindAdjacentTiles()
    {
        AdjacentTiles = new List<GameObject>();

        //Top
        if (GridPosition[0] + 1 < TileManager.Instance.Width && GridPosition[0] + 1 >= 0)
        {
            if (GridPosition[1] < TileManager.Instance.Height && GridPosition[1] >= 0)
            {
                if (TileManager.Instance.Grid[GridPosition[0] + 1, GridPosition[1]].GetComponent<Tile>().CanMoveOn)
                {
                    AdjacentTiles.Add(TileManager.Instance.Grid[GridPosition[0] + 1, GridPosition[1]]);
                }
            }
        }

        //Left
        if (GridPosition[0] < TileManager.Instance.Width && GridPosition[0] >= 0)
        {
            if (GridPosition[1] - 1 < TileManager.Instance.Height && GridPosition[1] - 1 >= 0)
            {
                if (TileManager.Instance.Grid[GridPosition[0], GridPosition[1] - 1].GetComponent<Tile>().CanMoveOn)
                {
                    AdjacentTiles.Add(TileManager.Instance.Grid[GridPosition[0], GridPosition[1] - 1]);
                }
            }
        }

        //Right
        if (GridPosition[0] < TileManager.Instance.Width && GridPosition[0] >= 0)
        {
            if (GridPosition[1] + 1 < TileManager.Instance.Height && GridPosition[1] + 1 >= 0)
            {
                if (TileManager.Instance.Grid[GridPosition[0], GridPosition[1] + 1].GetComponent<Tile>().CanMoveOn)
                {
                    AdjacentTiles.Add(TileManager.Instance.Grid[GridPosition[0], GridPosition[1] + 1]);
                }
            }
        }

        //Bottom
        if (GridPosition[0] - 1 < TileManager.Instance.Width && GridPosition[0] - 1 >= 0)
        {
            if (GridPosition[1] < TileManager.Instance.Height && GridPosition[1] >= 0)
            {
                if (TileManager.Instance.Grid[GridPosition[0] - 1, GridPosition[1]].GetComponent<Tile>().CanMoveOn)
                {
                    AdjacentTiles.Add(TileManager.Instance.Grid[GridPosition[0] - 1, GridPosition[1]]);
                }
            }
        }
    }
}
