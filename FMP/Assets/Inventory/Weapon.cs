using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon")]
public class Weapon : Item
{
    public int Damage;
    public int Range;
    public int Durablity;
    public int HitRate;
    public int CritRate;
    public SpecialAttacks Special;
    public int ProficiencyIncrease;
    //public WeaponType Type;
}
