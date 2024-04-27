using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereDetector : Detector
{
    public override SteeringData Detect(ref SteeringData data)
    {
        Vector3 start = transform.position;

        Collider[] colliders = Physics.OverlapSphere(start, radius, layerMask);

        //Debug.Log(name + " detecting...");

        foreach (Collider collider in colliders)
        {
            //Debug.Log(name + " detected " + collider.gameObject.name);
            data.targets.Add(collider.transform.position);
        }


        return data;
    }

    private void OnDrawGizmos()
    {
        Vector3 start = transform.position;

        Gizmos.DrawWireSphere(start, radius);
    }
}
