using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BattleUnit : MonoBehaviour
{
    public Animator animator;
    public UnitAttributes Attributes;

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

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            Attack(this);
    }

    public void Attack(BattleUnit target)
    {
        if(bBusy)
            return;
        bBusy = true;
        animator.SetBool("IsAttacking", true);
        CurrentTarget = target;
    }

    public void FinishAttack()
    {
        bBusy = false;
        CurrentTarget.GetDamage(this, Attributes.GetAttributeValue(Attribute.Attack));
        animator.SetBool("IsAttacking", false);
    }

    public void GetDamage(BattleUnit damageDealer, int amount)
    {
        Health -= amount;
        Health = Mathf.Clamp(Health, 0, MaxHealth);
        OnDamageTaken.Invoke(this, damageDealer, amount);
    }

}