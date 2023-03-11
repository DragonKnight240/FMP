using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum WeaponType
{
    Sword,
    Bow,
    Gauntlets,
    Staff,
    None
}

[CreateAssetMenu(fileName = "Weapon")]
public class Weapon : Item
{
    public int Damage;
    public int Range;
    public int Durablity;
    internal int CurrentDurablity;
    public int HitRate;
    public int CritRate;
    public SpecialAttacks Special;
    public int ProficiencyIncrease;
    public WeaponType WeaponType;
    public Sprite WeaponImage;
}
