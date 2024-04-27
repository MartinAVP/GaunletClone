using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAvoidance : SteeringContext
{
    public float radius = 6;

    private Compass compass = new Compass();

    public override void GetWeights(ref SteeringData data)
    {
        //danger = interest = new float[8];

        //Debug.Log(name + " num obstacles " + data.obstacles.Count);

        foreach (Vector3 obstacle in data.obstacles)
        {

            Vector3 direction = (obstacle - transform.position);
            float distance = direction.magnitude;

            float weight = 1 - Mathf.Clamp(distance / radius, 0, 1);

            direction = direction.normalized;
            for (int i = 0; i < Compass.Length; i++)
            {
                float value = Vector3.Dot(compass[i], direction) * weight;
                //value = (value + 1) / 2;

                //Debug.Log(name + "Dot Prod. " + value / weight + ", Weight. " + weight + ", value. " + value);

                if(value > data.danger[i])
                {
                    data.danger[i] = value;
                }
            }
        }
    }
}
