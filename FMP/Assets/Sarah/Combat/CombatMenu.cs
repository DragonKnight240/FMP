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

    //Item Notification
    public TMP_Text ItemText;
    public GameObject ItemNotification;
    public float ItemStayTime = 5.0f;
    float ItemStayTimer = 0;

    //Selected
    public GameObject SelectedUnitTab;
    public TMP_Text UnitText;

    private void Start()
    {
        CombatMenuObject.SetActive(false);
        InventoryObject.SetActive(false);
        AttackMenuObject.SetActive(false);
        ItemNotification.SetActive(false);
        SelectedUnitTab.SetActive(false);
    }

    private void Update()
    {
        if(ItemNotification.activeInHierarchy)
        {
            ItemStayTimer += Time.deltaTime;

            if(ItemStayTime <= ItemStayTimer)
            {
                ItemNotification.SetActive(false);
                ItemStayTimer = 0;
            }

        }
    }

    public void CloseInventory()
    {
        InventoryObject.SetActive(false);
    }

    public void ChangeAttackTarget(bool Next)
    {
        UnitBase Unit = Interact.Instance.SelectedUnit;
        int NewIndex;
        if (Next)
        {
            NewIndex = Unit.InRangeTargets.IndexOf(Unit.AttackTarget) + 1;
            if (NewIndex >= Unit.InRangeTargets.Count)
            {
                NewIndex = 0;
            }
        }
        else
        {
            NewIndex = Unit.InRangeTargets.IndexOf(Unit.AttackTarget) - 1;
            if (NewIndex < 0)
            {
                NewIndex = Unit.InRangeTargets.Count - 1;
            }
        }

        Unit.AttackTarget = Unit.InRangeTargets[NewIndex];
        Unit.GetComponent<UnitControlled>().AttackDisplay();
        CameraMove.Instance.FollowTarget = Unit.AttackTarget.transform;
    }

    public void ChangeWeapon(bool Next)
    {
        UnitBase Unit = Interact.Instance.SelectedUnit;
        List<Weapon> Weapons = Unit.WeaponsIninventory;

        int NewIndex;
        if(Next)
        {
            NewIndex = Weapons.IndexOf(Unit.EquipedWeapon) + 1;
            if(NewIndex >= Weapons.Count)
            {
                NewIndex = 0;
            }
        }
        else
        {
            NewIndex = Weapons.IndexOf(Unit.EquipedWeapon) - 1;
            if (NewIndex < 0)
            {
                NewIndex = Unit.WeaponsIninventory.Count -1;
            }
        }

        Unit.EquipedWeapon = Weapons[NewIndex];
        ChangeAvailableAttacks();
        CheckTargetStatus();
    }

    public void ChangeAttack(bool Next)
    {
        UnitBase Unit = Interact.Instance.SelectedUnit;
        List<SpecialAttacks> AttackList = Unit.AvailableAttacks;

        int NewIndex;
        if (Next)
        {
            NewIndex = AttackList.IndexOf(Unit.CurrentAttack) + 1;
            if (NewIndex >= AttackList.Count)
            {
                NewIndex = 0;
            }
        }
        else
        {
            NewIndex = AttackList.IndexOf(Unit.CurrentAttack) - 1;
            if (NewIndex < 0)
            {
                NewIndex = AttackList.Count - 1;
            }
        }

        Unit.CurrentAttack = AttackList[NewIndex];

        CheckTargetStatus();
    }

    internal void ChangeAvailableAttacks()
    {
        UnitBase Unit = Interact.Instance.SelectedUnit;

        Unit.AvailableAttacks = new List<SpecialAttacks>();

        foreach (SpecialAttacks Attack in Unit.UnlockedAttacks)
        {
            if (Attack.WeaponType == Unit.EquipedWeapon.WeaponType)
            {
                Unit.AvailableAttacks.Add(Attack);
            }
        }

        Unit.AvailableAttacks.Add(Unit.EquipedWeapon.Special);

        Unit.CurrentAttack = Unit.AvailableAttacks[0];
    }

    public void CancelAttack()
    {
        AttackMenuObject.SetActive(false);
        CameraMove.Instance.FollowTarget = null;
        Interact.Instance.SelectedUnit.HideAllChangedTiles();
        Interact.Instance.SelectedUnit = null;
        Interact.Instance.UISelectedUnit();
    }

    internal void CheckTargetStatus()
    {
        UnitBase Unit = Interact.Instance.SelectedUnit;

        Unit.HideAllChangedTiles();
        Unit.MoveableArea();
        Unit.GetComponent<UnitControlled>().FindInRangeTargets();

        if (Unit.InRangeTargets.Count > 0)
        {
            if (!Unit.InRangeTargets.Contains(Unit.AttackTarget))
            {
                Unit.AttackTarget = Unit.InRangeTargets[0];
                CameraMove.Instance.FollowTarget = Unit.InRangeTargets[0].transform;
            }

            Unit.GetComponent<UnitControlled>().AttackDisplay();
        }
        else
        {
            CameraMove.Instance.FollowTarget = null;
            Interact.Instance.CombatMenu.AttackMenuObject.SetActive(false);
        }
    }

    internal void CheckButtons()
    {
        UnitBase Unit = Interact.Instance.SelectedUnit;

        foreach (GameObject tile in TileManager.Instance.Grid[Unit.Position[0], Unit.Position[1]].GetComponent<Tile>().AdjacentTiles)
        {
            if (tile.GetComponent<Tile>().Special)
            {
                SpecialButton.gameObject.SetActive(true);
                break;
            }
            else
            {
                SpecialButton.gameObject.SetActive(false);
            }
        }

        Unit.GetComponent<UnitControlled>().FindInRangeTargets();

        if (Unit.GetComponent<UnitControlled>().InRangeTargets.Count > 0)
        {
            AttackButton.gameObject.SetActive(true);
        }
        else
        {
            AttackButton.gameObject.SetActive(false);
        }

        if(Unit.MovedForTurn)
        {
            MoveButton.gameObject.SetActive(false);
        }
        else
        {
            MoveButton.gameObject.SetActive(true);
        }
    }
}
