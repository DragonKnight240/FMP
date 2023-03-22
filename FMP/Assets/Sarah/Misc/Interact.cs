using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    public static Interact Instance;
    internal UnitBase SelectedUnit;
    internal UnitBase TempSelectedUnit;
    internal CombatMenu CombatMenu;
    internal GameObject VirtualCam;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        CombatMenu = FindObjectOfType<CombatMenu>();
        VirtualCam = FindObjectOfType<CameraMove>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        bool isOverUI = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();

        if (Options.Instance.OptionsMenuUI.activeInHierarchy || isOverUI)
        {
            return;
        }

        //With Combat Options up checks against selected unit so deselection is possible
        if (CombatMenu.CombatMenuObject.activeInHierarchy || CombatMenu.AttackMenuObject.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                RaycastHit Hit;

                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out Hit))
                {
                    print(Hit.transform.gameObject);
                    print(SelectedUnit.AttackTarget.transform);

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
                                print("Attack");
                                AttackUnit(TileManager.Instance.Grid[Hit.transform.GetComponent<UnitBase>().Position[0], Hit.transform.GetComponent<UnitBase>().Position[0]].GetComponent<Tile>());
                            }
                        }
                    }
                    else if (Hit.transform.GetComponent<Tile>())
                    {
                        print("Tile");
                        if (Hit.transform.GetComponent<Tile>().Unit)
                        {
                            if (Hit.transform.GetComponent<Tile>().Unit == SelectedUnit)
                            {
                                SelectionUnit(Hit.transform.GetComponent<Tile>().Unit);
                            }
                            else if (Hit.transform.GetComponent<Tile>().Unit.CompareTag("Enemy"))
                            {
                                print("Enemy Unit");
                                if (Hit.transform.GetComponent<Tile>().Unit == SelectedUnit.AttackTarget)
                                {
                                    print("Attack");
                                    AttackUnit(Hit.transform.GetComponent<Tile>());
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
                                else if(Hit.transform.GetComponent<Tile>().Unit.CompareTag("Enemy"))
                                {
                                    if(SelectedUnit)
                                    {
                                        AttackUnit(Hit.transform.GetComponent<Tile>());
                                    }
                                }
                            }
                            else if (SelectedUnit)
                            {
                                MoveUnit(Hit.transform.GetComponent<Tile>());
                            }
                        }
                        else if(Hit.transform.GetComponent<UnitBase>() && Hit.transform.CompareTag("Enemy"))
                        {
                            print("Hit Enemy");
                            if (SelectedUnit)
                            {
                                AttackUnit(TileManager.Instance.Grid[Hit.transform.GetComponent<UnitBase>().Position[0], Hit.transform.GetComponent<UnitBase>().Position[1]].GetComponent<Tile>());
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
            UISelectedUnit();

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
                UISelectedUnit();
            }
            
            ChangeMenuButtons();
        }
    }

    internal void MoveUnit(Tile Tile)
    {
        if (SelectedUnit.Move(Tile))
        {
            SelectedUnit = null;
            UISelectedUnit();
        }
    }

    internal void AttackUnit(Tile tile)
    {
        UnitBase Unit = tile.Unit;

        if (SelectedUnit)
        {
            if (CombatMenu.AttackMenuObject.activeInHierarchy)
            {
                print("Attack Unit");
                if (SelectedUnit.AttackTiles.Contains(tile))
                {
                    print("In Attack tiles");
                    SelectedUnit.Attack(Unit);
                    SelectedUnit = null;
                    UISelectedUnit();
                    UnitManager.Instance.UnitUpdate.Invoke();
                    CameraMove.Instance.FollowTarget = null;
                    //CombatMenu.AttackMenuObject.SetActive(false);
                    CombatMenu.AttackMenuObject.GetComponent<UIFade>().ToFadeOut();
                }
            }
            else
            {
                UnitControlled TempSelect = (UnitControlled)SelectedUnit;
                TempSelect.AttackButton(Unit);
            }
        }
    }

    public void ChangeMenuButtons()
    {
        if (!CombatMenu.CombatMenuObject.activeInHierarchy)
        {
            CombatMenu.CheckButtons();

            UnitControlled Unit = (UnitControlled)SelectedUnit;

            CombatMenu.AttackButton.onClick.RemoveAllListeners();
            CombatMenu.AttackButton.onClick.AddListener(() => { Unit.AttackButton(); });

            CombatMenu.MoveButton.onClick.RemoveAllListeners();
            CombatMenu.MoveButton.onClick.AddListener(Unit.MoveButton);

            CombatMenu.ItemButton.onClick.RemoveAllListeners();
            CombatMenu.ItemButton.onClick.AddListener(Unit.ItemButton);

            CombatMenu.WaitButton.onClick.RemoveAllListeners();
            CombatMenu.WaitButton.onClick.AddListener(Unit.WaitUnit);

            CombatMenu.SpecialButton.onClick.RemoveAllListeners();
            CombatMenu.SpecialButton.onClick.AddListener(Unit.SpecialButton);

            CameraMove.Instance.ShouldFollow = false;

            CombatMenu.CombatMenuObject.SetActive(true);
            CombatMenu.CombatMenuObject.GetComponent<UIFade>().ToFadeIn();

            CombatMenu.AttackMenuObject.GetComponent<UIFade>().ToFadeOut();

            CameraMove.Instance.FollowTarget = SelectedUnit.transform;
        }
        else
        {
            CameraMove.Instance.ShouldFollow = true;
            CombatMenu.CombatMenuObject.GetComponent<UIFade>().ToFadeOut();

            CameraMove.Instance.FollowTarget = null;
        }
    }

    internal void ResetTargets()
    {
        SelectedUnit = null;
        UISelectedUnit();
        CameraMove.Instance.FollowTarget = null;

    }

    internal void UISelectedUnit()
    {
        if (SelectedUnit)
        {
            CombatMenu.UnitText.text = SelectedUnit.UnitName;
            UIWeaponImage();
            CombatMenu.SelectedUnitTab.GetComponent<MoveToScreenLocation>().Display = true;
            GameManager.Instance.ToolTipCheck(Tutorial.CUnitSelect);
        }
        else
        {
            CombatMenu.SelectedUnitTab.GetComponent<MoveToScreenLocation>().Display = false;
        }
    }

    internal void UIWeaponImage()
    {
        CombatMenu.WeaponImage.sprite = SelectedUnit.EquipedWeapon.WeaponImage;
    }
}
