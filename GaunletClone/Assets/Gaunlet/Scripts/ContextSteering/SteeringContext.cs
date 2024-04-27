using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SteeringContext : MonoBehaviour
{
    /// <summary>
    /// Update obstacle and / or targets array + weights, depending on if this context seeks targets and / or obstacles.
    /// </summary>
    /// <param name="data">The steering data to update</param>
    public abstract void GetWeights(ref SteeringData data);
}