using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public enum BattleManagerState
{
    ModePicking,
    Moving,
    Atttacking,
    CharacterPicking,
    TilePicking,
    TargetPicking
}

public class BattleScreenManager : MonoBehaviour
{
    private static BattleScreenManager Instance;
    public GameObject MovedUnit;
    private UnityEngine.Events.UnityAction ResetMovedUnitCallback = () => ResetMovedUnit();

    public GridController GridController;

    public Image BattleShadeImage;
    private BattleManagerState CurrentState = BattleManagerState.CharacterPicking;

    public Button AttackButton;
    public Button MoveButton;

    private static readonly Func<Tile, GameObject, bool> TileInMoveRange = (Tile t, GameObject Unit) =>
    {
        if(t.CurrentUnit != null)
            return false;
        return
            Unit.GetComponent<ObjectMover>().CurrentTile == null ||
            GridController.ManhattanDistance(t.PositionInGrid, Unit.GetComponent<ObjectMover>().CurrentTile.PositionInGrid) <=
            Unit.GetComponent<UnitAttributes>().GetAttributeValue(Attribute.Range);
    };

    private static readonly Func<Tile, GameObject, bool> TileInMeleeAttackRange = (Tile t, GameObject Unit) =>
    {
        if(Unit.GetComponent<ObjectMover>().CurrentTile == null)
            return false;
        return GridController.ManhattanDistance(Unit.GetComponent<ObjectMover>().CurrentTile.PositionInGrid, t.PositionInGrid) == 1;
    };

    private static readonly Func<Tile, GameObject, bool> UnitInMeleeRange = (Tile t, GameObject Unit) =>
    {
        return TileInMeleeAttackRange(t, Unit) && t.CurrentUnit != null;
    };

    public static GridController GetGridController()
    {
        return Instance.GridController;
    }

    private void Awake()
    {
        if(!Instance)
        {
            Instance = this;

        }
        else
            Debug.LogError("Multiple battle screen managers");
    }

    private void Start()
    {
        AttackButton.onClick.AddListener(() => SetCurrentState(BattleManagerState.Atttacking));
        MoveButton.onClick.AddListener(() => SetCurrentState(BattleManagerState.Moving));

        SetCurrentState(BattleManagerState.ModePicking);
        //GridController.SetGridEnabled(false);
        //SetCharPickersEnabled(true);
        //SetButtonsActive(true);
        // Instance.BattleShadeImage.gameObject.SetActive(true);
    }

    private static void SetButtonsActive(bool bActive)
    {
        Instance.AttackButton.gameObject.SetActive(bActive);
        Instance.MoveButton.gameObject.SetActive(bActive);
    }

    public static void SetCurrentState(BattleManagerState NewState)
    {
        Instance.CurrentState = NewState;
        switch(Instance.CurrentState)
        {
            case BattleManagerState.Atttacking:
            case BattleManagerState.Moving:
                SetCharPickersEnabled(true);
                SetButtonsActive(false);
                break;
            case BattleManagerState.CharacterPicking:
                break;
            case BattleManagerState.ModePicking:
                SetCharPickersEnabled(false);
                SetButtonsActive(true);
                GetGridController().SetGridEnabled(false);
                Debug.Log("EEEE");
                break;
            case BattleManagerState.TargetPicking:
                break;
            case BattleManagerState.TilePicking:
                break;
        }
    }

    public static void SetMovedUnit(GameObject NewMovedUnit)
    {
        // GetGridController().SetGridEnabled((bool)NewMovedUnit);
        SetCharPickersEnabled(!(bool)NewMovedUnit);
        Instance.BattleShadeImage.transform.root.gameObject.SetActive(!(bool)NewMovedUnit);
        if(Instance.MovedUnit)
            Instance.MovedUnit.GetComponent<ObjectMover>().OnDestinationReached.RemoveListener(Instance.ResetMovedUnitCallback);
        Instance.MovedUnit = NewMovedUnit;

        if(!NewMovedUnit)
        {
            SetCurrentState(BattleManagerState.ModePicking);
            //GetGridController().SetGridEnabled(false);
            //if(Instance.MovedUnit)
            //    Instance.MovedUnit.GetComponent<ObjectMover>().OnDestinationReached.RemoveListener(Instance.ResetMovedUnitCallback);

            //Instance.CurrentState = BattleManagerState.CharacterPicking;
            return;
        }

        if(Instance.CurrentState == BattleManagerState.Moving)
            GetGridController().EnableValidTiles((Tile t) => { return TileInMoveRange(t, NewMovedUnit); });
        else if(Instance.CurrentState == BattleManagerState.Atttacking)
            GetGridController().EnableValidTiles((Tile t) => { return UnitInMeleeRange(t, NewMovedUnit); });

        // Instance.CurrentState = BattleManagerState.TilePicking;
        Instance.MovedUnit.GetComponent<ObjectMover>().OnDestinationReached.AddListener(Instance.ResetMovedUnitCallback);
    }

    private static void SetCharPickersEnabled(bool bEnabled)
    {
        foreach(var Picker in FindObjectsOfType<CharacterPicker>())
        {
            Picker.PickerEnabled = bEnabled;
        }
    }

    public static void TileClicked(Tile t)
    {
        switch(Instance.CurrentState)
        {
            case BattleManagerState.Moving:
                SetUnitCurrentTile(t);
                MoveUnit(t.UnitPosition);
                break;
            case BattleManagerState.Atttacking:
                GameObject target = t.CurrentUnit;
                if(target == null)
                    return;
                Instance.MovedUnit.GetComponent<BattleUnit>().Attack(target);
                break;
            default:
                Debug.LogError("This shoudln't happen");
                break;
        }
    }

    public static void SetUnitCurrentTile(Tile t)
    {
        if(!Instance.MovedUnit)
            return;
        if(Instance.MovedUnit.GetComponent<ObjectMover>().CurrentTile)
            Instance.MovedUnit.GetComponent<ObjectMover>().CurrentTile.CurrentUnit = null;

        Instance.MovedUnit.GetComponent<ObjectMover>().CurrentTile = t;
        t.CurrentUnit = Instance.MovedUnit;
    }

    public static void MoveUnit(Vector3 target)
    {
        if(!Instance.MovedUnit)
            return;
        Instance.MovedUnit.GetComponent<ObjectMover>().SetCurrentTarget(target);
    }

    private static void ResetMovedUnit()
    {
        SetMovedUnit(null);
    }
}
