using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public enum BattleManagerState
{
    CharacterPicking,
    TilePicking,
    TargetPicking
}

public class BattleScreenManager : MonoBehaviour
{
    private static BattleScreenManager Instance;
    public GameObject MovedUnit;
    private UnityEngine.Events.UnityAction ResetMovedUnitCallback = () => BattleScreenManager.ResetMovedUnit();

    public GridController GridController;

    public Image BattleShadeImage;
    private BattleManagerState CurrentState = BattleManagerState.CharacterPicking;

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
        GridController.SetGridEnabled(false);
        SetCharPickersEnabled(true);
        // Instance.BattleShadeImage.gameObject.SetActive(true);
    }

    public static void SetMovedUnit(GameObject NewMovedUnit)
    {
        // GetGridController().SetGridEnabled((bool)NewMovedUnit);
        SetCharPickersEnabled(!(bool)NewMovedUnit);
        Instance.BattleShadeImage.transform.root.gameObject.SetActive(!(bool)NewMovedUnit);

        if(!NewMovedUnit)
        {
            GetGridController().SetGridEnabled(false);
            if(Instance.MovedUnit)
                Instance.MovedUnit.GetComponent<ObjectMover>().OnDestinationReached.RemoveListener(Instance.ResetMovedUnitCallback);

            Instance.CurrentState = BattleManagerState.CharacterPicking;
            return;
        }

        UnitAttributes attributes = NewMovedUnit.GetComponent<UnitAttributes>();
        int? range = attributes.GetAttributeValue(Attribute.Range);
        GetGridController().EnableValidTiles((Tile t) => {
            return 
            t.CurrentUnit == null &&
            (
                NewMovedUnit.GetComponent<ObjectMover>().CurrentTile == null ||
                GridController.ManhattanDistance(t.PositionInGrid, NewMovedUnit.GetComponent<ObjectMover>().CurrentTile.PositionInGrid) <=
                    NewMovedUnit.GetComponent<UnitAttributes>().GetAttributeValue(Attribute.Range)
            );
        });

        Instance.CurrentState = BattleManagerState.TilePicking;
        Instance.MovedUnit = NewMovedUnit;
        Instance.MovedUnit.GetComponent<ObjectMover>().OnDestinationReached.AddListener(Instance.ResetMovedUnitCallback);
    }

    private static void SetCharPickersEnabled(bool bEnabled)
    {
        foreach(var Picker in FindObjectsOfType<CharacterPicker>())
        {
            Picker.PickerEnabled = bEnabled;
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
