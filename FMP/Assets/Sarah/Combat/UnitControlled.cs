using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitControlled : UnitBase
{
    internal void MoveButton()
    {
        //Interact.Instance.CombatMenu.CombatMenuObject.SetActive(false);
        Interact.Instance.CombatMenu.CombatMenuObject.GetComponent<UIFade>().ToFadeOut();
    }

    internal void AttackButton(UnitBase Unit = null)
    {
        FindInRangeTargets();

        if (InRangeTargets.Count > 0)
        {
            if (Unit)
            {
                if (InRangeTargets.Contains(Unit))
                {
                    CameraMove.Instance.FollowTarget = Unit.transform;
                    AttackTarget = Unit;
                }
                else
                {
                    return;
                }
            }
            else
            {
                CameraMove.Instance.FollowTarget = InRangeTargets[0].transform;
                AttackTarget = InRangeTargets[0];
            }
            FindWeapons();
            AttackDisplay();
            Interact.Instance.CombatMenu.AttackMenuObject.SetActive(true);
            Interact.Instance.CombatMenu.AttackMenuObject.GetComponent<UIFade>().ToFadeIn();

            //Interact.Instance.CombatMenu.CombatMenuObject.SetActive(false);
            Interact.Instance.CombatMenu.CombatMenuObject.GetComponent<UIFade>().ToFadeOut();
        }
        else
        {
            //Interact.Instance.CombatMenu.CombatMenuObject.SetActive(false);
            Interact.Instance.CombatMenu.CombatMenuObject.GetComponent<UIFade>().ToFadeOut();
        }
    }

    internal void ItemButton()
    {
        FindObjectOfType<Inventory>().ButtonText(Inventory);
        Interact.Instance.CombatMenu.InventoryObject.SetActive(true);
        Interact.Instance.CombatMenu.InventoryObject.GetComponent<UIFade>().ToFadeIn();

    }

    internal void SpecialButton()
    {
        //Interact.Instance.CombatMenu.CombatMenuObject.SetActive(false);
        Interact.Instance.CombatMenu.CombatMenuObject.GetComponent<UIFade>().ToFadeOut();

        foreach (GameObject tile in TileManager.Instance.Grid[Position[0], Position[1]].GetComponent<Tile>().AdjacentTiles)
        {
            if (tile.GetComponent<Tile>().Special)
            {
                tile.GetComponent<Tile>().Special.Special(this);
                UnitManager.Instance.UnitUpdate.Invoke();
                Interact.Instance.SelectedUnit = null;
                Interact.Instance.UISelectedUnit();
                CameraMove.Instance.FollowTarget = null;
                HideAllChangedTiles();
                EndTurn = true;
            }
        }

        CameraMove.Instance.FollowTarget = null;
    }

    internal void FindWeapons()
    {
        foreach(Item item in Inventory)
        {
            if(item.Type == ItemTypes.Weapon)
            {
                WeaponsIninventory.Add((Weapon)item);
            }
        }
    }

    internal void FindInRangeTargets()
    {
        InRangeTargets.Clear();

        if(MovedForTurn)
        {
            List<GameObject> Tiles = new List<GameObject>();
            Tiles.Add(TileManager.Instance.Grid[Position[0], Position[1]]);
            AttackTiles.Clear();

            for (int i = 0; i < EquipedWeapon.Range; i++)
            {
                Tiles = WeaponRangeAttack(Tiles, true);
            }
        }

        foreach (Tile tile in AttackTiles)
        {
            if (tile.Unit)
            {
                if (!InRangeTargets.Contains(tile.Unit))
                {
                    if (tile.Unit.CompareTag("Enemy"))
                    {
                        if (tile.Unit.isAlive)
                        {
                            InRangeTargets.Add(tile.Unit);
                        }
                    }
                }
            }
        }
    }

    internal void AttackDisplay()
    {
        if(Interact.Instance.SelectedUnit.WeaponsIninventory.Count == 0)
        {
            Interact.Instance.CombatMenu.NextWeapon.SetActive(false);
            Interact.Instance.CombatMenu.PreviousWeapon.SetActive(false);
        }
        else
        {
            Interact.Instance.CombatMenu.NextWeapon.SetActive(true);
            Interact.Instance.CombatMenu.PreviousWeapon.SetActive(true);
        }

        if(Interact.Instance.SelectedUnit.AvailableAttacks.Count <= 1 )
        {
            Interact.Instance.CombatMenu.NextAttack.SetActive(false);
            Interact.Instance.CombatMenu.PreviousAttack.SetActive(false);
        }
        else
        {
            Interact.Instance.CombatMenu.NextAttack.SetActive(true);
            Interact.Instance.CombatMenu.PreviousAttack.SetActive(true);
        }

        if(Interact.Instance.SelectedUnit.InRangeTargets.Count <= 1)
        {
            Interact.Instance.CombatMenu.NextTarget.SetActive(false);
            Interact.Instance.CombatMenu.PreviousTarget.SetActive(false);
        }
        else
        {
            Interact.Instance.CombatMenu.NextTarget.SetActive(true);
            Interact.Instance.CombatMenu.PreviousTarget.SetActive(true);
        }

        Interact.Instance.CombatMenu.HealthAlly.value = (float)(CurrentHealth - AttackTarget.CalculateReturnDamage()) / HealthMax;
        Interact.Instance.CombatMenu.Weapon.text = EquipedWeapon.Name;
        Interact.Instance.CombatMenu.Attack.text = CurrentAttack.Name;
        Interact.Instance.CombatMenu.DamageAlly.text = CalculateDamage().ToString();
        Interact.Instance.CombatMenu.HitAlly.text = CalcuateHitChance().ToString();
        Interact.Instance.CombatMenu.CritAlly.text = CalculateCritChance().ToString();

        Interact.Instance.CombatMenu.HealthEnemy.value = (float)(AttackTarget.CurrentHealth - CalculateDamage()) / AttackTarget.HealthMax;
        Interact.Instance.CombatMenu.DamageEnemy.text = AttackTarget.CalculateReturnDamage().ToString();
        Interact.Instance.CombatMenu.HitEnemy.text = AttackTarget.CalculateReturnHitChance().ToString();
        Interact.Instance.CombatMenu.CritEnemy.text = AttackTarget.CalculateReturnCritChance().ToString();
    }
}
