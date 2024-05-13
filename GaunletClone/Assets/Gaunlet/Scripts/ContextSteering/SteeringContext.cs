using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstarct class for a steering context.
/// Steering contexts will take a steering data structure and fill it out
/// based on what type of movement the context desires.
/// The actual final direction is a blend of each context's weights, which is calculated by a context solver.
/// </summary>
public abstract class SteeringContext : MonoBehaviour
{
    /// <summary>
    /// Update obstacle and / or targets array + weights, depending on if this context seeks targets and / or obstacles.
    /// </summary>
    /// <param name="data">The steering data to update</param>
    public abstract void GetWeights(ref SteeringData data);
}