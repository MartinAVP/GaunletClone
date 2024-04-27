using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDetector : Detector
{
    public float range = 6;

    private string[] targetLayers = { "Player" };
    private LayerMask layerMask;

    private void Start()
    {
        layerMask = LayerMask.GetMask(targetLayers);
    }

    public override SteeringData Detect(SteeringData data)
    {
        Vector3 start = transform.position;

        Collider[] colliders = Physics.OverlapSphere(start, range, layerMask);

        Debug.Log(name + " detecting...");

        foreach(Collider collider in colliders)
        {
            Debug.Log(name + " detected " + collider.gameObject.name);
            data.targets.Add(collider.transform.position);
        }


        return data;
    }

    private void OnDrawGizmos()
    {
        Vector3 start = transform.position;

        Gizmos.DrawWireSphere(start, range);
    }
}
