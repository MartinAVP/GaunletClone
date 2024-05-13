using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType
{
    Body,
    Weapon,
    Projectile
}

/// <summary>
/// Information about damage being given to a hurtbox or health component.
/// </summary>
[System.Serializable]
public struct DamageInfo
{
    public DamageType type;
    public float value;
}
