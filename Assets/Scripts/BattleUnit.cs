using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BattleUnit : MonoBehaviour
{
    public string unitName;

    protected Animator animator;
    private UnitAttributes Attributes;
    private ObjectMover Mover;
    public Vector2Int InitialGridPosition;
    public Tile CurrentTile = null;

    public UnitTeam team;

    protected BattleUnit CurrentTarget;

    public int MaxHealth { get; private set; }
    public int Health { get; private set; }

    [System.Serializable]
    public class DamageEvent : UnityEvent<BattleUnit, BattleUnit, int> { }; // this, dealer, amount
    public DamageEvent OnDamageTaken = new DamageEvent();

    public UnityEvent OnAttackFinished = new UnityEvent();

    private bool bBusy = false;


    private void Start()
    {
        animator = GetComponent<Animator>();
        Attributes = GetComponent<UnitAttributes>();
        Mover = GetComponent<ObjectMover>();

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
    }

    public void MoveToStartPosition()
    {
        Tile startingTile = GridController.GetGridController().GetTile(InitialGridPosition);
        GridController.GetGridController().MoveUnit(gameObject, startingTile);
    }

    public void Attack(BattleUnit target)
    {
        Debug.Log("Start attack");
        if(bBusy)
            return;
        Debug.Log("Not busy");
        bBusy = true;
        animator.SetBool("IsAttacking", true);
        Mover.FaceObject(target.gameObject);
        CurrentTarget = target;
    }

    public void Attack(GameObject target)
    {
        BattleUnit asBattleUnit = target.GetComponent<BattleUnit>();
        if(!asBattleUnit)
        {
            Debug.LogError("Missing battle unit componet");
            return;
        }
        Attack(asBattleUnit);
    }

    public virtual void FinishAttack()
    {
        bBusy = false;
        CurrentTarget.ReceiveDamage(this, Attributes.GetAttributeValue(Attribute.Attack));
        CurrentTarget = null;
        animator.SetBool("IsAttacking", false);
        OnAttackFinished.Invoke();
        // BattleScreenManager.SetCurrentState(BattleManagerState.ModePicking);
    }

    public void ReceiveDamage(BattleUnit damageDealer, int amount)
    {
        amount = Mathf.Clamp(amount - GetComponent<UnitAttributes>().GetAttributeValue(Attribute.Defence), 0, amount);
        Health -= amount;
        Health = Mathf.Clamp(Health, 0, MaxHealth);
        OnDamageTaken.Invoke(this, damageDealer, amount);
    }

    public BattleUnit ClosestUnit()
    {
        List<BattleUnit> units = new List<BattleUnit>(FindObjectsOfType<BattleUnit>());
        units.RemoveAll((BattleUnit b) => b.team == team || b == this);
        units.ForEach((BattleUnit b) => { if(b.CurrentTile == null) Debug.Log("Dupa: " + b.name + b.team); });
        units.Sort((BattleUnit a, BattleUnit b) => GridController.ManhattanDistance(a.CurrentTile, this.CurrentTile) - GridController.ManhattanDistance(b.CurrentTile, this.CurrentTile));
        return units[0];
    }

}