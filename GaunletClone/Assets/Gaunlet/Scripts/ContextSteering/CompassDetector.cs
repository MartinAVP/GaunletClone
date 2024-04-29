using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Detector for directions stored in the Compass type.
/// </summary>
public class CompassDetector : Detector
{
    private Compass compass = new Compass();

    /// <summary>
    /// Perform a raycast originating from this root for each direction in Compass.
    /// Stores the first hit for each raycast in values.
    /// </summary>
    /// <param name="values"></param>
    public override void Detect(ref List<Vector3> values)
    {
        for (int i = 0; i < Compass.Length; i++)
        {
            Vector3 start = transform.position;
            Vector3 end = start + compass[i] * radius;
            RaycastHit hit;

            Debug.DrawLine(transform.position, transform.position + compass[i] * radius, Color.red, 1f);

            if (Physics.Linecast(start, end, out hit, layerMask))
            {
                values.Add(hit.point);
            }
        }
    }
}
