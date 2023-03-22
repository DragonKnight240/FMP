using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Tutorial
{
    CUnitSelect,
    CMove,
    CAttack,
    CChangeWeapon,
    CUseItem,
    CWeaponAbility,
    CGridInteraction,
    CWait,
    OMove,
    OEnterCombat,
    OInventory,
    CMoveCamera,
    OIntentory
}

[CreateAssetMenu(fileName = "Tutorial")]

public class ToolTip : ScriptableObject
{
    public Tutorial tutorial;
    [TextAreaAttribute]
    public string Text; 
}
