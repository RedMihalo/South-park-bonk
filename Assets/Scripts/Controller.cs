using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public enum ControllerState
{
    ModePicking,
    Moving,
    Atttacking,
    CharacterPicking
}

public abstract class Controller : MonoBehaviour
{
    public List<UnitSerializeInfo> SerializedUnits = new List<UnitSerializeInfo>();
    [HideInInspector]
    public List<GameObject> Units = new List<GameObject>();
    [HideInInspector]
    public GameObject CurrentUnit = null;

    // public BattleScreenManager BattleManager;
    // public Controller NextController;
    public BattleManager Manager;

    protected UnitTeam team;
    protected int MoveOrder;

    private UnityEngine.Events.UnityAction DestinationReachedCallback;
    private UnityEngine.Events.UnityAction AttackFinishedCallback;
    public UnityEngine.Events.UnityEvent OnPassControl = new UnityEngine.Events.UnityEvent();

    protected static readonly Func<Tile, GameObject, bool> TileInMoveRange = (Tile t, GameObject Unit) =>
    {
        if(t.CurrentUnit != null)
            return false;
        return
            Unit.GetComponent<BattleUnit>().CurrentTile == null ||
            GridController.ManhattanDistance(t.PositionInGrid, Unit.GetComponent<BattleUnit>().CurrentTile.PositionInGrid) <=
            Unit.GetComponent<UnitAttributes>().GetAttributeValue(Attribute.Range);
    };

    protected static readonly Func<Tile, GameObject, bool> TileInAttackRange = (Tile t, GameObject Unit) =>
    {
        if(Unit.GetComponent<BattleUnit>().CurrentTile == t)
            return false;
        return 
            GridController.ManhattanDistance(Unit.GetComponent<BattleUnit>().CurrentTile, t) <=
            Unit.GetComponent<UnitAttributes>().GetAttributeValue(Attribute.AttackRange);
    };

    protected static readonly Func<Tile, GameObject, bool> UnitInAttackRange = (Tile t, GameObject Unit) =>
    {
        return TileInAttackRange(t, Unit) && t.CurrentUnit != null;
    };


    public virtual void Start()
    {
        DestinationReachedCallback = () => this.HandleDestinationReached();
        AttackFinishedCallback = () => this.HandleAttackFinished();
        DontDestroyOnLoad(gameObject);
        SpawnUnits();
    }

    private void SpawnUnits()
    {
        foreach(UnitSerializeInfo info in SerializedUnits)
        {
            GameObject lastObject = Instantiate(info.prefab, transform.position, Quaternion.identity);
            Units.Add(lastObject);
            info.team = team;
            UnitSerializeInfo.DeserializeUnit(lastObject, info);
        }

        Units.ForEach((GameObject u) =>
        {
            u.GetComponent<BattleUnit>().MoveToStartPosition();
        });
    }
    protected void Attack(GameObject target)
    {
        CurrentUnit.GetComponent<BattleUnit>().Attack(target);
        CurrentUnit.GetComponent<BattleUnit>().OnAttackFinished.AddListener(AttackFinishedCallback);
    }

    protected virtual void PickUnit(GameObject unit)
    {
        CurrentUnit = unit;
    }

    public List<GameObject> GetUnits()
    {
        List<GameObject> toReturn = new List<GameObject>();
        Units.ForEach((GameObject o) => toReturn.Add(o));
        return toReturn;
    }

    protected void MoveUnit(Tile target)
    {
        CurrentUnit.GetComponent<ObjectMover>().OnDestinationReached.AddListener(DestinationReachedCallback);
        GridController.GetGridController().MoveUnit(CurrentUnit, target);
    }

    private void HandleDestinationReached()
    {
        CurrentUnit.GetComponent<ObjectMover>().OnDestinationReached.RemoveListener(DestinationReachedCallback);
        PassControl();
    }

    private void HandleAttackFinished()
    {
        CurrentUnit.GetComponent<BattleUnit>().OnAttackFinished.RemoveListener(AttackFinishedCallback);
        PassControl();
    }

    public abstract void ReceiveControl();

    public virtual void PassControl()
    {
        CurrentUnit = null;
        OnPassControl.Invoke();
    }
}
