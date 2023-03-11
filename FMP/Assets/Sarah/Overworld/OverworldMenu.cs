using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OverworldMenu : MonoBehaviour
{
    static public OverworldMenu Instance;

    public GameObject InventoryMenu;
    public GameObject ButtonsMenu;
    public GameObject UnitSection;
    public GameObject ConveySection;
    public GameObject AttacksMenu;

    public Button SwordButton;
    public Button MageButton;
    public Button GauntletButton;
    public Button BowButton;

    public TMP_Text Health;
    public TMP_Text Strength;
    public TMP_Text Dex;
    public TMP_Text Magic;
    public TMP_Text Defence;
    public TMP_Text Resistance;
    public TMP_Text Speed;
    public TMP_Text Luck;
    public TMP_Text Class;
    public TMP_Text Name;
    public TMP_Text Level;

    public TMP_Text LevelBow;
    public TMP_Text LevelMagic;
    public TMP_Text LevelSword;
    public TMP_Text LevelGauntlet;

    public List<GameObject> InventoryItems;

    public GameObject AttackParent;
    internal List<GameObject> Attacks;

    public GameObject ItemDetails;

    public TMP_Text ItemName;
    public TMP_Text Damage;
    public TMP_Text Crit;
    public TMP_Text Hit;
    public TMP_Text Range;
    public TMP_Text Durability;

    public GameObject ConvoyItems;
    public GameObject ConvoyPrefab;
    internal List<GameObject> ConvoyPrefabItems;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        ConvoyPrefabItems = new List<GameObject>();

        Attacks = new List<GameObject>();

        foreach(Transform Child in AttackParent.transform)
        {
            Attacks.Add(Child.gameObject);
        }

        ItemDetails.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.CombatTutorialComplete)
        {
            if(Input.GetButtonDown("OpenInventory"))
            {
                ToggleInventory();
            }
        }
    }

    public void ToggleInventory()
    {
        if(InventoryMenu.activeInHierarchy)
        {
            CloseInventory();
        }
        else
        {
            OpenInventory();
        }
    }

    public void SwitchToConvoy()
    {
        foreach(GameObject gameObject in ConvoyPrefabItems)
        {
            Destroy(gameObject);
        }

        foreach(Item item in GameManager.Instance.Convoy)
        {
            ConvoyPrefabItems.Add(Instantiate(ConvoyPrefab, ConvoyItems.transform));
            ConvoyPrefabItems[ConvoyPrefabItems.Count - 1].GetComponentInChildren<TMP_Text>().text = item.Name;
        }

        UnitSection.SetActive(false);
        ConveySection.SetActive(true);

    }

    public void ChangeUnitData(string UnitName)
    {
        CharacterData Character = new CharacterData();
        foreach(CharacterData Data in GameManager.Instance.UnitData)
        {
            if(Data.UnitName.Contains(UnitName))
            {
                Character = Data;
                break;
            }
        }

        if(Character == null)
        {
            print("No Character with Name");
            return;
        }

        Level.text = Character.Level.ToString();
        Health.text = Character.CurrentHealth.ToString() + " / " + Character.HealthMax.ToString(); ;
        Strength.text = Character.Strength.ToString();
        Dex.text = Character.Dexterity.ToString();
        Magic.text = Character.Magic.ToString();
        Defence.text = Character.Defence.ToString();
        Resistance.text = Character.Resistance.ToString();
        Speed.text = Character.Speed.ToString();
        Luck.text = Character.Luck.ToString();
        Class.text = Character.Class.Name.ToString();
        Name.text = Character.UnitName.ToString();

        LevelBow.text = Character.BowLevel.ToString();
        LevelGauntlet.text = Character.FistLevel.ToString();
        LevelMagic.text = Character.MagicLevel.ToString();
        LevelSword.text = Character.SwordLevel.ToString();

        int Index = 0;

        foreach(GameObject InventorySlots in InventoryItems)
        {
            if (Index > Character.Inventory.Count - 1)
            {
                InventorySlots.GetComponentInChildren<TMP_Text>().text = "None";
                InventorySlots.GetComponent<ItemButton>().LinkedItem = null;
                InventorySlots.SetActive(true);
                Index++;
                continue;
            }

            InventorySlots.GetComponentInChildren<TMP_Text>().text = Character.Inventory[Index].Name;
            InventorySlots.GetComponent<ItemButton>().LinkedItem = Character.Inventory[Index];
            InventorySlots.SetActive(true);

            Index++;
        }

        ConveySection.SetActive(false);
        UnitSection.SetActive(true);
        InventoryMenu.SetActive(true);

        if(!Shop.Instance.gameObject.activeInHierarchy)
        {
            DisplayAttacks(Character);
        }

        AttacksMenu.SetActive(Shop.Instance.gameObject.activeInHierarchy || Character.UnlockedAttacks.Count == 0? false: true);
    }

    internal void DisplayAttacks(CharacterData Unit)
    {
        int Index = 0;

        foreach(GameObject Attack in Attacks)
        {
            if(Unit.UnlockedAttacks.Count - 1 < Index)
            {
                Index++;
                Attack.SetActive(false);
                continue;
            }

            Attack.GetComponentInChildren<TMP_Text>().text = Unit.UnlockedAttacks[Index].Name;
            Attack.SetActive(true);

            Index++;
        }
    }

    public void ItemDisplay(Weapon item)
    {
        ItemName.text = item.Name;
        Damage.text = item.Damage.ToString();
        Crit.text = item.CritRate.ToString();
        Hit.text = item.HitRate.ToString();
        Range.text = item.Range.ToString();
        Durability.text = item.Durablity.ToString() + " / " + item.CurrentDurablity.ToString();

        ItemDetails.SetActive(true);
    }

    public void ItemUndisplay()
    {
        ItemDetails.SetActive(false);
    }

    public void OpenInventory()
    {
        Time.timeScale = 0;
        SwordButton.gameObject.SetActive(true);
        MageButton.gameObject.SetActive(true);

        if(GameManager.Instance.ArcherRecruitComplete)
        {
            BowButton.gameObject.SetActive(true);
        }
        else
        {
            BowButton.gameObject.SetActive(false);
        }

        if(GameManager.Instance.GauntletRecruitComplete)
        {
            GauntletButton.gameObject.SetActive(true);
        }
        else
        {
            GauntletButton.gameObject.SetActive(false);
        }

        ChangeUnitData("Zoom");

        Cursor.lockState = CursorLockMode.Confined;

    }

    public void CloseInventory()
    {
        InventoryMenu.SetActive(false);
        ConveySection.SetActive(false);
        UnitSection.SetActive(false);

        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
    }

}
