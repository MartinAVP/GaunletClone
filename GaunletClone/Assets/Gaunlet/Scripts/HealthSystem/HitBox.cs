using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sends damage events when overlapping another hurtbox.
/// Must be paired with a damage component at or above this in the heirarchy.
/// </summary>
public class HitBox : MonoBehaviour
{
    protected DamageComponent damageComponent;

    protected void Awake()
    {
        if (!FindDamageComponent())
        {
            Debug.LogWarning(name + " hit box could not find damage component in heirarchy.");
        }
    }

    /// <summary>
    /// Find the first damage component above this hit box in the heirarchy.
    /// </summary>
    /// <returns>True if damage component was found.</returns>
    protected bool FindDamageComponent()
    {
        Transform root = transform;
        while (root != null)
        {
            damageComponent = root.GetComponent<DamageComponent>();
            if (damageComponent != null)
            {
                damageComponent.RegisterHitBox(this);
                return true;
            }
            root = root.parent;
        }

        return false;
    }

    protected void OnTriggerEnter(Collider other)
    {
        HurtBox hurtBox = other.GetComponent<HurtBox>();
        if (hurtBox != null)
        {
            DealDamage(hurtBox);
        }
    }

    /// <summary>
    /// Invoke take damage on provided hurtbox, using this hit box's damage component data.
    /// </summary>
    /// <param name="hurtBox"></param>
    protected void DealDamage(HurtBox hurtBox)
    {
        if(damageComponent != null)
        {
            hurtBox.TakeDamage(damageComponent.Info);
        }
    }
}
