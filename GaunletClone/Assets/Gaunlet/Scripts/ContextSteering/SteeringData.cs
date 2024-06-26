using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Data required to resolve a direction within the context steering system.
/// </summary>
public class SteeringData
{
    public List<Vector3> targets = new List<Vector3>();
    public List<float> interest = new List<float>();
    public List<Vector3> obstacles = new List<Vector3>();
    public List<float> danger = new List<float>();
    public Vector3 target = Vector3.zero;
}
