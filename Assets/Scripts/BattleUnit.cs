using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BattleUnit : MonoBehaviour
{
    public string unitName;

    public Animator animator;
    public UnitAttributes Attributes;
    public ObjectMover Mover;
    public Vector2Int InitialGridPosition;
    public Tile CurrentTile = null;

    public UnitTeam team;

    private BattleUnit CurrentTarget;

    public int MaxHealth { get; private set; }
    public int Health { get; private set; }

    [System.Serializable]
    public class DamageEvent : UnityEvent<BattleUnit, BattleUnit, int> { }; // this, dealer, amount
    public DamageEvent OnDamageTaken = new DamageEvent();

    public UnityEvent OnAttackFinished = new UnityEvent();

    private bool bBusy = false;


    private void Start()
    {
        MaxHealth = gameObject.GetComponent<UnitAttributes>().GetAttributeValue(Attribute.MaxHealth);
        Health = MaxHealth;

    }

    public List<GameObject> GetUnitsInRange()
    {
        List<GameObject> units = new List<GameObject>();
        GridController.GetGridController().GetTiles().FindAll(
            (Tile t) => t.CurrentUnit != null 
            && t.CurrentUnit.GetComponent<BattleUnit>().team != team
            && GridController.ManhattanDistance(t, CurrentTile) <= GetComponent<UnitAttributes>().GetAttributeValue(Attribute.AttackRange)
            ).ForEach((Tile t) => units.Add(t.CurrentUnit));
        return units;
    }

    public bool HasUnitsInAttackRange()
    {
        return GetUnitsInRange().Count > 0;
        return GridController.GetGridController().GetTiles()
            .FindAll((Tile t) => GridController.ManhattanDistance(CurrentTile, t) <= GetComponent<UnitAttributes>().GetAttributeValue(Attribute.AttackRange))
            .FindAll((Tile t) => t.CurrentUnit != null)
            .FindAll((Tile t) => t.CurrentUnit.GetComponent<BattleUnit>().team != team)
            .Count > 0;
    }

    public void Attack(BattleUnit target)
    {
        if(bBusy)
            return;
        bBusy = true;
        animator.SetBool("IsAttacking", true);
        CurrentTarget = target;
    }

    public void Attack(GameObject target)
    {
        BattleUnit asBattleUnit = target.GetComponent<BattleUnit>();
        if(!asBattleUnit)
        {
            Debug.Log("Missing battle unit componet");
            return;
        }
        Mover.FaceObject(target);
        Attack(asBattleUnit);
    }

    public void MoveToStartPosition()
    {
        Tile startingTile = GridController.GetGridController().GetTile(InitialGridPosition);
        GridController.GetGridController().MoveUnit(gameObject, startingTile);
    }

    public void FinishAttack()
    {
        bBusy = false;
        CurrentTarget.ReceiveDamage(this, Attributes.GetAttributeValue(Attribute.Attack));
        animator.SetBool("IsAttacking", false);
        OnAttackFinished.Invoke();
        // BattleScreenManager.SetCurrentState(BattleManagerState.ModePicking);
    }

    public void ReceiveDamage(BattleUnit damageDealer, int amount)
    {
        Health -= amount;
        Health = Mathf.Clamp(Health, 0, MaxHealth);
        OnDamageTaken.Invoke(this, damageDealer, amount);
    }

    public BattleUnit ClosestUnit()
    {
        List<BattleUnit> units = new List<BattleUnit>(FindObjectsOfType<BattleUnit>());
        units.RemoveAll((BattleUnit b) => b.team == team || b == this);
        units.Sort((BattleUnit a, BattleUnit b) => GridController.ManhattanDistance(a.CurrentTile, this.CurrentTile) - GridController.ManhattanDistance(b.CurrentTile, this.CurrentTile));
        return units[0];
    }

}