using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;
public class Level : MonoBehaviour
{
    public NODE_TYPE type;
    public List<Sprite> sprites;
    public Button Button;
    public enum NODE_TYPE
    {
        FIGHT,
        SHOP,
        STORY
    }

    void Start()
    {
        type = (NODE_TYPE)Random.Range(0, Enum.GetNames(typeof(NODE_TYPE)).Length);
        if(type == NODE_TYPE.FIGHT)
        {
            Button.onClick.AddListener(()=>ChangeScene("SampleScene"));
        }
        if (type == NODE_TYPE.STORY)
        {
            Button.onClick.AddListener(() => ChangeScene("FreeRoamScene"));
        }
        GameObject text = this.transform.Find("Text").gameObject;
        text.GetComponent<Text>().text = type.ToString();
        GetComponent<Image>().sprite = sprites[(int)type];
    }
    void ChangeScene(string Name)
    {
        SceneManager.LoadScene(Name);
    }
    
    

}


