using System.Linq;
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

    private void ChooseAction()
    {
        List<GameObject> unitsWithTarget = Units.FindAll((GameObject o) => o.GetComponent<BattleUnit>().HasUnitsInAttackRange());
        if(unitsWithTarget.Count > 0)
            Attack();
        else
            Move();
    }

    private void Attack()
    {
        throw new System.NotImplementedException();
    }

    private void Move()
    {
        PickUnit(Units[Random.Range(0, Units.Count)]);
        Tile target = GridController.GetGridController().GetTile(
            CurrentUnit.GetComponent<BattleUnit>().CurrentTile.PositionInGrid + new Vector2Int(-1, 0)
            );
        List<GameObject> potentialUnits = Manager.NextController.GetUnits().FindAll((GameObject o) => {
            return GridController.ManhattanDistance(
                CurrentUnit.GetComponent<BattleUnit>().CurrentTile, o.GetComponent<BattleUnit>().CurrentTile
                ) <= CurrentUnit.GetComponent<UnitAttributes>().GetAttributeValue(Attribute.Range);
        });
        potentialUnits.Sort((GameObject a, GameObject b) =>
        {
            return GridController.ManhattanDistance(a.GetComponent<BattleUnit>().CurrentTile, b.GetComponent<BattleUnit>().CurrentTile);
        });
        Debug.Log(potentialUnits.Count);
        if(potentialUnits.Count == 0)
        {
            Debug.Log("BONGOS");
            PassControl();
            return;
        }

        MoveUnit(GridController.GetGridController().GetTile(potentialUnits[0].GetComponent<BattleUnit>().CurrentTile.PositionInGrid + new Vector2Int(1, 0)));
        // GridController.GetGridController().EnableValidTiles((Tile t) => potentialUnits.Contains(t.CurrentUnit));

        // StartCoroutine(PassCorutine());

        //var possibleTargets = NextController.GetUnits();
        //possibleTargets.Sort((GameObject a, GameObject b) =>
        //{
        //    return GridController.ManhattanDistance(a.GetComponent<BattleUnit>().CurrentTile, b.GetComponent<BattleUnit>().CurrentTile);
        //});


        //GameObject unitToMoveTo = possibleTargets.Count > 0 ? possibleTargets[0] : null;
        //if(unitToMoveTo)
        //    throw new System.Exception("Oh boy");

        //List<Tile> targetTiles = GridController.GetGridController().Gettiles().FindAll((Tile t) => TileInMoveRange(t, unitToMoveTo));
        //targetTiles.Sort((Tile a, Tile b) => 
        //    GridController.ManhattanDistance(a, b)
        //);

        //MoveUnit(targetTiles[0]);
    }

    private List<Tile> GetTileNextToTarget(GameObject target)
    {
        return GridController.GetGridController().GetTiles().FindAll((Tile t) => Controller.TileInMeleeAttackRange(t, target));
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
        List<GameObject> nextUnits = Manager.NextController.GetUnits();

        BattleUnit battleUnit = Units[Random.Range(0, Units.Count)].GetComponent<BattleUnit>();
        BattleUnit target = battleUnit.ClosestUnit();
        target.GetComponent<CharacterPicker>().PickerEnabled = true;

        List<Tile> tiles = GridController.GetGridController().GetTiles();
        tiles = tiles.FindAll((Tile t) =>
        {
            return TileInRange(t, target.gameObject);
        });

        Tile targetTile = battleUnit.CurrentTile.GetClosestTile(tiles);

        GridController.GetGridController().EnableValidTiles((Tile t) => t == targetTile);

        StartCoroutine(PassCorutine());
        // ChooseAction();
    }

    private bool TileInRange(Tile t, GameObject unit)
    {
        return GridController.ManhattanDistance(unit.GetComponent<BattleUnit>().CurrentTile, t) == 1;
    }
}
