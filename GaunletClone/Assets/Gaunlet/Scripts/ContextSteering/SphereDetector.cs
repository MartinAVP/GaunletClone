using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Detector for objects within a sphere around this root.
/// </summary>
public class SphereDetector : Detector
{
    /// <summary>
    /// Detect the root location of colliders within radius.
    /// </summary>
    /// <param name="values"></param>
    public override void Detect(ref List<Vector3> values)
    {
        Vector3 start = transform.position;

        Collider[] colliders = Physics.OverlapSphere(start, radius, layerMask);

        //Debug.Log(name + " detecting...");

        foreach (Collider collider in colliders)
        {
            //Debug.Log(name + " detected " + collider.gameObject.name);
            values.Add(collider.transform.position);
        }
    }

    /* TESTONLY */

    private void OnDrawGizmos()
    {
        Vector3 start = transform.position;

        Gizmos.DrawWireSphere(start, radius);
    }

    /* ENDTEST */
}
