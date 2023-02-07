using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBase : MonoBehaviour
{
    class Node
    {
        public Node PreviousTile;
        public Tile Tile;
        public int FCost;
        public int HCost;
        public int GCost;
    }

    public string UnitName;
    public int HealthMax = 50;
    public int CurrentHealth;
    internal int[] Position;
    public int Movement = 3;
    public float MoveSpeed = 10;

    public List<Tile> MoveableTiles;
    public List<Tile> AttackTiles;

    internal bool isAlive = true;
    internal bool Moving = false;
    List<Tile> Path;

    //Inventory
    public Weapon EquipedWeapon;
    public List<Item> Inventory;
    internal List<Weapon> WeaponsIninventory;
    internal UnitBase AttackTarget;

    //Turn Checks
    internal bool MovedForTurn = false;
    internal bool AttackedForTurn = false;
    internal bool EndTurn = false;

    //Stats
    public int Strength = 2;

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


    public List<UnitBase> InRangeTargets; 

    // Start is called before the first frame update
    void Start()
    {
        MoveableTiles = new List<Tile>();
        AttackTiles = new List<Tile>();
        CurrentHealth = HealthMax;

        MoveableArea(false);

        Inventory = new List<Item>();
        InRangeTargets = new List<UnitBase>();
        WeaponsIninventory = new List<Weapon>();
        Path = new List<Tile>();
    }

    // Update is called once per frame
    virtual public void Update()
    {
        if (isAlive)
        {
            if (CurrentHealth <= 0)
            {
                isAlive = false;
                TileManager.Instance.Grid[Position[0], Position[1]].GetComponent<Tile>().ChangeOccupant(null);
                UnitManager.Instance.DeadEnemyUnits.Add(this);
                //Destroy(this.gameObject);
                gameObject.SetActive(false);
            }
            else
            {
                if (Moving)
                {
                    if (Path.Count <= 0)
                    {
                        return;
                    }

                    if (new Vector3(Path[0].CentrePoint.transform.position.x, transform.position.y, Path[0].CentrePoint.transform.position.z) ==
                        new Vector3(transform.position.x, transform.position.y, transform.position.z))
                    {
                        Path.RemoveAt(0);
                        if (Path.Count <= 0)
                        {
                            Moving = false;
                            return;
                        }
                    }

                    transform.LookAt(new Vector3(Path[0].CentrePoint.transform.position.x, transform.position.y, Path[0].CentrePoint.transform.position.z));
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(Path[0].transform.position.x, transform.position.y, Path[0].transform.position.z), MoveSpeed * Time.deltaTime);
                }
            }
        }
    }

    //Moves the character from the current location to the wanted location
    internal bool Move(Tile NewTile, bool Attacking = false)
    {
        if (MovedForTurn)
        {
            return false;
        }

        if (MoveableTiles.Contains(NewTile) || Attacking)
        {
            MovedForTurn = true;
            TileManager.Instance.Grid[Position[0], Position[1]].GetComponent<Tile>().ChangeOccupant(null);
            Path = new List<Tile>(FindRouteTo(NewTile));
            Moving = true;
            //transform.position = NewTile.CentrePoint.transform.position;
            if (Path.Count > 0)
            {
                Position[0] = Path[Path.Count - 1].GridPosition[0];
                Position[1] = Path[Path.Count - 1].GridPosition[1];
            }

            if (!Attacking)
            {
                NewTile.ChangeOccupant(this);
            }
            else
            {
                if (Path.Count > 0)
                {
                    Path[Path.Count - 1].ChangeOccupant(this);
                }
            }

            ResetMoveableTiles();
            UnitManager.Instance.UnitUpdate.Invoke();
            List<GameObject> Tile = new List<GameObject>();
            Tile.Add(TileManager.Instance.Grid[Position[0], Position[1]].gameObject);

            return true;
        }

        return false;
    }
    public bool AttackableArea(List<GameObject> CheckingTiles, bool ShowTiles = true)
    {
        CheckingTiles.Add(TileManager.Instance.Grid[Position[0], Position[1]]);

        if (EquipedWeapon)
        {
            for (int i = 0; i < EquipedWeapon.Range; i++)
            {
                CheckingTiles = CheckTiles(CheckingTiles, true, ShowTiles);
            }
        }
        else
        {
            CheckingTiles = CheckTiles(CheckingTiles, true, ShowTiles);
        }

        //Checks if an enemy unit is in the attack range
        foreach(Tile tile in AttackTiles)
        {
            if(tile.Unit)
            {
                //Makes sure it's not a unit on the same team
                if (!tile.Unit.CompareTag(tag))
                {
                    return true;
                }
            }
        }

        return false;

    }

    //Finds what tiles can be moved to from the current place
    public void MoveableArea(bool ShowTiles = true)
    {
        HideAllChangedTiles();
        MoveableTiles.Clear();
        AttackTiles.Clear();
        List<GameObject> CheckingTiles = new List<GameObject>();
        CheckingTiles.Add(TileManager.Instance.Grid[Position[0],Position[1]]);

        for(int i = 0; i < Movement; i++)
        {
            CheckingTiles = CheckTiles(CheckingTiles, false, ShowTiles);
        }

        AttackableArea(CheckingTiles, ShowTiles);
    }

    //Adds the adjacent tiles to moveable/attack tiles list and shows the tiles in the correct colour
    internal List<GameObject> CheckTiles(List<GameObject> tiles, bool WeaponRange = false, bool ShowTile = true)
    {
        List<GameObject> NextLayer = new List<GameObject>();

        foreach (GameObject tile in tiles)
        {
            foreach (GameObject AdjacentTile in tile.GetComponent<Tile>().AdjacentTiles)
            {
                if (!MoveableTiles.Contains(AdjacentTile.GetComponent<Tile>()))
                {
                    if (AdjacentTile.GetComponent<Tile>().CanMoveOn || AdjacentTile.GetComponent<Tile>().Unit)
                    {
                        if (!WeaponRange)
                        {
                            MoveableTiles.Add(AdjacentTile.GetComponent<Tile>());
                        }

                        NextLayer.Add(AdjacentTile);

                    }

                    if (!AttackTiles.Contains(AdjacentTile.GetComponent<Tile>()))
                    {
                        AttackTiles.Add(AdjacentTile.GetComponent<Tile>());
                    }

                    if (ShowTile)
                    {
                        AdjacentTile.GetComponent<Tile>().Show(WeaponRange);
                    }
                }

            }
        }

        return NextLayer;
    }

    //Hides all changed tiles
    internal void HideAllChangedTiles()
    {
        foreach (Tile tile in AttackTiles)
        {
            if (Interact.Instance.SelectedUnit)
            {
                //if (!Interact.Instance.SelectedUnit.AttackTiles.Contains(tile))
                //{
                    tile.Hide();
                //}
            }
            else
            {
                tile.Hide();
            }
        }
    }

    internal void ShowAllInRangeTiles()
    {
        foreach (Tile tile in AttackTiles)
        {
            tile.WhichColour(this);
        }
    }

    //Hides all changed tiles and Clears both lists
    internal void ResetMoveableTiles()
    {
        HideAllChangedTiles();

        MoveableTiles.Clear();
        AttackTiles.Clear();
    }

    internal void Attack(UnitBase Enemy)
    {
        List<GameObject> tiles = new List<GameObject>();
        tiles.Add(TileManager.Instance.Grid[Position[0], Position[1]]);

        //Change later to proper logic
        Move(TileManager.Instance.Grid[Enemy.Position[0], Enemy.Position[1]].GetComponent<Tile>(), true);
        //Damage and hit rate to be calculated and implemented later

        if (EquipedWeapon)
        {
            Enemy.CurrentHealth -= EquipedWeapon.Damage * 2;
        }
        else
        {
            Enemy.CurrentHealth -= 2;
        }

        ResetMoveableTiles();
        MovedForTurn = true;
        AttackedForTurn = true;
        EndTurn = true;
    }

    internal int CalculateDamage()
    {
        int Damage;
        if (EquipedWeapon)
        {
            Damage = EquipedWeapon.Damage;
        }
        else
        {
            Damage = Strength;
        }
        
        return Damage;
    }

    internal int CalcuateHitChance()
    {
        int HitChance = 100;
        return HitChance;
    }

    internal int CalculateCritChance()
    {
        int CritChance = 0;
        return CritChance;
    }

    internal int CalculateReturnDamage()
    {
        int ReturnDamage = 0;
        return ReturnDamage;
    }

    internal int CalculateReturnHitChance()
    {
        int ReturnHitChance = 0;
        return ReturnHitChance;
    }

    internal int CalculateReturnCritChance()
    {
        int ReturnCritChance = 0;
        return ReturnCritChance;
    }

    internal void DecreaseHealth(int Attack)
    {
        CurrentHealth -= Attack;
    }

    internal void IncreaseHealth(int Health)
    {
        CurrentHealth += Health;
    }

    private void OnMouseEnter()
    {
        if(EndTurn)
        {
            return;
        }

        if (!Interact.Instance.CombatMenu.transform.GetChild(0).gameObject.activeInHierarchy && !Interact.Instance.CombatMenu.AttackMenuObject.gameObject.activeInHierarchy)
        {
            if (!CompareTag("Enemy"))
            {
                if (!MovedForTurn)
                {
                    ShowAllInRangeTiles();
                }
                else if (!AttackedForTurn)
                {
                    List<GameObject> Tiles = new List<GameObject>();
                    Tiles.Add(TileManager.Instance.Grid[Position[0], Position[1]]);
                    AttackableArea(Tiles);
                }
            }
            else
            {
                ShowAllInRangeTiles();
            }

            TileManager.Instance.Grid[Position[0], Position[1]].GetComponent<Tile>().Show(false, true);
        }
    }

    private void OnMouseExit()
    {
        if (!Interact.Instance.CombatMenu.transform.GetChild(0).gameObject.activeInHierarchy && !Interact.Instance.CombatMenu.AttackMenuObject.gameObject.activeInHierarchy)
        {
            if (!Interact.Instance.SelectedUnit)
            {
                HideAllChangedTiles();
            }
            else
            {
                TileManager.Instance.Grid[Position[0], Position[1]].GetComponent<Tile>().WhichColour();
            }
        }
    }

    public void TurnChange()
    {
        MovedForTurn = false;
        AttackedForTurn = false;
        EndTurn = false;

        MoveableArea(false);
    }

    internal void WaitUnit()
    {
        foreach(Tile tile in AttackTiles)
        {
            tile.Hide();
        }

        MovedForTurn = true;
        AttackedForTurn = true;
        EndTurn = true;

        Interact.Instance.SelectedUnit = null;
        HideAllChangedTiles();
        Interact.Instance.CombatMenu.CombatMenuObject.SetActive(false);
        CameraMove.Instance.FollowTarget = null;
    }

    //A* Pathfinding
    List<Tile> FindRouteTo(Tile TargetTile)
    {
        List<Node> ToCheckNodes = new List<Node>();
        Dictionary<Tile, Node> CheckedNodes = new Dictionary<Tile, Node>();

        Node Start = new Node { Tile = TileManager.Instance.Grid[Position[0], Position[1]].GetComponent<Tile>(), FCost = 0, GCost = 0, HCost = 0 };
        Node End = new Node { Tile = TargetTile, FCost = 0, GCost = 0, HCost = 0 };

        ToCheckNodes.Add(Start);
        Node CurrentNode;

        int G;
        List<Tile> Path = new List<Tile>();

        while (ToCheckNodes.Count > 0)
        {
            CurrentNode = ToCheckNodes[0];

            foreach(Node Node in ToCheckNodes)
            {
                if(Node.FCost < CurrentNode.FCost)
                {
                    CurrentNode = Node;
                }
            }

            CheckedNodes.Add(CurrentNode.Tile, CurrentNode);
            ToCheckNodes.Remove(CurrentNode);

            if(CurrentNode.Tile == End.Tile)
            {
                Path = FindPath(CurrentNode);
                //print("Success");
                return Path;
            }

            foreach(GameObject AdjacentTile in CurrentNode.Tile.AdjacentTiles)
            {
                if(CheckedNodes.ContainsKey(AdjacentTile.GetComponent<Tile>()) || 
                    (AdjacentTile.GetComponent<Tile>().Unit && AdjacentTile.GetComponent<Tile>() != End.Tile))
                {
                    continue;
                }

                G = CurrentNode.GCost + 1;

                if(G<CurrentNode.GCost || !HasTile(ToCheckNodes, AdjacentTile.GetComponent<Tile>()))
                {
                    Node AdjacentNode = new Node { PreviousTile = CurrentNode, Tile = AdjacentTile.GetComponent<Tile>(), GCost = G, HCost = DistanceToTile(AdjacentTile.GetComponent<Tile>(), End.Tile) };
                    AdjacentNode.FCost = AdjacentNode.GCost + AdjacentNode.HCost;

                    if(!ToCheckNodes.Contains(AdjacentNode))
                    {
                        ToCheckNodes.Add(AdjacentNode);
                    }
                }
            }
        }

        print("Failed");
        return Path;
    }

    List<Tile> FindPath(Node EndNode)
    {
        List<Tile> Path = new List<Tile>();
        Node Node = EndNode;

        if (Node.Tile.Unit == null && MoveableTiles.Contains(Node.Tile))
        {
            Path.Add(EndNode.Tile);
        }

        while (Node.Tile != TileManager.Instance.Grid[Position[0], Position[1]].GetComponent<Tile>())
        {
            if (Node.Tile.Unit == null && MoveableTiles.Contains(Node.PreviousTile.Tile))
            {
                Path.Add(Node.PreviousTile.Tile);
            }
            
            Node = Node.PreviousTile;
        }
        Path.Reverse();

        return Path;
    }

    int DistanceToTile(Tile StartTile, Tile EndTile)
    {
        return Mathf.Abs(StartTile.GridPosition[0] - EndTile.GridPosition[0]) + Mathf.Abs(StartTile.GridPosition[1] - EndTile.GridPosition[1]);
    }

    bool HasTile(List<Node> Nodes, Tile EndTile)
    {
        foreach(Node Node in Nodes)
        {
            if(EndTile == Node.Tile)
            {
                return true;
            }
        }

        return false;
    }

}
