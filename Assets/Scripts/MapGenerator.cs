using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour
{

    public int stages = 10;
    public int maxOptions = 3;
    public GameObject Level;
    public List<GameObject> map;
    public int curentStage = 0;




    // Start is called before the first frame update
    void setUp()
    {
        map = new List<GameObject>();
        int amount = Random.Range(1, maxOptions + 1);
        for (int j = 0; j < amount; j++)
        {
            GameObject obj = Instantiate(Level, this.transform) as GameObject;
            map.Add(obj);
        }
    }

    public void changeStage()
    {
        //for(int i = 0; i < this.transform.childCount; i++)
        //{
        //    this.transform.GetChild(i).gameObject.SetActive(false);
        //}
        this.transform.DetachChildren();
        setUp();
    }
    void Start()
    {
        setUp();
        //changeStage();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
