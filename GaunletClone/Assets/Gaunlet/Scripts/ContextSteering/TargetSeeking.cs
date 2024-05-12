using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Steering context for chasing targets, detected within the player layer using a sphere detector.
/// </summary>
public class TargetSeeking : SteeringContext
{
    [Tooltip("Half size of the sphere to search within.")]
    protected float radius = 20;
    [Tooltip("Half size of the sphere to search within.")]
    public float Radius
    {
        set
        {
            // To keep the detector's radius up-to-date,create a detector if we don't have one already.
            if (sphereDetector == null)
                sphereDetector = gameObject.AddComponent<SphereDetector>();
            sphereDetector.radius = value;
            radius = value;
        }
    }

    protected Compass compass = new Compass();
    protected Detector sphereDetector;

    private void Awake()
    {
        // Create a detector if we don't have one already. Set default values including search layer for targets.
        if (sphereDetector == null)
            sphereDetector = gameObject.AddComponent<SphereDetector>();
        sphereDetector.radius = radius;
        sphereDetector.layers = new string[1] { "Player" };
    }

    /// <summary>
    /// Fill out targets, target, and interest weights within steering data.
    /// Does not reset the data's targets, target, or interest lists. Manually reset these before calling if this is the expected behavior.
    /// </summary>
    /// <param name="data"></param>
    public override void GetWeights(ref SteeringData data)
    {
        // Gather context from environment.
        sphereDetector.Detect(ref data.targets);

        // Pick closest target
        float distance = float.MaxValue;
        Vector3 direction;
        foreach (Vector3 target in data.targets) 
        {
            direction = (target - transform.position);

            if(direction.magnitude < distance)
            {
                distance = direction.magnitude;
                data.target = target;
            }
        }

        // Weigh the target
        direction = (data.target - transform.position);
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
            }
        }
    }
}
