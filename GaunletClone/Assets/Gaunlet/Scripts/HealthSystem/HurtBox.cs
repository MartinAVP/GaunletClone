using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Attach to colliders that can receive damage.
/// Must be paried with a health component at or above this in the heriarchy.
/// </summary>
public class HurtBox : MonoBehaviour
{
    protected HealthComponent healthComponent;

    protected void Awake()
    {
        if (!FindHealthComponent())
            Debug.LogWarning(name + " failed to find a health component in the transform heirarchy.");
    }

    /// <summary>
    /// Find and register to the first health component above this hit box in the heirarchy.
    /// </summary>
    /// <returns>True if health component was found.</returns>
    protected bool FindHealthComponent()
    {
        Transform root = transform;
        while (root != null)
        {
            Debug.Log(name + " checking " + root.name + " for health component.");

            healthComponent = root.GetComponent<HealthComponent>();
            if(healthComponent != null)
            {
                healthComponent.RegisterHurtBox(this);
                return true;
            }

            root = root.parent;
        }

        return false;
    }

    /// <summary>
    /// Damage the health component associated with this hurtbox.
    /// </summary>
    /// <param name="info"></param>
    public void TakeDamage(DamageInfo info)
    {
        if (healthComponent != null)
        {
            healthComponent.TakeDamage(info);
        }
    }
}
