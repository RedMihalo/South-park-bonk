using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AIController : Controller
{

    public override void Start()
    {
        spawnColumn = GridController.GetGridController().GridSize.x - 1;
        MoveOrder = 1;
        SerializedUnits.ForEach( (UnitSerializeInfo info) => {
            info.positionInGrid = new Vector2Int(GridController.GetGridController().GridSize.x - 1 - info.positionInGrid.x, info.positionInGrid.y);
        });
        team = UnitTeam.Enemy;
        base.Start();
    }

    private void ChooseAction()
    {
        Debug.Log("Choose action");
        List<GameObject> unitsWithTarget = Units.FindAll((GameObject o) => o.GetComponent<BattleUnit>().HasUnitsInAttackRange());
        if(unitsWithTarget.Count > 0)
        {
            Debug.Log("attack");
            CurrentUnit = unitsWithTarget[UnityEngine.Random.Range(0, unitsWithTarget.Count)];
            List<GameObject> possibleTargets = CurrentUnit.GetComponent<BattleUnit>().GetUnitsInRange();
            Attack(possibleTargets[UnityEngine.Random.Range(0, possibleTargets.Count)]);
        }
        else
            Move();
    }

    private void Move()
    {
        Debug.Log("move");
        Tuple<BattleUnit, Tile> moveInfo = GetMoveInfo();
        CurrentUnit = moveInfo.Item1.gameObject;
        MoveUnit(moveInfo.Item2);
        // GridController.GetGridController().MoveUnit(moveInfo.Item1.gameObject, moveInfo.Item2);
    }

    private List<Tile> GetTileNextToTarget(GameObject target)
    {
        return GridController.GetGridController().GetTiles().FindAll((Tile t) => Controller.TileInAttackRange(t, target));
    }

    private IEnumerator PassCorutine()
    {
        Debug.Log("Waiting....");
        yield return new WaitForSeconds(2);
        Debug.Log("Passing");
        PassControl();
    }

    public override void ReceiveControl()
    {
        ChooseAction();
    }

    private Tuple<BattleUnit, Tile> GetMoveInfo()
    {
        List<GameObject> nextUnits = Manager.NextController.GetUnits();

        BattleUnit battleUnit = Units[UnityEngine.Random.Range(0, Units.Count)].GetComponent<BattleUnit>();
        BattleUnit target = battleUnit.ClosestUnit();
        target.GetComponent<CharacterPicker>().PickerEnabled = true;

        List<Tile> tiles = GridController.GetGridController().GetTiles();
        tiles = tiles.FindAll((Tile t) =>
        {
            return TileInRange(t, target.gameObject);
        });

        Tile targetTile = battleUnit.CurrentTile.GetClosestTile(tiles);

        targetTile = GridController.GetDestinationTileInRange(battleUnit.CurrentTile, targetTile, battleUnit.GetComponent<UnitAttributes>().GetAttributeValue(Attribute.Range));

        return new Tuple<BattleUnit, Tile>(battleUnit, targetTile);
    }

    private bool TileInRange(Tile t, GameObject unit)
    {
        return GridController.ManhattanDistance(unit.GetComponent<BattleUnit>().CurrentTile, t) == 1;
    }

    protected override List<UnitSerializeInfo> GetUnitSerializeInfos()
    {
        return SerializedUnits;
    }
}
