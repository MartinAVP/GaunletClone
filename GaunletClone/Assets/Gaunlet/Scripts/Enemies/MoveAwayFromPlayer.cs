using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Windows;

/// <summary>
/// Behavior for fleeing the player while trying to avoid some obstacles.
/// </summary>
public class MoveAwayFromPlayer : MonoBehaviour, IEnemyBehaviorInterface
{
    [Tooltip("How far away this behavior will look for players.")]
    public float PlayerDetectionRadius
    {
        set
        {
            // To keep contexts in sync, creat seeking context if we don't have one.
            if (evasion == null)
                evasion = gameObject.AddComponent<TargetEvasion>();
            evasion.Radius = value;
            sightDistance = value;
        }
    }
    [Tooltip("How far away this behvior will look for obstacles.")]
    public float ObstacleDetectionRadius
    {
        set
        {
            // To keep context in sync, create avoidance context if we don't have one.
            if (avoidance == null)
                avoidance = gameObject.AddComponent<ObstacleAvoidance>();
            avoidance.Radius = value;
        }
    }

    public Vector3 Target
    {
        get
        {
            return evasion.GetMostDesiredTarget();
        }
    }

    private float sightDistance;
    private float attackRange;
    private float speed = 2;

    private List<SteeringContext> contexts = new List<SteeringContext>();

    private ObstacleAvoidance avoidance;
    private TargetEvasion evasion;
    private ContextSolver solver;

    [Tooltip("The most recent input applied, as it was calcuated by the solver.")]
    protected Vector3 rawInput;

    [Tooltip("Enemy this behavior is controlling.")]
    private IEnemyInterface enemy;

    private bool fixDirection = false;

    private void Awake()
    {
        // Set up steering contexts if we haven't already
        if (evasion == null)
            evasion = gameObject.AddComponent<TargetEvasion>();
        if (avoidance == null)
            avoidance = gameObject.AddComponent<ObstacleAvoidance>();

        // Add steering contexts to the context list.
        contexts.Add(evasion);
        contexts.Add(avoidance);

        // Creat a solver.
        solver = gameObject.AddComponent<ContextSolver>();
    }

    public void Execute(IEnemyInterface enemy, UnityAction onComplete)
    {
        this.enemy = enemy;
        StartCoroutine(GetInput(onComplete));
    }

    public void Cancel()
    {
        StopAllCoroutines();
        enemy.Rigidbody.velocity = Vector3.zero;
    }

    /// <summary>
    /// Get direction from context solver and update velocity.
    /// Wait some small amount of time before invoking on behavior complete.
    /// </summary>
    /// <param name="onComplete"></param>
    /// <returns></returns>
    private IEnumerator GetInput(UnityAction onComplete)
    {
        if (!fixDirection)
        {
            Vector3 input = solver.GetDirection(contexts);

            if (Vector3.Distance(transform.position, solver.Target) < sightDistance &&
                solver.Target != Vector3.zero)
            {
                rawInput = input;
                enemy.Rigidbody.velocity = input * speed;
                StartCoroutine(FixedDirectionTimer());
            }
            else
            {
                enemy.Rigidbody.velocity = Vector3.zero;
            }
        }

        yield return null;
        onComplete?.Invoke();
    }

    private IEnumerator FixedDirectionTimer()
    {
        fixDirection = true;
        yield return new WaitForSeconds(.6f);
        fixDirection = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        OnCollisionStay(collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (enemy == null || enemy.CurrBehavior != this) return;
        if (collision.gameObject.layer == LayerMask.NameToLayer("CharacterCollision")) return;

        // Slide along surface
        Vector3 posToTarget = (collision.GetContact(0).point - transform.position).normalized;
        posToTarget.y = transform.position.y;

        // Rotate raw input slightly to avoid a cross product with zero length
        Vector3 cross = Vector3.Cross(collision.GetContact(0).normal, Quaternion.AngleAxis(1, transform.up) * rawInput).normalized;
        enemy.Rigidbody.velocity = Vector3.Cross(cross, collision.GetContact(0).normal).normalized * speed;
    }

    protected void OnDisable()
    {
        StopAllCoroutines();
        fixDirection = false;
    }
}