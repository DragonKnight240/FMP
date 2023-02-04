using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnitManager : MonoBehaviour
{
    public enum UnitType
    {
        Ally,
        Enemy
    }

    [System.Serializable]
    public struct StartingPositions
    {
        public UnitType UnitType;
        public GameObject Unit;
        public int X;
        public int Y;
    }

    public static UnitManager Instance;
    public List<StartingPositions> UnitPositions;
    internal List<GameObject> AllyUnits;
    internal List<GameObject> EnemyUnits;
    internal List<UnitBase> DeadEnemyUnits;
    internal List<UnitBase> DeadAllyUnits;
    public string OverWorldScene;
    internal bool SetupFinished = false;
    internal UnityEvent UnitUpdate;


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

        AllyUnits = new List<GameObject>();
        EnemyUnits = new List<GameObject>();
        DeadEnemyUnits = new List<UnitBase>();
        DeadAllyUnits = new List<UnitBase>();
        UnitUpdate = new UnityEvent();
        UnitUpdate.AddListener(print);
    }

    void print()
    {
        print("Played");
    }

    private void Update()
    {
        if (!TurnManager.Instance.isPlayerTurn)
        {
            AIUnitMove();
        }

        if (SetupFinished)
        {
            if (DeadEnemyUnits.Count == EnemyUnits.Count)
            {
                //win
                print("Win");
                EndingCombat();
            }
            else if (DeadAllyUnits.Count == AllyUnits.Count)
            {
                //lose
                print("Lose");
                RestartCombat();
            }
        }
    }

    void AIUnitMove()
    {
        foreach (GameObject Enemy in EnemyUnits)
        {
            if (!Enemy.GetComponent<UnitAI>().MovedForTurn)
            {
                Enemy.GetComponent<UnitAI>().MoveUnit();
                break;
            }
        }
    }

    public void PlaceUnits()
    {
        int X;
        int Y;

        int Index = 0;

        foreach (StartingPositions Position in UnitPositions)
        {
            X = Position.X;
            Y = Position.Y;

            if (X > TileManager.Instance.Width)
            {
                X = TileManager.Instance.Width;
            }
            else if (X < 0)
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

            GameObject NewUnit;

            if (Position.UnitType == UnitType.Ally)
            {
                if (Index <= GameManager.Instance.MaxUnits + GameManager.Instance.NumRecruited)
                {
                    NewUnit = Instantiate(GameManager.Instance.ControlledUnits[Index], TileManager.Instance.Grid[X, Y].GetComponent<Tile>().CentrePoint.transform.position, Quaternion.identity, transform);
                    Index++;
                    AllyUnits.Add(NewUnit);
                    TurnManager.Instance.TurnChange.AddListener(NewUnit.GetComponent<UnitBase>().TurnChange);

                }
                else
                {
                    print("Skip Unit");
                    continue;
                }
            }
            else
            {
                NewUnit = Instantiate(Position.Unit, TileManager.Instance.Grid[X, Y].GetComponent<Tile>().CentrePoint.transform.position, Quaternion.identity, transform);
                EnemyUnits.Add(NewUnit);
                TurnManager.Instance.TurnChange.AddListener(NewUnit.GetComponent<UnitAI>().TurnChange);
            }

            UnitUpdate.AddListener(() => { NewUnit.GetComponent<UnitBase>().MoveableArea(false); });
            NewUnit.GetComponent<UnitBase>().Position = new int[2];
            NewUnit.GetComponent<UnitBase>().Position[0] = X;
            NewUnit.GetComponent<UnitBase>().Position[1] = Y;
            TileManager.Instance.Grid[X, Y].GetComponent<Tile>().ChangeOccupant(NewUnit.GetComponent<UnitBase>());
        }

        SetupFinished = true;
    }

    internal void RestartCombat()
    {
        SceneLoader.Instance.ReloadScene();
    }

    internal void EndingCombat()
    {
        //GameManager.Instance.ControlledUnits = AllyUnits;
        SceneLoader.Instance.LoadNewScene(OverWorldScene);
    }
}
