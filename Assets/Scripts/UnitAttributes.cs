using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum Attribute
{
    Range,
    Defence,
    Attack,
    MaxHealth
}

public class UnitAttributes : MonoBehaviour
{
    //[HideInInspector]
    public List<AttributeValuePair> Attributes = new List<AttributeValuePair>();
    public AttributesPreset AttributesPreset;

    private void Start()
    {
        foreach(AttributeValuePair a in AttributesPreset.Attributes)
            Attributes.Add(a);

        Func<AttributeValuePair, Attribute, bool> Predicate = (AttributeValuePair P, Attribute A) => { return P.Attribute == A; };
        foreach(Attribute A in Enum.GetValues(typeof(Attribute)))
        {
            if(Attributes.FindAll((AttributeValuePair P) => Predicate(P, A)).Count > 1)
                Debug.LogError("eee, " + gameObject + " has multiple values for attribute: " + A);
        }
    }

    public int GetAttributeValue(Attribute a)
    {
        int index = Attributes.FindIndex((AttributeValuePair P) => { return P.Attribute == a; });
        if(index < 0)
            return -1;
        return Attributes[index].value;
    }

    public void ClearAttributes()
    {
        Attributes.Clear();
    }

    public void AddAttribute(AttributeValuePair attribute)
    {
        if(Attributes.FindIndex((AttributeValuePair p) => p.Attribute == attribute.Attribute) != -1)
        {
            Debug.LogError("Trying to add already existing attribute");
            return;
        }

        Attributes.Add(attribute);
    }
}
