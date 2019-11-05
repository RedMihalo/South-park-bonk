using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPicker : MonoBehaviour
{
    public Button CharacterPickButton;
    public Image Circle;

    public bool PickerEnabled
    {
        set
        {
            CharacterPickButton.interactable = value;
            Circle.enabled = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        CharacterPickButton.onClick.AddListener(() => { BattleScreenManager.SetMovedUnit(transform.root.gameObject);  });
    }

}
