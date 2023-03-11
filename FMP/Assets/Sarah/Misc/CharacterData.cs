using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterData
{
    public string UnitName;
    public int HealthMax;
    public int CurrentHealth;
    public int Movement;

    //Inventory
    public List<Item> Inventory;

    //Stats
    public int Level;
    public int EXP;
    public int Strength;
    public int Dexterity;
    public int Magic;
    public int Defence;
    public int Resistance;
    public int Speed;
    public int Luck;

    //Weapon Proficientcy
    public float BowProficiency;
    public int BowLevel;

    public float SwordProficiency;
    public int SwordLevel;

    public float MagicProficiency;
    public int MagicLevel;

    public float FistProficiency;
    public int FistLevel;

    //Class
    public Class Class;

    //Attack
    public List<SpecialAttacks> UnlockedAttacks;

    //Supports
    public List<UnitSupports> Supports;
}
