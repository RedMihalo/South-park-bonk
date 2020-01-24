using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PickerScript : MonoBehaviour
{
    public Text stanAmountText;
    int stanAmount;
    int kayleAmount;
    public Text kayleAmountText;
    public Text chosenLimitText;
    int chosenLimit;
    public GameObject stanSlot;
    public GameObject kayleSlot;
    public GameObject fightButton;
    //public List<string> chosenUnits;
    ChosenUnits chosenUnits;
    // Start is called before the first frame update
    void Start()
    {
        chosenLimit = 4;
        chosenLimitText.text = chosenLimit.ToString();
        if (!PlayerPrefs.HasKey("Stan"))
        {
            stanAmount = 1;
            PlayerPrefs.SetInt("Stan", 1);
        }
        else
        {
            stanAmount = PlayerPrefs.GetInt("Stan");
        }
        kayleAmount = PlayerPrefs.GetInt("Kayle");
        stanAmountText.text = stanAmount.ToString();
        kayleAmountText.text = kayleAmount.ToString();
        //chosenUnits = new List<string>();
        chosenUnits = new ChosenUnits();
        chosenUnits.units = new List<string>();
        InitatePicker();
    }

    // Update is called once per frame
    void Update()
    {
        stanAmountText.text = stanAmount.ToString();
        kayleAmountText.text = kayleAmount.ToString();
        chosenLimitText.text = chosenLimit.ToString();
    }

    private void InitatePicker()
    {
        //string slotName = stanSlot.name;
        stanSlot.GetComponent<Button>().onClick.AddListener(() => OnButtonClick("Stan"));
        kayleSlot.GetComponent<Button>().onClick.AddListener(() => OnButtonClick("Kayle"));
        fightButton.GetComponent<Button>().onClick.AddListener(() => OnFightClick());
    }

    private void OnButtonClick(string unitName)
    {
        if(chosenUnits.units.Count < 4)
        {
            chosenLimit--;
            chosenUnits.units.Add(unitName);
        }
    }

    private void OnFightClick()
    {
        if(chosenUnits.units.Count == 0)
            return;
        string s = chosenUnits.Serialize();
        PlayerPrefs.SetString("ChosenUnits", s);
        Debug.Log(s);
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);

    }
}
