using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
