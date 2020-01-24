using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickedButton : MonoBehaviour
{
    public Button InteractButton;
    public Text textUI;
    // Start is called before the first frame update
    void Start()
    {
        InteractButton.onClick.AddListener(() => TryInteract());
    }

    private void TryInteract()
    {
        textUI.text = "CLICKEEEEEEEED";
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
