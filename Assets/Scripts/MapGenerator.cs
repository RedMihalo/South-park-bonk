using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour
{

    public int maxOptions = 3;
    public GameObject Level;

    // Start is called before the first frame update
    void setUp()
    {
        int amount = Random.Range(1, maxOptions + 1);
        for (int j = 0; j < amount; j++)
        {
            GameObject obj = Instantiate(Level, this.transform) as GameObject;
            obj.transform.SetParent(this.transform);
        }
    }
    void Start()
    {
        setUp();
    }
}
