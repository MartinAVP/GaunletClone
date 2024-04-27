using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSeeking : SteeringContext
{
    public float radius = 20;

    protected Compass compass = new Compass();
    protected Detector sphereDetector;

    private void Awake()
    {
        sphereDetector = gameObject.AddComponent<SphereDetector>();
        sphereDetector.radius = radius;
        sphereDetector.layers = new string[1] { "Player" };
    }

    public override void GetWeights(ref SteeringData data)
    {
        sphereDetector.Detect(ref data);

        foreach (Vector3 target in data.targets)
        {
            Vector3 direction = (target - transform.position);
            float distance = direction.magnitude;

            float weight = 1 - Mathf.Clamp(distance / radius, 0, 1);

            direction = direction.normalized;
            for(int i = 0; i < Compass.Length; i++)
            {
                float value = Vector3.Dot(compass[i], direction) * weight;
                //value = (value + 1) / 2;

                if (value > data.interest[i])
                {
                    data.interest[i] = value;    
                }
            }
        }
    }
}
