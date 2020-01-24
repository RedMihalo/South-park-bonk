using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class KeyToUnitPrefabMapping
{
    public string name;
    public GameObject prefab;
}

[CreateAssetMenu(fileName = "Unit mapping", menuName = "ScriptableObjects/UnitMapping", order = 2)]
public class UnitPregfabsMap : ScriptableObject
{
    public List<KeyToUnitPrefabMapping> mapping;
}


[Serializable]
public class UnitSerializeInfo
{
    public List<AttributeValuePair> attributes;
    public Vector2Int positionInGrid;
    public UnitTeam team;
    public GameObject prefab;

    public static UnitSerializeInfo SerializeUnit(GameObject unit)
    {
        UnitSerializeInfo toReturn = new UnitSerializeInfo();

        BattleUnit asBattleUnit = unit.GetComponent<BattleUnit>();
        toReturn.positionInGrid = asBattleUnit.CurrentTile.PositionInGrid;
        toReturn.team = asBattleUnit.team;

        foreach(var a in unit.GetComponent<UnitAttributes>().Attributes)
            toReturn.attributes.Add(a);

        return toReturn;
    }

    public static void DeserializeUnit(GameObject unit, UnitSerializeInfo info)
    {
        BattleUnit asBattleUnit = unit.GetComponent<BattleUnit>();
        asBattleUnit.InitialGridPosition = info.positionInGrid;
        asBattleUnit.team = info.team;
        /*
        UnitAttributes unitAttributes = unit.GetComponent<UnitAttributes>();

        foreach(AttributeValuePair p in info.attributes)
            unitAttributes.AddAttribute(p);
        */
    }

}
