using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour
{
    public List<Button> Items;

    internal Dictionary<Button, Item> ButtonDictionary;

    public TMP_Text WeaponText;
    public GameObject NextWeapon;
    public GameObject PreviousWeapon;

    private void Update()
    {
        if (Interact.Instance.SelectedUnit)
        {
            if (Interact.Instance.SelectedUnit.EquipedWeapon.Name != WeaponText.text)
            {
                WeaponText.text = Interact.Instance.SelectedUnit.EquipedWeapon.Name;

                if (Interact.Instance.SelectedUnit.WeaponsIninventory.Count > 1)
                {
                    NextWeapon.SetActive(true);
                    PreviousWeapon.SetActive(true);
                }
                else
                {
                    NextWeapon.SetActive(false);
                    PreviousWeapon.SetActive(false);
                }
            }
        }
    }

    internal void ButtonText(List<Item> UnitInventory)
    {
        for(int i = 0; i < Items.Count; i++)
        {
            if(i < UnitInventory.Count)
            {
                Items[i].GetComponentInChildren<Text>().text = UnitInventory[i].Name;
                Items[i].onClick.RemoveAllListeners();

                int index = i;
                Items[i].onClick.AddListener(() => UseItem(UnitInventory[index]));
                Items[i].interactable = true;
            }
            else
            {
                Items[i].GetComponentInChildren<Text>().text = "";
                Items[i].interactable = false;
            }
        }
    }

    public void UseItem(Item item)
    {
        switch(item.Type)
        {
            case ItemTypes.Heal:
                {
                    HealItem HealItem = (HealItem)item;
                    Interact.Instance.SelectedUnit.IncreaseHealth(HealItem.HealValue);
                    Interact.Instance.SelectedUnit.Inventory.Remove(item);
                    ButtonText(Interact.Instance.SelectedUnit.Inventory);
                    break;
                }
            case ItemTypes.Weapon:
                {
                    ChangeWeapon(item);
                    break;
                }
        }
    }

    public void ChangeWeapon(Item item)
    {
        if (Interact.Instance.SelectedUnit.EquipedWeapon != item)
        {
            Interact.Instance.SelectedUnit.EquipedWeapon = (Weapon)item;
        }
        else
        {
            Interact.Instance.SelectedUnit.EquipedWeapon = Interact.Instance.SelectedUnit.BareHands;
        }

        Interact.Instance.SelectedUnit.MoveableArea();
        Interact.Instance.SelectedUnit.ShowAllInRangeTiles();
    }
}
