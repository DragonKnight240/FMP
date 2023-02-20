using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu ]
public class Class : ScriptableObject
{
    public string Name;
    public List<SpecialAttacks> Abilties;
    public List<int> LevelRequirement;

    public int Level;
    public int EXP;

    public List<int> TotalEXPNeeded;

    [Header("Stats")]
    public int Strength;
    public int Dexterity;
    public int Magic;
    public int Defence;
    public int Resistance;
    public int Speed;
    public int Luck;

    [Header("Growth Rates")]
    public int HPGrowthRate;
    public int StrengthGrowthRate;
    public int DexterityGrowthRate;
    public int MagicGrowthRate;
    public int DefenceGrowthRate;
    public int ResistanceGrowthRate;
    public int SpeedStrengthGrowthRate;
    public int LuckGrowthRate;

    internal void FindLevel()
    {
        foreach(int LevelEXP in TotalEXPNeeded)
        {
            if(LevelEXP >= EXP)
            {
                Level += 1;
            }
        }
    }

    internal void AbilityUnlock(UnitBase Unit)
    {
        int Index = 0;

        foreach(SpecialAttacks Ability in Abilties)
        {
            if(Level >= LevelRequirement[Index])
            {
                if(!Unit.UnlockedAttacks.Contains(Ability))
                {
                    Unit.UnlockedAttacks.Add(Ability);
                }
            }

            Index++;
        }
    }
}
