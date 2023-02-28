using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicOrb : InteractOnGrid
{
    public int Range;
    public int StayForTurns;
    internal int ActiveForTurns;
    public int MagicLevelMin;
    internal bool Active = false;
    public List<Tile> AoEArea;
    public int Damage;

    // Start is called before the first frame update
    void Start()
    {
        AoEArea = new List<Tile>();
    }

    // Update is called once per frame
    void Update()
    {
        if(StayForTurns < ActiveForTurns)
        {
            Active = false;
        }
    }

    public override void Special(UnitBase Unit)
    {
        if (MagicLevelMin <= Unit.MagicLevel)
        {
            Active = true;
            CalculateAoE(InteractLocations[TileManager.Instance.Grid[Unit.Position[0], Unit.Position[1]].GetComponent<Tile>()]);
        }
    }

    internal void DealDamage()
    {
        if (Active)
        {
            foreach (Tile tile in AoEArea)
            {
                if (tile.Unit)
                {
                    tile.Unit.ShowLongDistanceDamageNumbers(Damage);
                }
            }
        }
    }

    internal void ShowRange()
    {
        foreach (Tile Tile in AoEArea)
        {
            Tile.Show(true);
        }
    }

    internal void HideRange()
    {
        foreach (Tile Tile in AoEArea)
        {
            Tile.Show(false, false, true);
        }
    }

    internal override void CalculateAoE(Direction DirectionInteraction)
    {
        List<GameObject> Tiles = new List<GameObject>();
        Tiles.Add(TileManager.Instance.Grid[Position[0], Position[1]]);

        for(int i = 0; i < Range; i++)
        {
            Tiles = FindAoE(Tiles);
        }
    }

    internal List<GameObject> FindAoE(List<GameObject> tiles)
    {
        List<GameObject> NextLayer = new List<GameObject>();

        foreach (GameObject tile in tiles)
        {
            foreach (GameObject AdjacentTile in tile.GetComponent<Tile>().AdjacentTiles)
            {
                if (!NextLayer.Contains(AdjacentTile))
                {
                    NextLayer.Add(AdjacentTile);
                }

                if (!AoEArea.Contains(AdjacentTile.GetComponent<Tile>()))
                {
                    AoEArea.Add(AdjacentTile.GetComponent<Tile>());
                }

            }
        }

        return NextLayer;
    }

    private void OnMouseEnter()
    {
        if (!UnitManager.Instance.SetupFinished || !Interact.Instance.VirtualCam.activeInHierarchy || Options.Instance.OptionsMenuUI.activeInHierarchy
            || Interact.Instance.CombatMenu.VictoryScreen.activeInHierarchy || Interact.Instance.CombatMenu.DefeatScreen.activeInHierarchy)
        {
            return;
        }

        if (Active)
        {
            ShowRange();
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
            HideRange();
        }
    }
}
