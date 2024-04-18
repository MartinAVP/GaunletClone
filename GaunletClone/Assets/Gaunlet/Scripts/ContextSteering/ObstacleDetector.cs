using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ObstacleDetector : Detector
{
    private Vector3[] compass = { new Vector3(1,0,0), new Vector3(.7f,0,.7f), new Vector3(0,0,1), new Vector3(-.7f,0,.7f), 
        new Vector3(-1,0,0), new Vector3(-.7f,0,-.7f), new Vector3(0,0,-1), new Vector3(.7f,0,-.7f) };

    public float range = 2;
    public LayerMask obstaclesLayerMask;

    public override void Detect(SteeringData data)
    {
        foreach (Vector3 direction in compass)
        {
            Vector3 start = transform.position;
            Vector3 end = start + direction * range;
            RaycastHit hit;

            Debug.Log(name + " direction " + direction);
            Debug.DrawLine(transform.position, transform.position + direction * range, Color.red, .1f);

            if(Physics.Raycast(start, end, out hit))
            {
                data.obstacles.Add(hit.point);
            }
        }
    }
}
