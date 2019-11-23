using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct AttributeValuePair
{
    public Attribute Attribute;
    public int value;
}


[CreateAssetMenu(fileName = "Attribute preset", menuName = "ScriptableObjects/AttributePreset", order = 1)]
public class AttributesPreset : ScriptableObject
{
    public List<AttributeValuePair> Attributes;
}
