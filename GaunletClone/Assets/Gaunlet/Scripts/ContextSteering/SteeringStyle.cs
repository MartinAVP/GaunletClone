using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SteeringStyle : MonoBehaviour
{
    public abstract void GetWeights(out float[] danger, out float[] interest, SteeringData data);
}
