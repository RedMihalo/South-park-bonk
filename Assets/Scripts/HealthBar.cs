using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image Bar;
    public BattleUnit Unit;

    private void Start()
    {
        Unit.OnDamageTaken.AddListener((BattleUnit target, BattleUnit dealer, int amount) => {
            UpdatePercentage((float)(target.Health) / (float)(target.MaxHealth));
        });
    }

    public void UpdatePercentage(float percentage)
    {
        Bar.fillAmount = percentage;
    }
}
