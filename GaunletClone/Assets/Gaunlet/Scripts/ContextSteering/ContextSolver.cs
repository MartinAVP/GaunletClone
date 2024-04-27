using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextSolver : MonoBehaviour
{
    Compass compass = new Compass();

    public Vector3 GetSolvedDirection(List<SteeringStyle> styles, SteeringData data)
    {
        float[] danger = new float[8];
        float[] interest = new float[8];

        foreach (SteeringStyle style in styles)
        {
            style.GetWeights(danger, interest, data);
        }

        int highestInterestInex = 0;
        float highestInterest = 0;
        for (int i = 0; i < Compass.Length; i++)
        {
            float value = (interest[i]) - danger[i];
            if (value > highestInterest)
            {
                highestInterest = value;
                highestInterestInex = i;
            }

            Debug.Log(name + " index " + i + " interest " + interest[i] + " danger " + danger[i] + " value " + value);
        }

        Vector3 finalDirection = compass[highestInterestInex];

        return finalDirection;
    }
}
