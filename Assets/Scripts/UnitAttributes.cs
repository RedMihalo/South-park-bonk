using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum Attribute
{
    Range,
    Defence,
    Attack
}

[System.Serializable]
public struct AttributeValuePair
{
    public Attribute Attribute;
    public int value;
}

public class UnitAttributes : MonoBehaviour
{
    public List<AttributeValuePair> Attributes;

    private void Start()
    {
        Func<AttributeValuePair, Attribute, bool> Predicate = (AttributeValuePair P, Attribute A) => { return P.Attribute == A; };
        foreach(Attribute A in Enum.GetValues(typeof(Attribute)))
        {
            if(Attributes.FindAll((AttributeValuePair P) => Predicate(P, A)).Count > 1)
                Debug.LogError("eee, " + gameObject + " has multiple values for attribute: " + A);
        }
    }

    public int? GetAttributeValue(Attribute a)
    {
        int index = Attributes.FindIndex((AttributeValuePair P) => { return P.Attribute == a; });
        if(index < 0)
            return null;
        return Attributes[index].value;
    }
}
