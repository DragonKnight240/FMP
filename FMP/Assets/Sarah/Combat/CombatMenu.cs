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

    private void Start()
    {
        CombatMenuObject.SetActive(false);
        InventoryObject.SetActive(false);
        AttackMenuObject.SetActive(false);
        ItemNotification.SetActive(false);
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
            if (NewIndex < 0)
            {
                NewIndex = Interact.Instance.SelectedUnit.WeaponsIninventory.Count -1;
            }
        }

        Interact.Instance.SelectedUnit.EquipedWeapon = Interact.Instance.SelectedUnit.WeaponsIninventory[NewIndex];
        CheckTargetStatus();
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
            if (NewIndex < 0)
            {
                NewIndex = Interact.Instance.SelectedUnit.UnlockedAttacks.Count - 1;
            }
        }

        Interact.Instance.SelectedUnit.CurrentAttack = Interact.Instance.SelectedUnit.UnlockedAttacks[NewIndex];

        CheckTargetStatus();
    }

    public void CancelAttack()
    {
        AttackMenuObject.SetActive(false);
        CameraMove.Instance.FollowTarget = null;
        Interact.Instance.SelectedUnit.HideAllChangedTiles();
        Interact.Instance.SelectedUnit = null;
    }

    internal void CheckTargetStatus()
    {
        Interact.Instance.SelectedUnit.HideAllChangedTiles();
        Interact.Instance.SelectedUnit.MoveableArea();
        Interact.Instance.SelectedUnit.GetComponent<UnitControlled>().FindInRangeTargets();

        if (Interact.Instance.SelectedUnit.InRangeTargets.Count > 0)
        {
            if (!Interact.Instance.SelectedUnit.InRangeTargets.Contains(Interact.Instance.SelectedUnit.AttackTarget))
            {
                Interact.Instance.SelectedUnit.AttackTarget = Interact.Instance.SelectedUnit.InRangeTargets[0];
                CameraMove.Instance.FollowTarget = Interact.Instance.SelectedUnit.InRangeTargets[0].transform;
            }

            Interact.Instance.SelectedUnit.GetComponent<UnitControlled>().AttackDisplay();
        }
        else
        {
            CameraMove.Instance.FollowTarget = null;
            Interact.Instance.CombatMenu.AttackMenuObject.SetActive(false);
        }
    }

    internal void CheckButtons()
    {
        foreach (GameObject tile in TileManager.Instance.Grid[Interact.Instance.SelectedUnit.Position[0], Interact.Instance.SelectedUnit.Position[1]].GetComponent<Tile>().AdjacentTiles)
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

        Interact.Instance.SelectedUnit.GetComponent<UnitControlled>().FindInRangeTargets();

        if (Interact.Instance.SelectedUnit.GetComponent<UnitControlled>().InRangeTargets.Count > 0)
        {
            AttackButton.gameObject.SetActive(true);
        }
        else
        {
            AttackButton.gameObject.SetActive(false);
            print("None in Range");
        }

        if(Interact.Instance.SelectedUnit.MovedForTurn)
        {
            MoveButton.gameObject.SetActive(false);
        }
        else
        {
            MoveButton.gameObject.SetActive(true);
        }
    }
}
