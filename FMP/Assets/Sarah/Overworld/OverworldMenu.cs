using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OverworldMenu : MonoBehaviour
{
    public GameObject InventoryMenu;
    public GameObject ButtonsMenu;
    public GameObject UnitSection;
    public GameObject ConveySection;

    public Button SwordButton;
    public Button MageButton;
    public Button GauntletButton;
    public Button BowButton;

    public TMP_Text Health;
    public TMP_Text Strength;
    public TMP_Text Dex;
    public TMP_Text Magic;
    public TMP_Text Resistance;
    public TMP_Text Speed;
    public TMP_Text Luck;
    public TMP_Text Class;
    public TMP_Text Name;

    public TMP_Text LevelBow;
    public TMP_Text LevelMagic;
    public TMP_Text LevelSword;
    public TMP_Text LevelGauntlet;

    public List<GameObject> InventoryItems;

    public GameObject ConvoyItems;
    public GameObject ConvoyPrefab;
    internal List<GameObject> ConvoyPrefabItems;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.CombatTutorialComplete)
        {
            if(Input.GetButtonDown("OpenInventory"))
            {
                OpenInventory();
            }
        }
    }

    public void SwitchToConvoy()
    {
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
            }
        }

        Health.text = Character.CurrentHealth.ToString() + " / " + Character.HealthMax.ToString(); ;
        Strength.text = Character.Strength.ToString();
        Dex.text = Character.Dexterity.ToString();
        Magic.text = Character.Magic.ToString();
        Resistance.text = Character.Resistance.ToString();
        Speed.text = Character.Speed.ToString();
        Luck.text = Character.Luck.ToString();
        Class.text = Character.Class.Name.ToString();
        Name.text = Character.UnitName.ToString();

        LevelBow.text = Character.Strength.ToString();
        LevelGauntlet.text = Character.Strength.ToString();
        LevelMagic.text = Character.Strength.ToString();
        LevelSword.text = Character.Strength.ToString();

        int Index = 0;
        foreach(Item item in Character.Inventory)
        {
            if(Index > Character.Inventory.Count)
            {
                InventoryItems[Index].SetActive(false);
                continue;
            }
            InventoryItems[Index].GetComponentInChildren<TMP_Text>().text = item.Name;

            InventoryItems[Index].SetActive(true);
        }

        ConveySection.SetActive(false);
        UnitSection.SetActive(true);
        InventoryMenu.SetActive(true);
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
