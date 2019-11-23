using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : Controller
{
    public override void Start()
    {
        MoveOrder = 1;
        SerializedUnits.ForEach( (UnitSerializeInfo info) => {
            info.positionInGrid = new Vector2Int(GridController.GetGridController().GridSize.x - 1 - info.positionInGrid.x, info.positionInGrid.y);
        });
        team = UnitTeam.Enemy;
        base.Start();
    }

    public override void ReceiveControl()
    {
        Debug.Log("AI received control");
        PassControl();
    }
}
