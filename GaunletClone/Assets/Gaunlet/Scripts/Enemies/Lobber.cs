using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lobber : EnemyBase
{
    public float playerDetectionRadius,
        obstacleDetectionRadius,
        fleeDistance,
        minProjectileInterval, maxProjectileInterval;

    [SerializeField] protected GameObject mesh;

    [SerializeField] protected AnimationClip rangedAttackAnim;

    [SerializeField] protected LobProjectile projectilePrefab;
    [SerializeField] protected GameObject projectileSpawnPoint;

    // Behaviors
    protected PlayClip rangedAttackBehavior;
    protected MoveTowardsPlayer chaseBehavior;
    // TODO: evade behavior (when player is too close)

    protected float chaseTime = 0;
    protected float timeUntilRangedAttack;

    protected override void Awake()
    {
        base.Awake();

        chaseBehavior = gameObject.AddComponent<MoveTowardsPlayer>();
        chaseBehavior.PlayerDetectionRadius = playerDetectionRadius;
        chaseBehavior.ObstacleDetectionRadius = obstacleDetectionRadius;

        rangedAttackBehavior = gameObject.AddComponent<PlayClip>();
        rangedAttackBehavior.SetAnimation(rangedAttackAnim);    

        timeUntilRangedAttack = Random.Range(minProjectileInterval, maxProjectileInterval);

        HealthComponent health = GetComponent<HealthComponent>();
        if(health != null)
        {
            health.onHealthDepleted += Kill;
        }
    }

    protected void PickBehavior()
    {
        Vector3 target = chaseBehavior.Target;
        float distance = Vector3.Distance(target, transform.position);

        if(distance > fleeDistance)
        {
            if(target != Vector3.zero && distance < playerDetectionRadius && chaseTime > timeUntilRangedAttack)
            {
                CurrBehavior = rangedAttackBehavior;
                timeUntilRangedAttack = Random.Range(minProjectileInterval, maxProjectileInterval);
                rb.velocity = Vector3.zero;
                chaseTime = 0;
            }
            else
            {
                CurrBehavior = chaseBehavior;
            }
        }
        else
        {
            // flee behavior would go here
            CurrBehavior = chaseBehavior;
        }

        Debug.Log(name + " " + CurrBehavior);
    }

    protected void FixedUpdate()
    {
        if (rb.velocity.magnitude > 0)
        {
            mesh.transform.LookAt(transform.position + rb.velocity);
        }

        if (CurrBehavior == chaseBehavior)
        {
            chaseTime += Time.fixedDeltaTime;
        }
        else
        {
            chaseTime = 0;
        }
    }

    protected override void OnBehaviorComplete()
    {
        PickBehavior();
        base.OnBehaviorComplete();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        PickBehavior();
        chaseTime = 0;

        StartBehavior();
    }

    public void LaunchProjectile()
    {
        Debug.Log(name + " launch projectile.");
        LobProjectile prjctl = Instantiate(projectilePrefab, projectileSpawnPoint.transform.position, Quaternion.identity);
        prjctl.Launch(chaseBehavior.Target);
    }
}
