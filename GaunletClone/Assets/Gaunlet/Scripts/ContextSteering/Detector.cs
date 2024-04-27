using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Detector : MonoBehaviour
{
    public float radius = 20;
    public string[] layers
    {
        set
        {
            layerMask = LayerMask.GetMask(value);
        }
    }

    protected LayerMask layerMask;

    public abstract SteeringData Detect(ref SteeringData data);
}
