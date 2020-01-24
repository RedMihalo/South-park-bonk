using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ChosenUnits
{
    public List<string> units;

    //public ChosenUnits()
    //{
    //    this.units = new List<string>();
    //}

    public string Serialize()
    {
        return JsonUtility.ToJson(this);
    }
}
