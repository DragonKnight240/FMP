using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Special Attacks")]
public class SpecialAttacks : ScriptableObject
{
    public int DamageMultiplier;
    public int HitRateMultiplier;
    public int CritRateMultiplier;
    public int ProficiencyIncreaseMultiplier;
}
