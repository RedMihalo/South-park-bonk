using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Shop Item")]
public class ShopItemsPreset : ScriptableObject
{
    public string itemName;
    public Sprite itemMinature;
    public int cost;

}
