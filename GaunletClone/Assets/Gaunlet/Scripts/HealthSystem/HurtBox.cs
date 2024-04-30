using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HurtBox : MonoBehaviour
{
    protected HealthComponent healthComponent;

    protected void Awake()
    {
        if (!FindHealthComponent())
            Debug.Log(name + " failed to find a health component in the transform heirarchy.");
    }

    protected bool FindHealthComponent()
    {
        Transform root = transform;
        while (root != null)
        {
            healthComponent = GetComponent<HealthComponent>();
            if(healthComponent != null)
            {
                // register hurt box
                return true;
            }
            else 
                root = transform.parent;
        }

        return false;
    }

    public void TakeDamage(DamageInfo info)
    {
        if (healthComponent != null)
        {
            healthComponent.TakeDamage(info);
        }
    }
}
