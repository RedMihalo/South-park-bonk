using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public List<Controller> Controllers;
    public Controller NextController { get => Controllers[(index + 1) % Controllers.Count]; }
    private Controller CurrentController { get => Controllers[(index) % Controllers.Count]; }

    private int index = -1;
    void Start()
    {
        foreach(var c in Controllers)
            c.OnPassControl.AddListener(() => ActivateNextController());

        ActivateNextController();
    }

    private void ActivateNextController()
    {
        index++;
        index = index % Controllers.Count;

        if(CurrentController.GetUnits().Count > 0)
            CurrentController.ReceiveControl();
        else
            Debug.Log("KONICE");
    }

}
