using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractOnGrid : MonoBehaviour
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    internal int[] Position;
    internal List<GameObject> AoETiles;
    internal List<GameObject> ActiveTiles;
    public Direction DirectionInteraction;
    public int HitAmountWidth;
    public int HitAmountHeight;

    private void Start()
    {
        Position = new int[2];
    }

    public virtual void Special(UnitBase Unit)
    {

    }

    internal virtual void CalculateAoE()
    {
        
    }
}
