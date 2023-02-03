using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitControlled : UnitBase
{
    internal void MoveButton()
    {

        Interact.Instance.CombatMenu.CombatMenuObject.SetActive(false);
    }

    internal void AttackButton()
    {
        FindInRangeTargets();

        if (InRangeTargets.Count > 0)
        {
            CameraMove.Instance.FollowTarget = InRangeTargets[0].transform;
            AttackTarget = InRangeTargets[0];
            AttackDisplay();
            Interact.Instance.CombatMenu.AttackMenuObject.SetActive(true);
            Interact.Instance.CombatMenu.CombatMenuObject.SetActive(false);
        }
        else
        {
            print("No in range targets");
            Interact.Instance.CombatMenu.CombatMenuObject.SetActive(false);
        }
    }

    internal void ItemButton()
    {
        FindObjectOfType<Inventory>().ButtonText(Inventory);
        Interact.Instance.CombatMenu.InventoryObject.SetActive(true);

    }

    internal void SpecialButton()
    {
        Interact.Instance.CombatMenu.CombatMenuObject.SetActive(false);

        foreach (GameObject tile in TileManager.Instance.Grid[Position[0], Position[1]].GetComponent<Tile>().AdjacentTiles)
        {
            if (tile.GetComponent<Tile>().Special)
            {
                tile.GetComponent<Tile>().Special.Special(this);
                EndTurn = true;
            }
        }
    }

    internal void FindInRangeTargets()
    {
        foreach (Tile tile in AttackTiles)
        {
            if (tile.Unit)
            {
                if (tile.Unit.CompareTag("Enemy"))
                {
                    InRangeTargets.Add(tile.Unit);
                }
            }
        }
    }

    internal void AttackDisplay()
    {
        Interact.Instance.CombatMenu.HealthAlly.value = (float)(CurrentHealth - AttackTarget.CalculateReturnDamage()) / HealthMax;
        Interact.Instance.CombatMenu.DamageAlly.text = CalculateDamage().ToString();
        Interact.Instance.CombatMenu.HitAlly.text = CalcuateHitChance().ToString();
        Interact.Instance.CombatMenu.CritAlly.text = CalculateCritChance().ToString();

        Interact.Instance.CombatMenu.HealthEnemy.value = (float)(AttackTarget.CurrentHealth - CalculateDamage()) / AttackTarget.HealthMax;
        Interact.Instance.CombatMenu.DamageEnemy.text = AttackTarget.CalculateReturnDamage().ToString();
        Interact.Instance.CombatMenu.HitEnemy.text = AttackTarget.CalculateReturnHitChance().ToString();
        Interact.Instance.CombatMenu.CritEnemy.text = AttackTarget.CalculateReturnCritChance().ToString();
    }
}
