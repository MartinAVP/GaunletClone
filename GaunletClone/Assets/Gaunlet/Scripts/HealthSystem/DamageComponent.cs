using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageComponent : MonoBehaviour
{
    protected DamageInfo info = new DamageInfo();
    public DamageInfo Info { get { return info; } }

    protected List<HitBox> hitBoxes = new List<HitBox>();

    public void RegisterHitBox(HitBox hitBox)
    {
        if (!hitBoxes.Contains(hitBox))
        {
            hitBoxes.Add(hitBox);
        }
    }
}
