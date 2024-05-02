using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Serializable info to be managed by a health component.
/// </summary>
[System.Serializable]
public struct HealthData
{
    public float minHealth;
    public float maxHealth;
    private float curHealth;
    public float CurHealth
    {
        get { return curHealth; }
        set { curHealth = value; }
    }
}
