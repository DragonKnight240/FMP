using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    public static Interact Instance;
    internal UnitBase SelectedUnit;
    internal UnitBase TempSelectedUnit;
    CombatMenu CombatMenu;

    // Start is called before the first frame update
    void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        CombatMenu = FindObjectOfType<CombatMenu>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            RaycastHit Hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out Hit))
            {
                if (TurnManager.Instance.isPlayerTurn)
                {
                    if (Hit.transform.GetComponent<UnitBase>() && Hit.transform.CompareTag("Ally"))
                    {
                        SelectionUnit(Hit.transform.GetComponent<UnitBase>());
                    }
                    else if (Hit.transform.GetComponent<UnitBase>() && Hit.transform.CompareTag("Enemy"))
                    {
                        AttackUnit(Hit.transform.GetComponent<UnitBase>());
                    }
                    else if(Hit.transform.GetComponent<Tile>())
                    {
                        if(Hit.transform.GetComponent<Tile>().Unit)
                        {
                            if(Hit.transform.GetComponent<Tile>().Unit.CompareTag("Ally"))
                            {
                                SelectionUnit(Hit.transform.GetComponent<Tile>().Unit);
                            }
                            else if(Hit.transform.GetComponent<Tile>().Unit.CompareTag("Enemy"))
                            {
                                AttackUnit(Hit.transform.GetComponent<Tile>().Unit);
                            }
                        }
                        else if (SelectedUnit)
                        {
                            MoveUnit(Hit.transform.GetComponent<Tile>());
                        }
                    }
                }
                else
                {
                    //Enemy Turn
                }
            }
        }
    }

    internal void SelectionUnit(UnitBase Unit)
    {
        if (SelectedUnit != Unit)
        {
            SelectedUnit = Unit;
            print("Selected Unit");
        }
        else
        {
            SelectedUnit = null;
            print("Deselect Unit");
        }
    }

    internal void MoveUnit(Tile Tile)
    {
        if (SelectedUnit.Move(Tile))
        {
            SelectedUnit = null;
            print("Move Unit");
        }
    }

    internal void AttackUnit(UnitBase Unit)
    {
        if (SelectedUnit)
        {
            if (SelectedUnit.AttackTiles.Contains(TileManager.Instance.Grid[Unit.Position[0], Unit.Position[1]].GetComponent<Tile>()))
            {
                SelectedUnit.Attack(Unit);
                SelectedUnit = null;
                print("Attack Enemy");
            }
        }
    }

    public void ChangeMenuButtons()
    {
        CombatMenu.AttackButton.onClick.RemoveAllListeners();
        //CombatMenu.AttackButton.onClick.AddListener(SelectedUnit.);

        CombatMenu.MoveButton.onClick.RemoveAllListeners();
        //CombatMenu.MoveButton.onClick.AddListener(SelectedUnit.);

        CombatMenu.ItemButton.onClick.RemoveAllListeners();
        //CombatMenu.ItemButton.onClick.AddListener(SelectedUnit);

        CombatMenu.WaitButton.onClick.RemoveAllListeners();
        CombatMenu.WaitButton.onClick.AddListener(SelectedUnit.WaitUnit);
    }
}
