using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : Controller
{
    public Button MoveButton;
    public Button AttackButton;
    public Button CancelButton;
    ControllerState state;


    public override void Start()
    {
        MoveButton.onClick.AddListener(() => SetState(ControllerState.Moving));
        AttackButton.onClick.AddListener(() => SetState(ControllerState.Atttacking));
        CancelButton.onClick.AddListener(() => SetState(ControllerState.ModePicking));

        MoveOrder = 0;
        team = UnitTeam.Player;
        base.Start();

        foreach(GameObject u in Units)
        {
            CharacterPicker picker = u.GetComponent<CharacterPicker>();
            picker.OnCharacterPicked.AddListener((CharacterPicker p) => this.PickUnit(p.gameObject));
        }

        GridController.GetGridController().GetTiles().ForEach(
            (Tile t) => t.OnClicked.AddListener((Tile tt) => PickTile(tt))
            );

        // SetState(ControllerState.ModePicking);
    }

    private void SetState(ControllerState s)
    {
        this.state = s;
        SetCharactersPickerEnabled(s == ControllerState.Atttacking || s == ControllerState.Moving);
        UpdateDisplayedButtons();
    }

    private void HighlightTiles()
    {
        switch(state)
        {
            case ControllerState.Atttacking:
                GridController.GetGridController().EnableValidTiles((Tile t) => { return UnitInAttackRange(t, CurrentUnit)
                    && t.CurrentUnit.GetComponent<BattleUnit>().team != CurrentUnit.GetComponent<BattleUnit>().team; });
                break;
            case ControllerState.Moving:
                GridController.GetGridController().EnableValidTiles((Tile t) => { return TileInMoveRange(t, CurrentUnit); });
                break;
            case ControllerState.ModePicking:
                GridController.GetGridController().SetGridEnabled(false);
                break;
        }
        UpdateDisplayedButtons();
    }

    private void UpdateDisplayedButtons()
    {
        MoveButton.gameObject.SetActive(state == ControllerState.ModePicking);
        AttackButton.gameObject.SetActive(state == ControllerState.ModePicking);
        CancelButton.gameObject.SetActive(state != ControllerState.ModePicking);
    }

    private void DisableButtons()
    {
        MoveButton.gameObject.SetActive(false);
        AttackButton.gameObject.SetActive(false);
        CancelButton.gameObject.SetActive(false);
    }

    public override void ReceiveControl()
    {
        SetState(ControllerState.ModePicking);
    }

    protected override void PickUnit(GameObject unit)
    {
        base.PickUnit(unit);
        SetCharactersPickerEnabled(false);
        HighlightTiles();
    }

    private void PickTile(Tile t)
    {
        switch(state)
        {
            case ControllerState.Atttacking:
                Attack(t.CurrentUnit);
                break;
            case ControllerState.Moving:
                MoveUnit(t);
                break;
        }
    }

    private void SetCharactersPickerEnabled(bool bEnabled)
    {
        foreach(GameObject u in Units)
            u.GetComponent<CharacterPicker>().PickerEnabled = bEnabled;
    }

    public override void PassControl()
    {
        DisableButtons();
        GridController.GetGridController().SetGridEnabled(false);
        base.PassControl();
    }

    protected override List<UnitSerializeInfo> GetUnitSerializeInfos()
    {
        List<UnitSerializeInfo> SerializedUnits = new List<UnitSerializeInfo>();
        if(!PlayerPrefs.HasKey("ChosenUnits"))
        {
            Debug.Log("nope ");
            UnitSerializeInfo unitInfo = new UnitSerializeInfo
            {
                prefab = stanPrefab,
                team = UnitTeam.Player
            };
            unitInfo.positionInGrid.x = 0;
            unitInfo.positionInGrid.y = 0;
            SerializedUnits.Add(unitInfo);
        }
        else
        {

            ChosenUnits chosenUnits = JsonUtility.FromJson<ChosenUnits>(PlayerPrefs.GetString("ChosenUnits"));
            Debug.Log("figth");
            Debug.Log(PlayerPrefs.GetString("ChosenUnits"));
            Debug.Log(chosenUnits.units);
            for(int i = 0; i < chosenUnits.units.Count; i++)
            {
                GameObject nextPrefab = GetUnitPrefab(chosenUnits.units[i]);
                if(nextPrefab == null)
                {
                    Debug.LogError("Prefab not defined for : " + chosenUnits.units[i]);
                    continue;
                }
                UnitSerializeInfo unitInfo = new UnitSerializeInfo
                {
                    prefab = nextPrefab,
                    team = team
                };
                unitInfo.positionInGrid.x = spawnColumn;
                unitInfo.positionInGrid.y = i;
                SerializedUnits.Add(unitInfo);
            }
        }

        return SerializedUnits;
    }


    private GameObject GetUnitPrefab(string name)
    {
        return unitMapping.mapping.Find((KeyToUnitPrefabMapping m) => m.name == name)?.prefab;
    }
}
