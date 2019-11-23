using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CharacterPicker : MonoBehaviour
{
    public GameObject UIPanel;
    public Button CharacterPickButton;
    public Image Circle;

    public class CharacterPickEvent : UnityEvent<CharacterPicker> { };
    public CharacterPickEvent OnCharacterPicked = new CharacterPickEvent();

    public bool PickerEnabled
    {
        set
        {
            UIPanel.SetActive(value);
            CharacterPickButton.interactable = value;
            Circle.gameObject.SetActive(value);
            // Circle.enabled = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        CharacterPickButton.onClick.AddListener(() => { OnCharacterPicked.Invoke(this); });
    }

}
