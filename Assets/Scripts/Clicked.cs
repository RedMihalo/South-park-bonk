using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Clicked : MonoBehaviour, IPointerClickHandler
{
    public Text textUI;

    public Clicked() {

    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnPointerClick(PointerEventData eventData) // 3
    {
         textUI.text+="CLIIIIIIIIIIIIIICKED";
    }

    public void Test()
    {
        textUI.text+="SUCSESECUSCEUSEUSECS";
    }
}
