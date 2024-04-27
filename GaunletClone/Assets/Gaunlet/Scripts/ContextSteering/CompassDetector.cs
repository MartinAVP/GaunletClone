using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CompassDetector : Detector
{
    private Compass compass = new Compass();

    public override SteeringData Detect(ref SteeringData data)
    {
        for (int i = 0; i < Compass.Length; i++)
        {
            Vector3 start = transform.position;
            Vector3 end = start + compass[i] * radius;
            RaycastHit hit;

            Debug.DrawLine(transform.position, transform.position + compass[i] * radius, Color.red, 1f);

            if (Physics.Linecast(start, end, out hit, layerMask))
            {
                data.obstacles.Add(hit.point);
            }
        }

        return data;
    }
}
