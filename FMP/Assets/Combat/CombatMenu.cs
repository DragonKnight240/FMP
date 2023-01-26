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

    private void Start()
    {
        CombatMenuObject.SetActive(false);
    }
}
