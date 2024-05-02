using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    protected List<HurtBox> hurtBoxes = new List<HurtBox>();

    [SerializeField] protected HealthData healthData = new HealthData();

    public delegate void OnHealthDepleted();
    public OnHealthDepleted onHealthDepleted;

    public delegate void OnTakeDamage();
    public OnTakeDamage onTakeDamage;

    protected void OnEnable()
    {
        healthData.CurHealth = healthData.maxHealth;
    }

    public void RegisterHurtBox(HurtBox hurtBox)
    {
        if (!hurtBoxes.Contains(hurtBox))
        {
            hurtBoxes.Add(hurtBox);
        }
    }

    public void TakeDamage(DamageInfo info)
    {
        healthData.CurHealth = Mathf.Clamp(healthData.CurHealth - info.value, healthData.minHealth, healthData.maxHealth);

        Debug.Log(name + " took " + info.value + " damage. Remaining health: " + healthData.CurHealth);

        onTakeDamage?.Invoke();

        if(healthData.CurHealth <= healthData.minHealth)
        {
            Debug.Log(name + " health was depleted.");

            onHealthDepleted?.Invoke();
        }
    }
}
