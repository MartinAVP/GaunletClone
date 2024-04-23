using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SteeringStyle : MonoBehaviour
{
    public abstract void GetWeights(float[] danger, float[] interest, SteeringData data);
}
