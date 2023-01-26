using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombatMenu : MonoBehaviour
{
    public GameObject CombatMenuObject;
    public Button MoveButton;
    public Button AttackButton;
    public Button ItemButton;
    public Button WaitButton;

    public GameObject InventoryObject;

    private void Start()
    {
        CombatMenuObject.SetActive(false);
        InventoryObject.SetActive(false);
    }

    public void DeactivateInventory()
    {
        InventoryObject.SetActive(false);
    }

    internal void DeactiveButton()
    {
        MoveButton.interactable = false;
    }
}
