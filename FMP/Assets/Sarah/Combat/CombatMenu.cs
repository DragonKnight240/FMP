using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombatMenu : MonoBehaviour
{
    public GameObject CombatMenuObject;
    public GameObject AttackMenuObject;

    //Combat Menu Buttons
    public Button SpecialButton;
    public Button MoveButton;
    public Button AttackButton;
    public Button ItemButton;
    public Button WaitButton;

    //Attack Menu Buttons
    public TMP_Text Weapon;
    public TMP_Text Attack;

    public Slider HealthAlly;
    public TMP_Text DamageAlly;
    public TMP_Text HitAlly;
    public TMP_Text CritAlly;

    public Slider HealthEnemy;
    public TMP_Text DamageEnemy;
    public TMP_Text HitEnemy;
    public TMP_Text CritEnemy;

    public GameObject NextTarget;
    public GameObject PreviousTarget;

    public GameObject NextAttack;
    public GameObject PreviousAttack;

    public GameObject NextWeapon;
    public GameObject PreviousWeapon;

    //Inventory
    public GameObject InventoryObject;

    private void Start()
    {
        CombatMenuObject.SetActive(false);
        InventoryObject.SetActive(false);
        AttackMenuObject.SetActive(false);
    }

    public void CloseInventory()
    {
        InventoryObject.SetActive(false);
    }

    public void ChangeAttackTarget(bool Next)
    {
        int NewIndex;
        if (Next)
        {
            NewIndex = Interact.Instance.SelectedUnit.InRangeTargets.IndexOf(Interact.Instance.SelectedUnit.AttackTarget) + 1;
            if (NewIndex >= Interact.Instance.SelectedUnit.InRangeTargets.Count)
            {
                NewIndex = 0;
            }
        }
        else
        {
            NewIndex = Interact.Instance.SelectedUnit.InRangeTargets.IndexOf(Interact.Instance.SelectedUnit.AttackTarget) - 1;
            if (NewIndex < 0)
            {
                NewIndex = Interact.Instance.SelectedUnit.InRangeTargets.Count - 1;
            }
        }

        Interact.Instance.SelectedUnit.AttackTarget = Interact.Instance.SelectedUnit.InRangeTargets[NewIndex];
        Interact.Instance.SelectedUnit.GetComponent<UnitControlled>().AttackDisplay();
        CameraMove.Instance.FollowTarget = Interact.Instance.SelectedUnit.AttackTarget.transform;
    }

    public void ChangeWeapon(bool Next)
    {
        int NewIndex;
        if(Next)
        {
            NewIndex = Interact.Instance.SelectedUnit.WeaponsIninventory.IndexOf(Interact.Instance.SelectedUnit.EquipedWeapon) + 1;
            if(NewIndex >= Interact.Instance.SelectedUnit.WeaponsIninventory.Count)
            {
                NewIndex = 0;
            }
        }
        else
        {
            NewIndex = Interact.Instance.SelectedUnit.WeaponsIninventory.IndexOf(Interact.Instance.SelectedUnit.EquipedWeapon) - 1;
            if (NewIndex <= 0)
            {
                NewIndex = Interact.Instance.SelectedUnit.WeaponsIninventory.Count -1;
            }
        }

        Interact.Instance.SelectedUnit.EquipedWeapon = Interact.Instance.SelectedUnit.WeaponsIninventory[NewIndex];
        Interact.Instance.SelectedUnit.GetComponent<UnitControlled>().AttackDisplay();
        Interact.Instance.SelectedUnit.MoveableArea();
    }

    public void ChangeAttack(bool Next)
    {
        int NewIndex;
        if (Next)
        {
            NewIndex = Interact.Instance.SelectedUnit.UnlockedAttacks.IndexOf(Interact.Instance.SelectedUnit.CurrentAttack) + 1;
            if (NewIndex >= Interact.Instance.SelectedUnit.UnlockedAttacks.Count)
            {
                NewIndex = 0;
            }
        }
        else
        {
            NewIndex = Interact.Instance.SelectedUnit.UnlockedAttacks.IndexOf(Interact.Instance.SelectedUnit.CurrentAttack) - 1;
            if (NewIndex <= 0)
            {
                NewIndex = Interact.Instance.SelectedUnit.UnlockedAttacks.Count - 1;
            }
        }

        Interact.Instance.SelectedUnit.CurrentAttack = Interact.Instance.SelectedUnit.UnlockedAttacks[NewIndex];
        Interact.Instance.SelectedUnit.GetComponent<UnitControlled>().AttackDisplay();
    }
}
