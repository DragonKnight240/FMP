using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemTypes
{
    Heal,
    Buff,
    ClassChanger,
    Weapon
}

[CreateAssetMenu(fileName = "Item")]
public class Item : ScriptableObject
{
    public string Name;
    public ItemTypes Type;
}
