using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Supports
{
    public SupportIncrease Stat;
    public int Increase;
}

[CreateAssetMenu(fileName = "Item")]
public class UnitSupports : ScriptableObject
{
    public GameObject UnitObj;
    internal UnitBase Unit;
    public int Level;
    public int EXP;
    public List<int> EXPNeeded;
    public List<Supports> SupportStats;
}
