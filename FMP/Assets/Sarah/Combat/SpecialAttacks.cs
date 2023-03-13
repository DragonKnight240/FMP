using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Effects
{
    Movement,
    DamageOverTime,
    None
}

[CreateAssetMenu(fileName = "Special Attacks")]
public class SpecialAttacks : ScriptableObject
{
    public string Name;
    public WeaponType WeaponType;
    [Header("Multipliers")]
    public int DamageMultiplier;
    public int HitRateMultiplier;
    public int CritRateMultiplier;
    public int ProficiencyIncreaseMultiplier;
    public int DurabilityMultiplier;
    public bool isAOE = false;

    [Header("Effects")]
    public UnitBase.StatusEffect StatusEffect;

    internal void Effect(UnitBase unit)
    {
        unit.CurrentStatusEffects.Add(StatusEffect);
    }
}
