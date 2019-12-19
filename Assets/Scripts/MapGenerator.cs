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
    public List<List<GameObject>> map;
    public int curentStage = 0;




    // Start is called before the first frame update
    void setUp()
    {
        map = new List<List<GameObject>>();
        for (int i = 0; i < stages; i++)
        {
            List<GameObject> stage = new List<GameObject>();
            int amount = Random.Range(1, maxOptions);
            for (int j = 0; j < amount; j++)
            {
                GameObject level = Instantiate(Level, this.transform) as GameObject;
                level.SetActive(false);
                stage.Add(level);
            }
            map.Add(stage);
        }
    }

    public void changeStage()
    {
        foreach(GameObject level in map[curentStage])
        {
            level.SetActive(true);
            curentStage++;
        }
    }
    void Start()
    {
        setUp();
        changeStage();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
