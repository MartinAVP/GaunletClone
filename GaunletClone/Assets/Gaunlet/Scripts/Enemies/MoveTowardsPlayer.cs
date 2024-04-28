using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class MoveTowardsPlayer : MonoBehaviour, IEnemyBehaviorInterface
{
    public float PlayerDetectionRadius
    {
        set
        {
            if (seeking == null)
                seeking = gameObject.AddComponent<TargetSeeking>();
            seeking.Radius = value;
        }
    }
    public float ObstacleDetectionRadius
    {
        set
        {
            if(avoidance == null)
                avoidance = gameObject.AddComponent<ObstacleAvoidance>();
            avoidance.Radius = value;
        }
    }

    public float sightDistance;
    public float attackRange;
    public float speed = 2;

    public List<SteeringContext> contexts = new List<SteeringContext>();

    private ObstacleAvoidance avoidance;
    private TargetSeeking seeking;
    private ContextSolver solver;

    private Rigidbody rb;

    protected Vector3 inputDirc;

    private void Awake()
    {
        if(seeking == null)
            seeking = gameObject.AddComponent<TargetSeeking>();
        if(avoidance == null)
            avoidance = gameObject.AddComponent<ObstacleAvoidance>();   
    
        
        contexts.Add(seeking);
        contexts.Add(avoidance);
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
        inputDirc = input;  

        Debug.DrawLine(transform.position, transform.position + input, Color.yellow, 1f);

        rb.velocity = input * speed;
        Debug.Log(name + " velocity " + rb.velocity);

        yield return new WaitForSeconds(.5f);
        onComplete?.Invoke();
    }

    private void OnCollisionStay(Collision collision)
    {
        // Slide along surface
        Vector3 posToTarget = (collision.GetContact(0).point - transform.position).normalized;
        posToTarget.y = transform.position.y;

        // Rotate input drc slightly to avoid a cross product with zero length
        Vector3 cross = Vector3.Cross(collision.GetContact(0).normal, Quaternion.AngleAxis(1, transform.up) * inputDirc).normalized;

        //Debug.DrawLine(transform.position, transform.position + cross, Color.blue, .1f);

        rb.velocity = Vector3.Cross(cross, collision.GetContact(0).normal).normalized * speed;

        //Debug.DrawLine(transform.position, transform.position + rb.velocity.normalized, Color.magenta, .1f);
    }
}
