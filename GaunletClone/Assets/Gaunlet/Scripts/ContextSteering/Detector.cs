using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract class for an object that collects data about the AI's environment or "context."
/// </summary>
public abstract class Detector : MonoBehaviour
{
    [Tooltip("Distance this detector will search within.")]
    public float radius = 20;
    [Tooltip("Collision layers this detector should search within.")]
    public string[] layers
    {
        set
        {
            layerMask = LayerMask.GetMask(value);
        }
    }

    protected LayerMask layerMask;

    /// <summary>
    /// Collect data from the environment and store it in values.
    /// </summary>
    /// <param name="values"></param>
    public abstract void Detect(ref List<Vector3> values);
}
