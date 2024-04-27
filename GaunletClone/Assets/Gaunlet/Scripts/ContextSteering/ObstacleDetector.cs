using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ObstacleDetector : Detector
{
    private Compass compass = new Compass();

    public float range = 6;

    private string[] obstacleLayers = { "Default" };
    private LayerMask layerMask;

    private void Start()
    {
        layerMask = LayerMask.GetMask(obstacleLayers);
    }

    public override SteeringData Detect(ref SteeringData data)
    {
        for(int i = 0; i < Compass.Length; i++)
        {
            Vector3 start = transform.position;
            Vector3 end = start + compass[i] * range;
            RaycastHit hit;

            Debug.DrawLine(transform.position, transform.position + compass[i] * range, Color.red, .1f);

            if(Physics.Linecast(start, end, out hit, layerMask))
            {
                data.obstacles.Add(hit.point);
            }
        }

        return data;
    }
}
