using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BattleUnit : MonoBehaviour
{
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

    private bool bBusy = false;

    private void Start()
    {
        MaxHealth = gameObject.GetComponent<UnitAttributes>().GetAttributeValue(Attribute.MaxHealth);
        Health = MaxHealth;

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
        Tile startingTile = BattleScreenManager.GetGridController().GetTile(InitialGridPosition);
        CurrentTile = startingTile;
        // this shouldn't be here
        CurrentTile.CurrentUnit = gameObject;
        GetComponent<ObjectMover>().SetCurrentTarget(CurrentTile.UnitPosition);
    }

    public void FinishAttack()
    {
        bBusy = false;
        CurrentTarget.ReceiveDamage(this, Attributes.GetAttributeValue(Attribute.Attack));
        animator.SetBool("IsAttacking", false);
        BattleScreenManager.SetMovedUnit(null);
        // BattleScreenManager.SetCurrentState(BattleManagerState.ModePicking);
    }

    public void ReceiveDamage(BattleUnit damageDealer, int amount)
    {
        Health -= amount;
        Health = Mathf.Clamp(Health, 0, MaxHealth);
        OnDamageTaken.Invoke(this, damageDealer, amount);
    }

}