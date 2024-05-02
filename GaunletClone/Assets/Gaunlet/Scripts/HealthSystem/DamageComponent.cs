using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages hit boxes below self in the heirarchy.
/// </summary>
public class DamageComponent : MonoBehaviour
{
    [SerializeField, Tooltip("Damage info for hurt boxes below this in the heirarchy.")]
    protected DamageInfo info = new DamageInfo();
    [Tooltip("Damage info for hurt boxes below this in the heirarchy.")]
    public DamageInfo Info { get { return info; } }

    [Tooltip("All hitboxes registered to this damage component.")]
    protected List<HitBox> hitBoxes = new List<HitBox>();

    /// <summary>
    /// Add a hit box to the list of hit boxes managed by this component.
    /// </summary>
    /// <param name="hitBox"></param>
    public void RegisterHitBox(HitBox hitBox)
    {
        if (!hitBoxes.Contains(hitBox))
        {
            hitBoxes.Add(hitBox);
        }
    }
}
