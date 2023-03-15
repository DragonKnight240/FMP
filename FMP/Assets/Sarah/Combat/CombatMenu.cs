using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum Stats
{
    HP,
    Dex,
    Str,
    Mag,
    Def,
    Res,
    Speed,
    Luck
}
public class CombatMenu : MonoBehaviour
{
    public GameObject CombatMenuObject;
    public GameObject AttackMenuObject;

    //Combat Menu Buttons
    public Button SpecialButton;
    public Button MoveButton;
    public Button AttackButton;
    public Button ItemButton;
    public Button WaitButton;

    //Attack Menu Buttons
    public TMP_Text Weapon;
    public TMP_Text Attack;

    public Slider HealthAlly;
    public TMP_Text DamageAlly;
    public TMP_Text HitAlly;
    public TMP_Text CritAlly;

    public Slider HealthEnemy;
    public TMP_Text DamageEnemy;
    public TMP_Text HitEnemy;
    public TMP_Text CritEnemy;

    public GameObject NextTarget;
    public GameObject PreviousTarget;

    public GameObject NextAttack;
    public GameObject PreviousAttack;

    public GameObject NextWeapon;
    public GameObject PreviousWeapon;

    public GameObject SupportMainObj;
    public List<Image> SupportImages;

    //Inventory
    public GameObject InventoryObject;

    //Item Notification
    public TMP_Text ItemText;
    public GameObject ItemNotification;
    public float ItemStayTime = 5.0f;
    float ItemStayTimer = 0;

    //Selected
    public GameObject SelectedUnitTab;
    public TMP_Text UnitText;
    public Image WeaponImage;

    //End Turn
    public GameObject EndTurnButton;
    internal MoveToScreenLocation EndButtonMover;

    //Top Bar Text
    public TMP_Text LivingEnemyText;
    public TMP_Text LivingAlliesText;
    public TMP_Text UnitsToActText;


    //Attack Name
    public TMP_Text AttackText; 

    //LevelUP
    public GameObject EXP;
    public int TargetEXP;
    public Slider EXPBar;
    internal bool ShowEXP;
    internal UnitBase ToLevel;

    public GameObject LevelScreen;

    public GameObject HPIncrease;
    public TMP_Text HPText;
    public GameObject StrIncrease;
    public TMP_Text StrText;
    public GameObject DexIncrease;
    public TMP_Text DexText;
    public GameObject MagicIncrease;
    public TMP_Text MagicText;
    public GameObject DefIncrease;
    public TMP_Text DefText;
    public GameObject ResIncrease;
    public TMP_Text ResText;
    public GameObject SpeedIncrease;
    public TMP_Text SpeedText;
    public GameObject LuckIncrease;
    public TMP_Text LuckText;

    public TMP_Text OldLevel;
    public TMP_Text NewLevel;

    //Ending Scences
    public GameObject VictoryScreen;
    public GameObject DefeatScreen;


    private void Start()
    {
        CombatMenuObject.GetComponent<CanvasGroup>().alpha = 0;
        CombatMenuObject.SetActive(false);

        InventoryObject.GetComponent<CanvasGroup>().alpha = 0;
        InventoryObject.SetActive(false);

        AttackMenuObject.GetComponent<CanvasGroup>().alpha = 0;
        AttackMenuObject.SetActive(false);

        ItemNotification.SetActive(false);

        VictoryScreen.GetComponent<CanvasGroup>().alpha = 0;
        VictoryScreen.SetActive(false);

        DefeatScreen.GetComponent<CanvasGroup>().alpha = 0;
        DefeatScreen.SetActive(false);

        EndButtonMover = EndTurnButton.GetComponent<MoveToScreenLocation>();

        LevelScreen.GetComponent<CanvasGroup>().alpha = 0;
        LevelScreen.SetActive(false);

        EXP.GetComponent<CanvasGroup>().alpha = 0;
        EXP.SetActive(false);

        AttackText.GetComponent<CanvasGroup>().alpha = 0;
        AttackText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(ItemNotification.activeInHierarchy)
        {
            ItemStayTimer += Time.deltaTime;

            if(ItemStayTime <= ItemStayTimer)
            {
                ItemNotification.SetActive(false);
                ItemStayTimer = 0;
            }
        }

        if(TurnManager.Instance.isPlayerTurn)
        {
            EndButtonMover.Display = true;
        }
        else
        {
            EndButtonMover.Display = false;
        }

        if(LivingAlliesText.text != (UnitManager.Instance.AllyUnits.Count - UnitManager.Instance.DeadAllyUnits.Count).ToString())
        {
            LivingAlliesText.text = (UnitManager.Instance.AllyUnits.Count - UnitManager.Instance.DeadAllyUnits.Count).ToString();
        }

        if(LivingEnemyText.text != (UnitManager.Instance.EnemyUnits.Count - UnitManager.Instance.DeadEnemyUnits.Count).ToString())
        {
            LivingEnemyText.text = (UnitManager.Instance.EnemyUnits.Count - UnitManager.Instance.DeadEnemyUnits.Count).ToString();
        }

        if (UnitsToActText.text != TurnManager.Instance.UnitsToMove.ToString() && TurnManager.Instance.UnitsToMove >= 0)
        {
            UnitsToActText.text = TurnManager.Instance.UnitsToMove.ToString();
        }

        if(ShowEXP && EXP.GetComponent<CanvasGroup>().alpha == 1)
        {
            EXPBar.value = Mathf.Lerp(EXPBar.value, TargetEXP, Time.deltaTime);

            if(EXPBar.value >= TargetEXP - 1 || EXPBar.value == EXPBar.maxValue)
            {
                ShowEXP = false;
                EXP.GetComponent<UIFade>().ToFadeOut();

                if(ToLevel != null)
                {
                    LevelShow(ToLevel);
                    ToLevel = null;
                }
            }
        }

        if(LevelScreen.activeInHierarchy)
        {
            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                LevelScreen.GetComponent<UIFade>().ToFadeOut();
            }
        }
    }

    public void EXPSliderShow(UnitBase Unit, int Damage)
    {
        EXPBar.maxValue = Unit.EXPNeeded[Unit.Level - 1];
        EXPBar.value = Unit.EXP;
        Unit.GainCharacterEXP(Damage);
        Unit.GainClassEXP(Damage);
        Unit.GainWeaponEXP();

        if(Unit.isSupported())
        {
            Unit.GainSupportEXP(Damage);
        }

        TargetEXP = Unit.EXP;

        ShowEXP = true;
        EXP.SetActive(true);
        EXP.GetComponent<UIFade>().ToFadeIn();
    }

    public void LevelShow(UnitBase Unit)
    {
        OldLevel.text = (Unit.Level - 1).ToString();
        NewLevel.text = (Unit.Level).ToString();

        LevelScreen.SetActive(true);
        LevelScreen.GetComponent<UIFade>().ToFadeIn();
    }

    public void StatIncrease(Stats Stat, int Amount)
    {
        switch(Stat)
        {
            case Stats.HP:
                {
                    if(Amount == 0)
                    {
                        HPIncrease.SetActive(false);
                        break;
                    }

                    HPText.text = Amount.ToString();
                    HPIncrease.SetActive(true);
                    break;
                }
            case Stats.Def:
                {
                    if (Amount == 0)
                    {
                        DefIncrease.SetActive(false);
                        break;
                    }

                    DefText.text = Amount.ToString();
                    DefIncrease.SetActive(true);
                    break;
                }
            case Stats.Dex:
                {
                    if (Amount == 0)
                    {
                        DexIncrease.SetActive(false);
                        break;
                    }

                    DexText.text = Amount.ToString();
                    DexIncrease.SetActive(true);
                    break;
                }
            case Stats.Speed:
                {
                    if (Amount == 0)
                    {
                        SpeedIncrease.SetActive(false);
                        break;
                    }

                    SpeedText.text = Amount.ToString();
                    SpeedIncrease.SetActive(true);
                    break;
                }
            case Stats.Str:
                {
                    if (Amount == 0)
                    {
                        StrIncrease.SetActive(false);
                        break;
                    }

                    StrText.text = Amount.ToString();
                    StrIncrease.SetActive(true);
                    break;
                }
            case Stats.Mag:
                {
                    if (Amount == 0)
                    {
                        MagicIncrease.SetActive(false);
                        break;
                    }

                    MagicText.text = Amount.ToString();
                    MagicIncrease.SetActive(true);
                    break;
                }
            case Stats.Luck:
                {
                    if (Amount == 0)
                    {
                        LuckIncrease.SetActive(false);
                        break;
                    }

                    LuckText.text = Amount.ToString();
                    LuckIncrease.SetActive(true);
                    break;
                }
            case Stats.Res:
                {
                    if (Amount == 0)
                    {
                        ResIncrease.SetActive(false);
                        break;
                    }

                    ResText.text = Amount.ToString();
                    ResIncrease.SetActive(true);
                    break;
                }
        }
    }

    public void CloseInventory()
    {
        //InventoryObject.SetActive(false);
        InventoryObject.GetComponent<UIFade>().ToFadeOut();
    }

    public void ChangeAttackTarget(bool Next)
    {
        UnitBase Unit = Interact.Instance.SelectedUnit;
        int NewIndex;
        if (Next)
        {
            NewIndex = Unit.InRangeTargets.IndexOf(Unit.AttackTarget) + 1;
            if (NewIndex >= Unit.InRangeTargets.Count)
            {
                NewIndex = 0;
            }
        }
        else
        {
            NewIndex = Unit.InRangeTargets.IndexOf(Unit.AttackTarget) - 1;
            if (NewIndex < 0)
            {
                NewIndex = Unit.InRangeTargets.Count - 1;
            }
        }

        Unit.AttackTarget = Unit.InRangeTargets[NewIndex];
        Unit.GetComponent<UnitControlled>().AttackDisplay();
        CameraMove.Instance.FollowTarget = Unit.AttackTarget.transform;
    }

    public void ChangeWeapon(bool Next)
    {
        UnitBase Unit = Interact.Instance.SelectedUnit;
        List<Weapon> Weapons = Unit.WeaponsIninventory;

        int NewIndex;
        if(Next)
        {
            NewIndex = Weapons.IndexOf(Unit.EquipedWeapon) + 1;
            if(NewIndex >= Weapons.Count)
            {
                NewIndex = 0;
            }
        }
        else
        {
            NewIndex = Weapons.IndexOf(Unit.EquipedWeapon) - 1;
            if (NewIndex < 0)
            {
                NewIndex = Unit.WeaponsIninventory.Count -1;
            }
        }

        GameManager.Instance.ToolTipCheck(Tutorial.CChangeWeapon);

        Unit.EquipedWeapon = Weapons[NewIndex];
        Interact.Instance.UIWeaponImage();
        Unit.ChangeWeaponImage();
        ChangeAvailableAttacks();
        CheckTargetStatus();
    }

    public void ChangeAttack(bool Next)
    {
        UnitBase Unit = Interact.Instance.SelectedUnit;
        List<SpecialAttacks> AttackList = Unit.AvailableAttacks;

        int NewIndex;
        if (Next)
        {
            NewIndex = AttackList.IndexOf(Unit.CurrentAttack) + 1;
            if (NewIndex >= AttackList.Count)
            {
                NewIndex = 0;
            }
        }
        else
        {
            NewIndex = AttackList.IndexOf(Unit.CurrentAttack) - 1;
            if (NewIndex < 0)
            {
                NewIndex = AttackList.Count - 1;
            }
        }

        Unit.CurrentAttack = AttackList[NewIndex];

        CheckTargetStatus();
    }

    internal void ChangeAvailableAttacks()
    {
        UnitBase Unit = Interact.Instance.SelectedUnit;

        Unit.AvailableAttacks.Clear();
        Unit.AvailableAttacks = new List<SpecialAttacks>();

        foreach (SpecialAttacks Attack in Unit.UnlockedAttacks)
        {
            if (Attack.WeaponType == Unit.EquipedWeapon.WeaponType)
            {
                if (!Unit.AvailableAttacks.Contains(Attack))
                {
                    Unit.AvailableAttacks.Add(Attack);
                }
            }
        }

        if (!Unit.AvailableAttacks.Contains(Unit.EquipedWeapon.Special))
        {
            Unit.AvailableAttacks.Add(Unit.EquipedWeapon.Special);
        }

        Unit.CurrentAttack = Unit.AvailableAttacks[0];
    }

    public void CancelAttack()
    {
        AttackMenuObject.GetComponent<UIFade>().ToFadeOut();
        CameraMove.Instance.FollowTarget = null;
        Interact.Instance.SelectedUnit.HideAllChangedTiles();
        Interact.Instance.SelectedUnit = null;
        Interact.Instance.UISelectedUnit();
    }

    internal void CheckTargetStatus()
    {
        UnitBase Unit = Interact.Instance.SelectedUnit;

        Unit.HideAllChangedTiles();
        Unit.MoveableArea(false);
        Unit.GetComponent<UnitControlled>().FindInRangeTargets();
        Unit.ShowAllInRangeTiles();

        if (Unit.InRangeTargets.Count > 0)
        {
            if (!Unit.InRangeTargets.Contains(Unit.AttackTarget))
            {
                Unit.AttackTarget = Unit.InRangeTargets[0];
                CameraMove.Instance.FollowTarget = Unit.InRangeTargets[0].transform;
            }

            Unit.GetComponent<UnitControlled>().AttackDisplay();
        }
        else
        {
            CameraMove.Instance.FollowTarget = null;
            Interact.Instance.CombatMenu.AttackMenuObject.SetActive(false);
        }
    }

    internal void CheckButtons()
    {
        UnitBase Unit = Interact.Instance.SelectedUnit;

        foreach (GameObject tile in TileManager.Instance.Grid[Unit.Position[0], Unit.Position[1]].GetComponent<Tile>().AdjacentTiles)
        {
            if (tile.GetComponent<Tile>().Special)
            {
                if(tile.GetComponent<Tile>().Special.ClassNeeded != null)
                {
                    if(tile.GetComponent<Tile>().Special.ClassNeeded == Unit.Class)
                    {
                        continue;
                    }
                }
                SpecialButton.gameObject.SetActive(true);
                if (ToolTipManager.Instance)
                {
                    if (ToolTipManager.Instance.CompletedTurn1)
                    {
                        ToolTip Tip = ToolTipManager.Instance.FindToolTip(Tutorial.CWeaponAbility);
                        if (!ToolTipManager.Instance.Seen[Tip])
                        {
                            GameManager.Instance.NextToolTip(Tip);
                        }
                    }
                }
                break;
            }
            else
            {
                if(Unit.GetComponent<UnitControlled>().CanRecruit())
                {
                    SpecialButton.gameObject.SetActive(true);
                    break;
                }

                SpecialButton.gameObject.SetActive(false);
            }
        }

        Unit.GetComponent<UnitControlled>().FindInRangeTargets();

        if (Unit.GetComponent<UnitControlled>().InRangeTargets.Count > 0)
        {
            AttackButton.gameObject.SetActive(true);
        }
        else
        {
            AttackButton.gameObject.SetActive(false);
        }

        if(Unit.MovedForTurn)
        {
            MoveButton.gameObject.SetActive(false);
        }
        else
        {
            MoveButton.gameObject.SetActive(true);
        }
    }

    public void DisplaySupport()
    {
        if(Interact.Instance.SelectedUnit)
        {
            UnitBase Unit = Interact.Instance.SelectedUnit;

            if (Unit.SupportedUnits.Count > 0)
            {
                for (int i = 0; i < SupportImages.Count; i++)
                {
                    if (Unit.SupportedUnits.Count < i)
                    {
                        SupportImages[i].sprite = Unit.SupportedUnits[i].UnitImage;
                        SupportImages[i].gameObject.SetActive(true);
                        continue;
                    }

                    SupportImages[i].gameObject.SetActive(false);
                }

                SupportMainObj.SetActive(true);
            }
            else
            {
                SupportMainObj.SetActive(false);
            }
        }
        else
        {
            SupportMainObj.SetActive(false);
        }
    }

    public void EndPlayerTurn()
    {
        foreach(GameObject Unit in UnitManager.Instance.AllyUnits)
        {
            Unit.GetComponent<UnitBase>().WaitUnit();
        }

        EndButtonMover.Display = false;
    }

    public void DisplayVictoryScreen()
    {
        Destroy(CameraMove.Instance);
        VictoryScreen.SetActive(true);
        VictoryScreen.GetComponent<UIFade>().ToFadeIn();
    }

    public void DisplayDefeatScreen()
    {
        Destroy(CameraMove.Instance);
        DefeatScreen.SetActive(true);
        DefeatScreen.GetComponent<UIFade>().ToFadeIn();
    }

    public void RestartCombat()
    {
        SceneLoader.Instance.ReloadScene();
    }

    public void ReturnToOverworld()
    {
        UnitManager.Instance.EndingCombat();
    }

    public void ReturnToMainMenu()
    {
        GameManager.Instance.ReturnToDefault();
        SceneLoader.Instance.LoadNewScene(UnitManager.Instance.OverWorldScene);
    }
}
