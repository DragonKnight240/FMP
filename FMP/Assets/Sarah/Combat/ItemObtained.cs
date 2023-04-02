using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObtained : InteractOnGrid
{
    public Item Item;
    Animator Anim;

    private void Start()
    {
        if(Item == null)
        {
            print(name + " doesn't have an item to give thus has destroyed itself");
            Destroy(this.gameObject);
        }

        Anim = GetComponent<Animator>();

    }

    public override void Special(UnitBase Unit)
    {
        if (Anim)
        {
            Anim.speed = 1;
        }

        Interact.Instance.CombatMenu.ItemText.text = Item.Name;
        Interact.Instance.CombatMenu.ItemNotification.SetActive(true);

        Unit.Inventory.Add(Instantiate(Item));

        if(Item.Type == ItemTypes.Weapon)
        {
            Unit.WeaponsIninventory.Add(Instantiate((Weapon)Item));
        }

        TileManager.Instance.Grid[Position[0], Position[1]].GetComponent<Tile>().CanMoveOn = true;
        TileManager.Instance.Grid[Position[0], Position[1]].GetComponent<Tile>().Occupied = false;
        Unit.WaitUnit();
        Destroy(this.gameObject);
    }
}
