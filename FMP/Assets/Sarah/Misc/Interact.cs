using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    public static Interact Instance;
    internal UnitBase SelectedUnit;
    internal UnitBase TempSelectedUnit;
    internal CombatMenu CombatMenu;

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
        //With Combat Options up checks against selected unit so deselection is possible
        if (CombatMenu.CombatMenuObject.activeInHierarchy || CombatMenu.AttackMenuObject.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                RaycastHit Hit;

                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out Hit))
                {
                    if (Hit.transform.GetComponent<UnitBase>())
                    {
                        if (Hit.transform.gameObject == SelectedUnit.gameObject)
                        {
                            SelectionUnit(Hit.transform.GetComponent<UnitBase>());
                        }
                        else if (Hit.transform.CompareTag("Enemy"))
                        {
                            if (Hit.transform == SelectedUnit.AttackTarget.transform)
                            {
                                AttackUnit(Hit.transform.GetComponent<UnitBase>());
                            }
                        }
                    }
                    else if (Hit.transform.GetComponent<Tile>())
                    {
                        if (Hit.transform.GetComponent<Tile>().Unit)
                        {
                            if (Hit.transform.GetComponent<Tile>().Unit == SelectedUnit)
                            {
                                SelectionUnit(Hit.transform.GetComponent<Tile>().Unit);
                            }
                            else if (Hit.transform.GetComponent<Tile>().Unit.CompareTag("Enemy"))
                            {
                                if (Hit.transform == TileManager.Instance.Grid[SelectedUnit.AttackTarget.Position[0], SelectedUnit.AttackTarget.Position[1]])
                                {
                                    AttackUnit(Hit.transform.GetComponent<Tile>().Unit);
                                }
                            }
                        }
                    }
                }
            }
        }
        else
        {
            //Checks every possible way the user could click on the unit/tile
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
                        else if (Hit.transform.GetComponent<Tile>())
                        {
                            if (Hit.transform.GetComponent<Tile>().Unit)
                            {
                                if (Hit.transform.GetComponent<Tile>().Unit.CompareTag("Ally"))
                                {
                                    SelectionUnit(Hit.transform.GetComponent<Tile>().Unit);
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
    }

    internal void SelectionUnit(UnitBase Unit)
    {
        if (SelectedUnit != Unit)
        {
            if (Unit.EndTurn)
            {
                return;
            }

            SelectedUnit = Unit;
            print("Selected Unit");

            if (Unit.MovedForTurn)
            {
                ChangeMenuButtons();
            }
        }
        else
        {
            if (Unit.EndTurn)
            {
                return;
            }

            if (CombatMenu.transform.GetChild(0).gameObject.activeInHierarchy)
            {
                SelectedUnit = null;
                print("Deselect Unit");
            }
            
            ChangeMenuButtons();
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
            if (CombatMenu.AttackMenuObject.activeInHierarchy)
            {
                if (SelectedUnit.AttackTiles.Contains(TileManager.Instance.Grid[Unit.Position[0], Unit.Position[1]].GetComponent<Tile>()))
                {
                    SelectedUnit.Attack(Unit);
                    SelectedUnit = null;
                    print("Attack Enemy");
                    UnitManager.Instance.UnitUpdate.Invoke();
                    CameraMove.Instance.FollowTarget = null;
                    CombatMenu.AttackMenuObject.SetActive(false);
                }
            }
            else
            {
                UnitControlled TempSelect = (UnitControlled)SelectedUnit;
                TempSelect.AttackButton();
            }
        }
    }

    public void ChangeMenuButtons()
    {
        if (!CombatMenu.gameObject.transform.GetChild(0).gameObject.activeInHierarchy)
        {
            CombatMenu.CheckButtons();

            UnitControlled Unit = (UnitControlled)SelectedUnit;

            CombatMenu.AttackButton.onClick.RemoveAllListeners();
            CombatMenu.AttackButton.onClick.AddListener(Unit.AttackButton);

            CombatMenu.MoveButton.onClick.RemoveAllListeners();
            CombatMenu.MoveButton.onClick.AddListener(Unit.MoveButton);

            CombatMenu.ItemButton.onClick.RemoveAllListeners();
            CombatMenu.ItemButton.onClick.AddListener(Unit.ItemButton);

            CombatMenu.WaitButton.onClick.RemoveAllListeners();
            CombatMenu.WaitButton.onClick.AddListener(Unit.WaitUnit);

            CombatMenu.SpecialButton.onClick.RemoveAllListeners();
            CombatMenu.SpecialButton.onClick.AddListener(Unit.SpecialButton);

            CameraMove.Instance.ShouldFollow = false;
            CombatMenu.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            CombatMenu.AttackMenuObject.SetActive(false);

            CameraMove.Instance.FollowTarget = SelectedUnit.transform;
        }
        else
        {
            CameraMove.Instance.ShouldFollow = true;
            CombatMenu.gameObject.transform.GetChild(0).gameObject.SetActive(false);

            CameraMove.Instance.FollowTarget = null;
        }
    }

    internal void ResetTargets()
    {
        SelectedUnit = null;
        CameraMove.Instance.FollowTarget = null;

        print("Targets Reset");
    }
}
