using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class MoveTowardsPlayer : MonoBehaviour, IEnemyBehaviorInterface
{
    public float sightDistance;
    public float attackRange;
    public float speed = 2;

    public List<SteeringContext> contexts = new List<SteeringContext>();

    private ContextSolver solver;

    private Rigidbody rb;

    private void Awake()
    {
        contexts.Add(gameObject.AddComponent<ObstacleAvoidance>());
        contexts.Add(gameObject.AddComponent<TargetSeeking>());
        solver = gameObject.AddComponent<ContextSolver>();

        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezePositionY;
        rb.freezeRotation = true;
    }

    public void Execute(IEnemyInterface enemy, UnityAction onComplete)
    {
        StartCoroutine(GetInput(onComplete));   
    }

    private IEnumerator GetInput(UnityAction onComplete)
    {
        Vector3 input = solver.GetDirection(contexts);

        Debug.DrawLine(transform.position, transform.position + input, Color.yellow, 1f);

        rb.velocity = input * speed;
        Debug.Log(name + " velocity " + rb.velocity);

        yield return new WaitForSeconds(.5f);
        onComplete?.Invoke();
    }

    private void OnCollisionStay(Collision collision)
    {
        Vector3 cross = Vector3.Cross(collision.relativeVelocity, collision.GetContact(0).normal).normalized;
        rb.velocity = Vector3.Cross(cross, collision.GetContact(0).normal).normalized * speed;

        Debug.DrawLine(transform.position, transform.position + rb.velocity.normalized, Color.blue, .1f);
    }
}
