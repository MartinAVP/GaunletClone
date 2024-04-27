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

            direction = direction.normalized;
            for(int i = 0; i < Compass.Length; i++)
            {
                // The further compass direction can cast without hitting the environment, the more strongly we should weight that direction.
                // In this case the the max distance of a cast should be the distance to target.
                RaycastHit hit;
                string[] layers = { "Default" };
                float visibility = 1;
                if (Physics.Linecast(transform.position, transform.position + compass[i] * distance, out hit, LayerMask.GetMask(layers)))
                {
                    visibility = Mathf.Clamp(hit.distance / distance, 0, 1);
                }

                float value = visibility + Vector3.Dot(compass[i], direction);

                if (value > data.interest[i])
                {
                    data.interest[i] = value;
                    data.target = target;
                }
            }
        }
    }
}
