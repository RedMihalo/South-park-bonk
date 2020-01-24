using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedBattleUnit : BattleUnit
{
    public Transform LaunchPosition;
    public GameObject ballPrefab;
    private GameObject currentBall = null;

    public override void FinishAttack()
    {
        currentBall = Instantiate(ballPrefab, transform.position, transform.rotation);
        currentBall.GetComponent<Ball>().Launch(CurrentTarget.gameObject);

        animator.SetBool("IsAttacking", false);

        currentBall.GetComponent<Ball>().OnTargetReached.AddListener(() => TargetReached());
    }

    private void Update()
    {
        Debug.Log(animator.GetBool("IsAttacking"));
    }

    public void TargetReached()
    {
        Destroy(this.currentBall);
        currentBall = null;

        CurrentTarget.ReceiveDamage(this, GetComponent<UnitAttributes>().GetAttributeValue(Attribute.Attack));
        OnAttackFinished.Invoke();
    }

}
