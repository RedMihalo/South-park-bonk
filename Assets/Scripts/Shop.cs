using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Shop : MonoBehaviour
{
    public Text moneyText;
    int money;
    public int startMoney;
    [SerializeField] private ShopItemsPreset[] shopItems;
    [SerializeField] private Transform slotsParent;
    [SerializeField] private GameObject shopItemPrefab;

    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("money"))
        {
            PlayerPrefs.SetInt("money", startMoney);
            money = startMoney;
            moneyText.text = money.ToString();
        }
        else
        {
            money = PlayerPrefs.GetInt("money");
            moneyText.text = money.ToString();
        }

        PopulateShop();
    }

    private void Update()
    {
        moneyText.text = money.ToString();
    }

    private void PopulateShop()
    {
        for (int i = 0; i < shopItems.Length; i++)
        {
            ShopItemsPreset item = shopItems[i];
            GameObject itemObject = Instantiate(shopItemPrefab, slotsParent);

          
            itemObject.GetComponent<Button>().onClick.AddListener(() => OnButtonClick(item));

            
            itemObject.transform.GetChild(0).GetComponent<Image>().sprite = item.itemMinature;
            itemObject.transform.GetChild(1).GetComponent<Text>().text = item.itemName;
            itemObject.transform.GetChild(2).GetComponent<Text>().text = item.cost.ToString();
        }
    }

    private void OnButtonClick(ShopItemsPreset item)
    {
        Debug.Log(item.itemName);
        if (money >= item.cost)
        {
            money = money - item.cost;
            PlayerPrefs.SetInt("money", money);
            if (item.itemName.Equals("Stan"))
            {
                int i = PlayerPrefs.GetInt("Stan");
                i++;
                PlayerPrefs.SetInt("Stan",i);
            }
            if (item.itemName.Equals("Kayle"))
            {
                int i = PlayerPrefs.GetInt("Kayle");
                i++;
                PlayerPrefs.SetInt("Kayle", i);
            }
        }

    }
}
