using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ShopUnitInfo
{
    public GameObject Unit;
    public int Cost;
}

public class Shop : MonoBehaviour
{
    public List<ShopUnitInfo> UnitsInStock;

    // Start is called before the first frame update
    void Start()
    {
        foreach(var o in UnitsInStock)
            Debug.Log(o.Unit);
    }
}
