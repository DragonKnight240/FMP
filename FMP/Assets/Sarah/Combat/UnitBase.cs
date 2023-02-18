using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public Slider UIHealth;
    public float HealthLerpSpeed = 10;

    [Header("General")]
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

    CombatAnimControl AnimControl;

    [Header("Inventory")]
    public Weapon EquipedWeapon;
    public List<Item> Inventory;
    public List<Weapon> WeaponsIninventory;
    internal UnitBase AttackTarget;
    public Weapon BareHands;

    //Turn Checks
    internal bool MovedForTurn = false;
    internal bool AttackedForTurn = false;
    internal bool EndTurn = false;

    [Header("Stats")]
    public int Strength = 2;
    public int Dexterity;
    public int Magic;
    public int Defence;
    public int Resistance;
    public int Speed;
    public int Luck;

    [Header("Weapon Proficiencies")]
    public float BowProficiency;
    public int BowLevel;

    public float SwordProficiency;
    public int SwordLevel;

    public float MagicProficiency;
    public int MagicLevel;

    public float FistProficiency;
    public int FistLevel;

    [Header("Class")]
    public Class Class;

    [Header("Attack")]
    public SpecialAttacks CurrentAttack;
    public List<SpecialAttacks> UnlockedAttacks;
    internal List<GameObject> OuterMostMove;
    public GameObject AttackCamera;
    bool ToAttack = false;

    public List<UnitBase> InRangeTargets; 

    // Start is called before the first frame update
    void Start()
    {
        MoveableTiles = new List<Tile>();
        AttackTiles = new List<Tile>();
        InRangeTargets = new List<UnitBase>();
        OuterMostMove = new List<GameObject>();
        //WeaponsIninventory = new List<Weapon>();
        Path = new List<Tile>();

        MoveableArea(false);

        WeaponsIninventory.Add(BareHands);

        AnimControl = GetComponent<CombatAnimControl>();
    }

    // Update is called once per frame
    virtual public void Update()
    {
        if (isAlive)
        {
            if (CurrentHealth <= 0)
            {
                //AnimControl.ChangeAnim("Death", CombatAnimControl.AnimParameters.Death);
                //TileManager.Instance.Grid[Position[0], Position[1]].GetComponent<Tile>().ChangeOccupant(null);
                //UnitManager.Instance.DeadEnemyUnits.Add(this);
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

                            if(ToAttack)
                            {
                                ToAttack = false;
                                AttackingZoom();
                            }
                            return;
                        }
                    }

                    transform.LookAt(new Vector3(Path[0].CentrePoint.transform.position.x, transform.position.y, Path[0].CentrePoint.transform.position.z));
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(Path[0].transform.position.x, transform.position.y, Path[0].transform.position.z), MoveSpeed * Time.deltaTime);
                }
                else
                {
                    if (ToAttack)
                    {
                        ToAttack = false;
                        AttackingZoom();
                    }
                }
            }
        }

        if (UIHealth)
        {
            if (UIHealth.value != CurrentHealth)
            {
                UIHealth.value = Mathf.Lerp(UIHealth.value, CurrentHealth, Time.deltaTime * HealthLerpSpeed);
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

        if ((MoveableTiles.Contains(NewTile) && NewTile.Unit == null) || Attacking)
        {
            MovedForTurn = true;
            TileManager.Instance.Grid[Position[0], Position[1]].GetComponent<Tile>().ChangeOccupant(null);
            Path = new List<Tile>(FindRouteTo(NewTile));
            Moving = true;

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
                CheckingTiles = WeaponRangeAttack(CheckingTiles, ShowTiles);
            }
        }
        else
        {
            CheckingTiles = WeaponRangeAttack(CheckingTiles, ShowTiles);
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
        OuterMostMove.Clear();
        List<GameObject> CheckingTiles = new List<GameObject>();
        CheckingTiles.Add(TileManager.Instance.Grid[Position[0],Position[1]]);

        for(int i = 0; i < Movement; i++)
        {
            CheckingTiles = CheckTiles(CheckingTiles, false, ShowTiles);
        }

        foreach(GameObject tile in CheckingTiles)
        {
            if (!OuterMostMove.Contains(tile))
            {
                OuterMostMove.Add(tile);
            }
        }

        if (OuterMostMove.Count > 0)
        {
            AttackableArea(OuterMostMove, ShowTiles);
        }
        else
        {
            AttackableArea(CheckingTiles, ShowTiles);
        }
    }

    //Adds the adjacent tiles to moveable/attack tiles list and shows the tiles in the correct colour
    internal List<GameObject> CheckTiles(List<GameObject> tiles, bool WeaponRange = false, bool ShowTile = true)
    {
        List<GameObject> NextLayer = new List<GameObject>();
        bool MoveEnd = false;

        foreach (GameObject tile in tiles)
        {
            MoveEnd = false;
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

                        if(AdjacentTile.GetComponent<Tile>().Unit)
                        {
                            if (AdjacentTile.GetComponent<Tile>().Unit != this)
                            {
                                MoveEnd = true;
                            }
                        }
                        else
                        {
                            NextLayer.Add(AdjacentTile);
                        }

                        
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

            if(MoveEnd)
            {
                OuterMostMove.Add(tile);
            }
        }

        return NextLayer;
    }

    internal List<GameObject> WeaponRangeAttack(List<GameObject> tiles, bool ShowTiles)
    {
        List<GameObject> NextLayer = new List<GameObject>();

        foreach (GameObject tile in tiles)
        {
            foreach (GameObject AdjacentTile in tile.GetComponent<Tile>().AdjacentTiles)
            {
                if (AdjacentTile.GetComponent<Tile>().CanMoveOn || AdjacentTile.GetComponent<Tile>().Unit)
                {
                    if (!NextLayer.Contains(AdjacentTile))
                    {
                        NextLayer.Add(AdjacentTile);
                    }

                    if (!AttackTiles.Contains(AdjacentTile.GetComponent<Tile>()))
                    {
                        AttackTiles.Add(AdjacentTile.GetComponent<Tile>());
                    }

                    if (ShowTiles)
                    {
                        AdjacentTile.GetComponent<Tile>().Show(true);
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

        ToAttack = true;

        //Change later to proper logic
        Move(TileManager.Instance.Grid[Enemy.Position[0], Enemy.Position[1]].GetComponent<Tile>(), true);
    }

    public void AttackingZoom()
    {
        transform.LookAt(AttackTarget.transform);
        AttackCamera.SetActive(true);
        Interact.Instance.GetComponent<Camera>().enabled = false;

        AnimControl.ChangeAnim("Attack", CombatAnimControl.AnimParameters.Attack);
    }

    public void HitZoom()
    {
        AttackTarget.AttackCamera.SetActive(true);
        AttackCamera.SetActive(false);

        //Will calculate crit change later
        AttackTarget.CurrentHealth -= CalculateDamage();

        if (AttackTarget.CurrentHealth > 0)
        {
            AttackTarget.AnimControl.ChangeAnim("Hit", CombatAnimControl.AnimParameters.Hit);
        }
        else
        {
            AttackTarget.AnimControl.ChangeAnim("Death", CombatAnimControl.AnimParameters.Death);
            AttackTarget.GetComponent<Fading>().ChangeMaterial();
            AttackTarget.GetComponent<Fading>().FadeOut = true;
            AttackTarget.isAlive = false;
        }

        ResetMoveableTiles();
        MovedForTurn = true;
        AttackedForTurn = true;
        EndTurn = true;
    }

    public void ReturnMainCamera()
    {
        Interact.Instance.GetComponent<Camera>().enabled = true;
        AttackCamera.SetActive(false);
        //AttackTarget.AttackCamera.SetActive(false);
    }

    public void OnDeath()
    {
        print("End");
        TileManager.Instance.Grid[Position[0], Position[1]].GetComponent<Tile>().ChangeOccupant(null);
        UnitManager.Instance.DeadEnemyUnits.Add(this);
        AttackCamera.SetActive(false);

        Interact.Instance.GetComponent<Camera>().enabled = true;
        gameObject.SetActive(false);
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
        if (!UnitManager.Instance.SetupFinished)
        {
            return;
        }

        if (EndTurn)
        {
            return;
        }

        if (!Interact.Instance.CombatMenu.transform.GetChild(0).gameObject.activeInHierarchy 
            && !Interact.Instance.CombatMenu.AttackMenuObject.gameObject.activeInHierarchy
            && Interact.Instance.SelectedUnit == null)
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
        }

        if (!Interact.Instance.CombatMenu.AttackMenuObject.gameObject.activeInHierarchy
             && !Interact.Instance.CombatMenu.CombatMenuObject.gameObject.activeInHierarchy)
        {
            TileManager.Instance.Grid[Position[0], Position[1]].GetComponent<Tile>().Show(false, true);
        }
    }

    private void OnMouseExit()
    {
        if (!UnitManager.Instance.SetupFinished)
        {
            return;
        }

        if (!Interact.Instance.CombatMenu.transform.GetChild(0).gameObject.activeInHierarchy
            && !Interact.Instance.CombatMenu.AttackMenuObject.gameObject.activeInHierarchy
            && Interact.Instance.SelectedUnit == null)
        {
            HideAllChangedTiles();
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
        Interact.Instance.UISelectedUnit();
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
            if (Node.PreviousTile.Tile.Unit == null && MoveableTiles.Contains(Node.PreviousTile.Tile))
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
