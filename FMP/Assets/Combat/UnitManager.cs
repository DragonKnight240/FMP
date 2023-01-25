using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    [System.Serializable]
    public struct StartingPositions
    {
        public GameObject Unit;
        public int X;
        public int Y;
    }

    public static UnitManager Instance;
    public List<StartingPositions> UnitPositions;
    internal List<GameObject> UnitsInGame;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        UnitsInGame = new List<GameObject>();
    }

    public void PlaceUnits()
    {
        int X;
        int Y;

        foreach (StartingPositions Position in UnitPositions)
        {
            X = Position.X;
            Y = Position.Y;

            if (X > TileManager.Instance.Width)
            {
                X = TileManager.Instance.Width;
            }
            else if(X < 0)
            {
                X = 0;
            }

            if (Y > TileManager.Instance.Height)
            {
                Y = TileManager.Instance.Height;
            }
            else if (Y < 0)
            {
                Y = 0;
            }

            GameObject NewUnit = Instantiate(Position.Unit, TileManager.Instance.Grid[X, Y].GetComponent<Tile>().CentrePoint.transform.position, Quaternion.identity, transform);
            NewUnit.GetComponent<UnitBase>().Position = new int[2];
            NewUnit.GetComponent<UnitBase>().Position[0] = X;
            NewUnit.GetComponent<UnitBase>().Position[1] = Y;
            TileManager.Instance.Grid[X, Y].GetComponent<Tile>().ChangeOccupant(NewUnit.GetComponent<UnitBase>());
            UnitsInGame.Add(NewUnit);
        }
    }
}
