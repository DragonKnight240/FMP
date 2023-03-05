using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class ForSale
{
    public Item item;
    public int AmountInStock;
    public Sprite Image;
    public int Price;
}

[System.Serializable]
public struct ItemUI
{
    public GameObject MainObject;
    public TMP_Text ItemName;
    public Image ItemImage;
    public TMP_Text ItemText;
    public ForSale Item;
    public TMP_Text Price;
}

public class Shop : MonoBehaviour
{
    public static Shop Instance; 
    internal List<ForSale> ItemsForSale;
    public GameObject ShopMenuObject;
    public List<ItemUI> Items;
    public TMP_Text MoneyAmount;
    public GameObject ShopItem;

    public GameObject ItemDetails;
    public TMP_Text DamageNum;
    public TMP_Text CritRateNum;
    public TMP_Text HitRateNum;
    public TMP_Text DurabilityNum;
    public TMP_Text RangeNum;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        ShopMenuObject.GetComponent<CanvasGroup>().alpha = 0;
        ShopMenuObject.SetActive(false);

        ItemDetails.GetComponent<CanvasGroup>().alpha = 0;
        ItemDetails.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void BuyItem(int Index)
    {
        if(GameManager.Instance.Money - Items[Index].Item.Price >= 0)
        {
            GameManager.Instance.Money = GameManager.Instance.Money - Items[Index].Item.Price;
            GameManager.Instance.Convoy.Add(Items[Index].Item.item);
            Items[Index].Item.AmountInStock--;
            if(Items[Index].Item.AmountInStock <= 0)
            {
                Items[Index].MainObject.SetActive(false);
            }
            else
            {
                Items[Index].ItemText.text = "x" + ItemsForSale[Index].AmountInStock.ToString();
            }
        }

        MoneyAmount.text = GameManager.Instance.Money.ToString();
    }

    void UpdateShop()
    {
        for(int i = 0; i < Items.Count; i++)
        {
            if (ItemsForSale.Count <= i)
            {
                Items[i].MainObject.SetActive(false);
                continue;
            }

            if (ItemsForSale[i].AmountInStock == 0)
            {
                Items[i].MainObject.SetActive(false);
                continue;
            }

            Items[i].Price.text = ItemsForSale[i].Price.ToString();
            Items[i].ItemName.text = ItemsForSale[i].item.Name;
            Items[i].ItemText.text = "x" + ItemsForSale[i].AmountInStock.ToString();
            Items[i].ItemImage.sprite = ItemsForSale[i].Image;
            Items[i] = new ItemUI { MainObject = Items[i].MainObject, ItemName = Items[i].ItemName, ItemImage = Items[i].ItemImage, ItemText = Items[i].ItemText, Item = ItemsForSale[i], Price = Items[i].Price };

            Items[i].MainObject.SetActive(true);
            Items[i].MainObject.GetComponent<ItemButton>().LinkedItem = Items[i].Item.item;
        }

        MoneyAmount.text = GameManager.Instance.Money.ToString();
    }

    internal void UpdateItemDetails(Weapon item)
    {
        DamageNum.text = item.Damage.ToString();
        CritRateNum.text = item.CritRate.ToString();
        HitRateNum.text = item.HitRate.ToString();
        DurabilityNum.text = item.Durablity.ToString();
        RangeNum.text = item.Range.ToString();

        ItemDetails.SetActive(true);
        ItemDetails.GetComponent<UIFade>().ToFadeIn();
    }

    internal void CloseItemDetails()
    {
        ItemDetails.SetActive(false);
    }

    public void CloseShop()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        ShopMenuObject.GetComponent<UIFade>().ToFadeOut();
    }

    public void OpenShop()
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.Confined;
        UpdateShop();
        ShopMenuObject.SetActive(true);
        ShopMenuObject.GetComponent<UIFade>().ToFadeIn();
    }
}
