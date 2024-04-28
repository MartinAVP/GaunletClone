using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextSolver : MonoBehaviour
{
    public Vector3 Target { get { return data.target; } }    

    protected Compass compass = new Compass();
    protected SteeringData data = new SteeringData();

    private void Awake()
    {
        for(int i = 0; i < Compass.Length; i++)
        {
            data.interest.Add(0);
            data.danger.Add(0);
        }
    }

    public Vector3 GetDirection(List<SteeringContext> contexts)
    {
        data.targets.Clear();
        data.obstacles.Clear();
        for (int i = 0; i < data.interest.Count; i++) data.interest[i] = data.danger[i] = 0;

        foreach (var context in contexts)
        {
            context.GetWeights(ref data);
        }

        // Get final direction from weighted compass
        int highestInterestInex = 0;
        float highestInterest = float.NegativeInfinity;
        for (int i = 0; i < Compass.Length; i++)
        {
            float value = data.interest[i] - data.danger[i];

            if (value > highestInterest)
            {
                highestInterest = value;
                highestInterestInex = i;
            }

            Debug.Log(name + " index " + i + " interest " + data.interest[i] + " danger " + data.danger[i] + " value " + value);
        }

        Vector3 finalDirection = compass[highestInterestInex];

        return finalDirection;
    }
}
