using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Tutorial
{
    CMove,
    CAttack,
    CChangeWeapon,
    CUseItem,
    OMove,
    OEnterCombat,
    OInventory
}

public class ToolTip : ScriptableObject
{
    public Tutorial tutorial;
    [TextAreaAttribute]
    public string Text; 
}
