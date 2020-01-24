using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedBattleUnit : BattleUnit
{
    public Transform LaunchPosition;
    public GameObject ballPrefab;
    private GameObject currentBall = null;

    public override void Attack(BattleUnit target)
    {
        CurrentTarget = target;
        FinishAttack();
    }

    public override void FinishAttack()
    {
        currentBall = Instantiate(ballPrefab, transform.position, transform.rotation);
        currentBall.transform.localScale = new Vector3(0.2f, 0.2f, 1);
        Debug.Log("Target: " + (CurrentTarget == null));
        currentBall.GetComponent<Ball>().Launch(CurrentTarget.gameObject);

        animator.SetBool("IsAttacking", false);

        currentBall.GetComponent<Ball>().OnTargetReached.AddListener(() => TargetReached());
    }

    public void TargetReached()
    {
        Destroy(this.currentBall);
        currentBall = null;

        CurrentTarget.ReceiveDamage(this, GetComponent<UnitAttributes>().GetAttributeValue(Attribute.Attack));
        OnAttackFinished.Invoke();
    }

}
