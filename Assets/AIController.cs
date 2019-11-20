using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : Controller
{
    public override void Start()
    {
        SerializedUnits.ForEach( (UnitSerializeInfo info) => {
            info.positionInGrid = new Vector2Int(BattleScreenManager.GetGridController().GridSize.x - 1 - info.positionInGrid.x, info.positionInGrid.y);
        });
        team = UnitTeam.Enemy;
        base.Start();
    }
}
