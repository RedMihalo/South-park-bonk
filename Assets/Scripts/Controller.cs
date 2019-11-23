﻿using System;
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
    protected List<GameObject> Units = new List<GameObject>();
    protected GameObject CurrentUnit = null;
    public GameObject UnitPrefab;

    // public BattleScreenManager BattleManager;
    public Controller NextController;

    protected UnitTeam team;
    protected int MoveOrder;

    private UnityEngine.Events.UnityAction DestinationReachedCallback;
    private UnityEngine.Events.UnityAction AttackFinishedCallback;

    protected static readonly Func<Tile, GameObject, bool> TileInMoveRange = (Tile t, GameObject Unit) =>
    {
        if(t.CurrentUnit != null)
            return false;
        return
            Unit.GetComponent<BattleUnit>().CurrentTile == null ||
            GridController.ManhattanDistance(t.PositionInGrid, Unit.GetComponent<BattleUnit>().CurrentTile.PositionInGrid) <=
            Unit.GetComponent<UnitAttributes>().GetAttributeValue(Attribute.Range);
    };

    protected static readonly Func<Tile, GameObject, bool> TileInMeleeAttackRange = (Tile t, GameObject Unit) =>
    {
        if(Unit.GetComponent<BattleUnit>().CurrentTile == null)
            return false;
        return GridController.ManhattanDistance(Unit.GetComponent<BattleUnit>().CurrentTile.PositionInGrid, t.PositionInGrid) == 1;
    };

    protected static readonly Func<Tile, GameObject, bool> UnitInMeleeRange = (Tile t, GameObject Unit) =>
    {
        return TileInMeleeAttackRange(t, Unit) && t.CurrentUnit != null;
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
            GameObject lastObject = Instantiate(UnitPrefab, transform.position, Quaternion.identity);
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

    public void PassControl()
    {
        CurrentUnit = null;
        NextController.ReceiveControl();
    }
}
