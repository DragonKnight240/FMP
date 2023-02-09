using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObtained : InteractOnGrid
{
    public Item Item;

    private void Start()
    {
        if(Item == null)
        {
            print(name + " doesn't have an item to give thus has destroyed itself");
            Destroy(this.gameObject);
        }

    }

    public override void Special(UnitBase Unit)
    {
        Interact.Instance.CombatMenu.ItemText.text = Item.Name;
        Interact.Instance.CombatMenu.ItemNotification.SetActive(true);

        Unit.Inventory.Add(Item);

        if(Item.Type == ItemTypes.Weapon)
        {
            Unit.WeaponsIninventory.Add((Weapon)Item);
        }

        TileManager.Instance.Grid[Position[0], Position[1]].GetComponent<Tile>().CanMoveOn = true;
        TileManager.Instance.Grid[Position[0], Position[1]].GetComponent<Tile>().Occupied = false;
        Unit.EndTurn = true;
        Destroy(this.gameObject);
    }
}
