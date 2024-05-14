using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages health by receiving events from hurtboxes.
/// </summary>
public class HealthComponent : MonoBehaviour
{
    [Tooltip("Every hurtbox registered to this component.")]
    protected List<HurtBox> hurtBoxes = new List<HurtBox>();

    [SerializeField, Tooltip("Serializable state of health.")] 
    protected HealthData healthData = new HealthData();

    public delegate void OnHealthDepleted();
    [Tooltip("Called when health becomes at or below min health.")]
    public OnHealthDepleted onHealthDepleted;

    public delegate void OnTakeDamage(DamageInfo info);
    [Tooltip("Called anytime the health component receives damage.")]
    public OnTakeDamage onTakeDamage;

    public float CurrentHealth { get { return healthData.CurHealth; } }
    public float MaxHealth { get { return healthData.maxHealth; } }

    protected void OnEnable()
    {
        // Reset health
        healthData.CurHealth = healthData.maxHealth;
    }

    /// <summary>
    /// Add a hurtbox to the list of hurtboxes managed by this health component.
    /// </summary>
    /// <param name="hurtBox"></param>
    public void RegisterHurtBox(HurtBox hurtBox)
    {
        if (!hurtBoxes.Contains(hurtBox))
        {
            hurtBoxes.Add(hurtBox);
        }
    }

    /// <summary>
    /// Receive damage defined by damage info parameter.
    /// </summary>
    /// <param name="info"></param>
    public void TakeDamage(DamageInfo info)
    {
        healthData.CurHealth = Mathf.Clamp(healthData.CurHealth - info.value, healthData.minHealth, healthData.maxHealth);

        Debug.Log(name + " took " + info.value + " damage. Remaining health: " + healthData.CurHealth);

        onTakeDamage?.Invoke(info);

        if(healthData.CurHealth <= healthData.minHealth)
        {
            Debug.Log(name + " health was depleted.");

            onHealthDepleted?.Invoke();
        }
    }
}
