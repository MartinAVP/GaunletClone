using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Steering context for avoiding obstacles, detected using a compass.
/// </summary>
public class ObstacleAvoidance : SteeringContext
{
    [Tooltip("Distance this should search along each compass direction.")]
    protected float radius = 3;
    [Tooltip("Distance this should search along each compass direction.")]
    public float Radius
    {
        set
        {
            // To keep the detector's radius up-to-date,create a detector if we don't have one already.
            if(compassDetector == null)
                compassDetector = gameObject.AddComponent<CompassDetector>();
            radius = value;
            compassDetector.radius = radius;
        }
    }

    private Compass compass = new Compass();
    private CompassDetector compassDetector;

    private void Awake()
    {
        // Create a detector if we don't have one already. Set default values including search layer for obstacles.
        if(compassDetector == null)
            compassDetector = gameObject.AddComponent<CompassDetector>();
        compassDetector.layers = new string[] { "Default" };
        compassDetector.radius = radius;
    }

    /// <summary>
    /// Fill out obstacles and add weights to steering data's danger list.
    /// Does not reset data's previous obstacles or danger weights; manually reset before calling if this is the expected behavior.
    /// </summary>
    /// <param name="data"></param>
    public override void GetWeights(ref SteeringData data)
    {
        // Gather context from environment.
        compassDetector.Detect(ref data.obstacles);

        // Weigh each obstacle.
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
