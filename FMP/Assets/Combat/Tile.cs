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
    public Material OverlayMaterial; //Temp
    Material OGMaterial;

    // Start is called before the first frame update
    void Start()
    {
        if(!CanMoveOn || Unit)
        {
            Occupied = true;
        }

        OGMaterial = GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Show(bool WeaponRange = false, bool Overlay = false, bool Reset = false)
    {
        if (WeaponRange && !Reset)
        {
            GetComponent<MeshRenderer>().material = WeaponRangeMaterial;
        }
        else if(!Overlay && !Reset)
        {
            GetComponent<MeshRenderer>().material = InRangeMaterial;
        }
        else if(Overlay && !Reset)
        {
            GetComponent<MeshRenderer>().material = OverlayMaterial;
        }
        else
        {
            GetComponent<MeshRenderer>().material = OGMaterial;
        }
    }

    public void ChangeOccupant(UnitBase NewUnit)
    {
        Unit = NewUnit;
        Occupied = NewUnit? true : false;
        CanMoveOn = NewUnit ? false : true;
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

    private void OnMouseEnter()
    {
        if (Unit)
        {
            if (!Unit.CompareTag("Enemy"))
            {
                Unit.MoveableArea();
            }
        }
        
        Show(false, true);
        CameraMove.Instance.FollowTarget = transform;
    }

    private void OnMouseExit()
    {
        if(Unit && !Interact.Instance.SelectedUnit)
        {
            Unit.ResetMoveableTiles();
        }
        
        WhichColour();
    }

    internal void WhichColour()
    {
        if (Interact.Instance.SelectedUnit)
        {
            bool inRange = Interact.Instance.SelectedUnit.AttackTiles.Contains(this);
            if (inRange)
            {
                Show(Interact.Instance.SelectedUnit.AttackTiles.Contains(this) && Interact.Instance.SelectedUnit.MoveableTiles.Contains(this) ? false : true, !inRange);
                return;
            }
        }

        Show(false, false, true);
    }
}
