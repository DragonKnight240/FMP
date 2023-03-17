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
    public AudioClip ActivateSound;
    public AudioClip DeactivateSound;
    public AudioClip DamageSound;

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
            ActiveForTurns = 0;
            if (DeactivateSound)
            {
                //print("Deactive sound " + gameObject);
                SoundManager.Instance.PlaySFX(DeactivateSound);
            }
        }
    }

    public override void Special(UnitBase Unit)
    {
        if (Unit.Class.Name == ClassNeeded.Name)
        {
            UnitToActiveIt = Unit;
            Active = true;
            CalculateAoE(InteractLocations[TileManager.Instance.Grid[Unit.Position[0], Unit.Position[1]].GetComponent<Tile>()]);
            if (ActivateSound)
            {
                //print("Active " + gameObject);
                SoundManager.Instance.PlaySFX(ActivateSound);
            }
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
                    if(UnitToActiveIt.MagicLevel == 5)
                    {
                        if(tile.Unit.CompareTag(UnitToActiveIt.tag))
                        {
                            continue;
                        }
                    }

                    if (DamageSound)
                    {
                        //print("Damage sound " + gameObject);
                        SoundManager.Instance.PlaySFX(DamageSound);
                    }
                    tile.Unit.ShowLongDistanceDamageNumbers(Damage + UnitToActiveIt.RankBonus[UnitToActiveIt.MagicLevel] - tile.Unit.CalculateMagicDefence(WeaponType.Staff));
                }
            }

            ActiveForTurns++;
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
            Tile.WhichColour();
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

        if (Active && Interact.Instance.SelectedUnit == null)
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
