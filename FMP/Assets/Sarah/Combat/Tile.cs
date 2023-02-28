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
    public InteractOnGrid Special;

    public Material InRangeMaterial;
    public Material WeaponRangeMaterial; 
    public Material OverlayMaterial; 
    public Material SpecialMaterial;
    internal Material OGMaterial; 

    // Start is called before the first frame update
    void Start()
    {
        if (!CanMoveOn || Unit)
        {
            Occupied = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Unit)
        {
            if(!Unit.isActiveAndEnabled)
            {
                ChangeOccupant(null);
            }
        }

        if(Special)
        {
            if(!Special.isActiveAndEnabled)
            {
                Special = null;
            }
        }

        if(!Interact.Instance.VirtualCam.activeInHierarchy || (!TurnManager.Instance.isPlayerTurn && !Interact.Instance.VirtualCam.activeInHierarchy))
        {
            Hide();
        }
    }

    public void Show(bool WeaponRange = false, bool Overlay = false, bool Reset = false)
    {
        if (WeaponRange && !Reset && Special == null)
        {
            GetComponent<MeshRenderer>().material = WeaponRangeMaterial;
        }
        else if(!Overlay && !Reset && Special == null)
        {
            GetComponent<MeshRenderer>().material = InRangeMaterial;
        }
        else if(Overlay && !Reset)
        {
            GetComponent<MeshRenderer>().material = OverlayMaterial;
        }
        else if(Special && !Reset)
        {
            GetComponent<MeshRenderer>().material = SpecialMaterial;
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
                if (TileManager.Instance.Grid[GridPosition[0] + 1, GridPosition[1]].GetComponent<Tile>().CanMoveOn 
                    || TileManager.Instance.Grid[GridPosition[0] + 1, GridPosition[1]].GetComponent<Tile>().Unit)
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
                if (TileManager.Instance.Grid[GridPosition[0], GridPosition[1] - 1].GetComponent<Tile>().CanMoveOn
                    || TileManager.Instance.Grid[GridPosition[0], GridPosition[1] -1].GetComponent<Tile>().Unit)
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
                if (TileManager.Instance.Grid[GridPosition[0], GridPosition[1] + 1].GetComponent<Tile>().CanMoveOn
                    || TileManager.Instance.Grid[GridPosition[0], GridPosition[1] + 1].GetComponent<Tile>().Unit)
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
                if (TileManager.Instance.Grid[GridPosition[0] - 1, GridPosition[1]].GetComponent<Tile>().CanMoveOn
                    || TileManager.Instance.Grid[GridPosition[0] - 1, GridPosition[1]].GetComponent<Tile>().Unit)
                {
                    AdjacentTiles.Add(TileManager.Instance.Grid[GridPosition[0] - 1, GridPosition[1]]);
                }
            }
        }

        if (Special)
        {
            Special.GetComponent<InteractOnGrid>().TileSetter(transform.GetComponentInParent<Tile>().AdjacentTiles);
        }

    }

    private void OnMouseEnter()
    {
        if(!UnitManager.Instance.SetupFinished || !Interact.Instance.VirtualCam.activeInHierarchy  || Options.Instance.OptionsMenuUI.activeInHierarchy 
            || Interact.Instance.CombatMenu.VictoryScreen.activeInHierarchy || Interact.Instance.CombatMenu.DefeatScreen.activeInHierarchy)
        {
            return;
        }

        if (Unit)
        {
            if (Unit.EndTurn || OGMaterial == null)
            {
                Hide();
                return;
            }
        }

        if (!Interact.Instance.CombatMenu.transform.GetChild(0).gameObject.activeInHierarchy
            && !Interact.Instance.CombatMenu.AttackMenuObject.gameObject.activeInHierarchy 
            && Interact.Instance.SelectedUnit == null)
        {
            if (Unit)
            {
                if (!Unit.CompareTag("Enemy"))
                {
                    if (!Unit.MovedForTurn)
                    {
                        Unit.ShowAllInRangeTiles();
                    }
                    else
                    {
                        List<GameObject> Tiles = new List<GameObject>();
                        Tiles.Add(gameObject);
                        Unit.AttackableArea(Tiles);
                    }
                }
                else
                {
                    Unit.ShowAllInRangeTiles();
                }
            }
        }

        if (!Interact.Instance.CombatMenu.AttackMenuObject.gameObject.activeInHierarchy
            && !Interact.Instance.CombatMenu.CombatMenuObject.gameObject.activeInHierarchy)
        {
            Show(false, true);
        }
    }

    private void OnMouseExit()
    {
        if (!UnitManager.Instance.SetupFinished)
        {
            return;
        }

        if (!Interact.Instance.CombatMenu.AttackMenuObject.gameObject.activeInHierarchy 
            && !Interact.Instance.CombatMenu.CombatMenuObject.gameObject.activeInHierarchy
            && Interact.Instance.SelectedUnit == null)
        {
            if (Unit && !Interact.Instance.SelectedUnit)
            {
                Unit.HideAllChangedTiles();
            }
            else if (Unit)
            {
                if (Unit != Interact.Instance.SelectedUnit)
                {
                    Unit.HideAllChangedTiles();
                }
            }

            WhichColour(Interact.Instance.SelectedUnit);
            return;
        }

        WhichColour(Interact.Instance.SelectedUnit);
    }

    internal void WhichColour(UnitBase PassedUnit = null)
    {
        bool inRange;

        if(OGMaterial == null)
        {
            return;
        }

        if (Interact.Instance.SelectedUnit)
        {
            inRange = Interact.Instance.SelectedUnit.AttackTiles.Contains(this);
            if (inRange)
            {
                if (!Interact.Instance.SelectedUnit.MovedForTurn)
                {
                    Show(Interact.Instance.SelectedUnit.AttackTiles.Contains(this) && Interact.Instance.SelectedUnit.MoveableTiles.Contains(this) ? false : true, !inRange);
                    return;
                }
                else
                {
                    Show(true);
                    return;
                }
            }
        }
        else if(PassedUnit)
        {
            inRange = PassedUnit.AttackTiles.Contains(this);
            if (inRange)
            {
                if (!PassedUnit.MovedForTurn)
                {
                    Show(PassedUnit.AttackTiles.Contains(this) && PassedUnit.MoveableTiles.Contains(this) ? false : true, !inRange);
                    return;
                }
                else
                {
                    Show(true);
                    return;
                }
            }
        }

        Show(false, false, true);
    }
}
