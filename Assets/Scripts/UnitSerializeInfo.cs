using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[System.Serializable]
public class UnitSerializeInfo
{
    public Vector2Int positionInGrid;
    public UnitTeam team;

    public static UnitSerializeInfo SerializeUnit(GameObject unit)
    {
        UnitSerializeInfo toReturn = new UnitSerializeInfo();

        BattleUnit asBattleUnit = unit.GetComponent<BattleUnit>();
        toReturn.positionInGrid = asBattleUnit.CurrentTile.PositionInGrid;
        toReturn.team = asBattleUnit.team;

        return toReturn;
    }

    public static void DeserializeUnit(GameObject unit, UnitSerializeInfo info)
    {
        BattleUnit asBattleUnit = unit.GetComponent<BattleUnit>();
        asBattleUnit.InitialGridPosition = info.positionInGrid;
        asBattleUnit.team = info.team;
    }

}
