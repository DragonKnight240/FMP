using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu ]
public class Class : ScriptableObject
{
    public string Name;
    public List<SpecialAttacks> Abilties;
    public List<int> LevelRequirement;
    public int Strength;
    public int Dexterity;
    public int Magic;
    public int Defence;
    public int Resistance;
    public int Speed;
    public int Luck;
}
