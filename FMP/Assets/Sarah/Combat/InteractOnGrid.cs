using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}

public class InteractOnGrid : MonoBehaviour
{
    internal int[] Position;
    internal List<GameObject> ActiveTiles;
    internal Dictionary<Tile, Direction> InteractLocations;
    internal UnitBase UnitToActiveIt;

    private void Start()
    {
        Position = new int[2];
        InteractLocations = new Dictionary<Tile, Direction>();
    }
    private void Update()
    {
        
    }

    public virtual void Special(UnitBase Unit)
    {

    }

    internal virtual void CalculateAoE(Direction DirectionInteraction)
    {
        
    }

    internal void TileSetter(List<GameObject> tiles)
    {
        InteractLocations = new Dictionary<Tile, Direction>();
        foreach(GameObject tile in tiles)
        {
            if(tile.GetComponent<Tile>().GridPosition[0] > Position[0])
            {
                InteractLocations.Add(tile.GetComponent<Tile>(), Direction.Right);
            }
            else if(tile.GetComponent<Tile>().GridPosition[0] < Position[0])
            {
                InteractLocations.Add(tile.GetComponent<Tile>(), Direction.Left);
            }
            else if (tile.GetComponent<Tile>().GridPosition[1] < Position[1])
            {
                InteractLocations.Add(tile.GetComponent<Tile>(), Direction.Down);
            }
            else if (tile.GetComponent<Tile>().GridPosition[1] > Position[1])
            {
                InteractLocations.Add(tile.GetComponent<Tile>(), Direction.Up);
            }
        }
    }
}
