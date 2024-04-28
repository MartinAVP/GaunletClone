using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAvoidance : SteeringContext
{
    public float radius = 3;

    private Compass compass = new Compass();
    private CompassDetector compassDetector;

    private void Awake()
    {
        compassDetector = gameObject.AddComponent<CompassDetector>();
        compassDetector.layers = new string[] { "Default" };
        compassDetector.radius = radius;
    }

    public override void GetWeights(ref SteeringData data)
    {
        //danger = interest = new float[8];

        //Debug.Log(name + " num obstacles " + data.obstacles.Count);

        compassDetector.Detect(ref data);

        foreach (Vector3 obstacle in data.obstacles)
        {
            Vector3 direction = (obstacle - transform.position);
            float distance = direction.magnitude;

            float weight = 1 - Mathf.Clamp(distance / radius, 0, 1);

            direction = direction.normalized;
            for (int i = 0; i < Compass.Length; i++)
            {
                float value = Vector3.Dot(compass[i], direction) * weight;

                if(value > data.danger[i])
                {
                    data.danger[i] = value;
                }
            }
        }
    }
}
