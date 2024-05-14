using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Demon can:
/// Chase player
/// Randomly stop and shoot a fireball
/// Melee attack player when close
/// </summary>
public class Demon : EnemyBase
{
    public float playerDetectionRadius, 
        obstacleDetectionRadius, 
        attackDistance,
        minProjectileInterval, maxProjectileInterval;

    [SerializeField] protected GameObject mesh;

    [SerializeField] protected AnimationClip meleeAttackAnim, rangedAttackAnim;

    [SerializeField] protected GameObject projectilePrefab, projectileSpawnPoint;

    protected PlayClip meleeAttackBehavior;
    protected PlayClip rangedAttackBehavior;    // Range attack will play a clip that calls an animation event on this to fire the projectile.
    protected MoveTowardsPlayer chaseBehavior;

    protected float timeUntilRangedAttack;
    protected bool shouldRangAttack = false;

    protected IEnumerator rangedAttackTimer;

    protected float chaseTime;

    protected override void Awake()
    {
        base.Awake();

        meleeAttackBehavior = gameObject.AddComponent<PlayClip>();
        meleeAttackBehavior.SetAnimation(meleeAttackAnim);

        rangedAttackBehavior = gameObject.AddComponent<PlayClip>();
        rangedAttackBehavior.SetAnimation(rangedAttackAnim);

        chaseBehavior = gameObject.AddComponent<MoveTowardsPlayer>();
        chaseBehavior.PlayerDetectionRadius = playerDetectionRadius;
        chaseBehavior.ObstacleDetectionRadius = obstacleDetectionRadius;

        timeUntilRangedAttack = Random.Range(minProjectileInterval, maxProjectileInterval);
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        PickBehavior();
        StartBehavior();
    }

    protected void FixedUpdate()
    {
        if (rb.velocity.magnitude > 0)
        {
            mesh.transform.LookAt(transform.position + rb.velocity);
        }

        if(CurrBehavior == chaseBehavior)
        {
            chaseTime += Time.fixedDeltaTime;
        }
        else
        {
            chaseTime = 0;
        }
    }

    protected void PickBehavior()
    {
        Vector3 target = chaseBehavior.Target;
        float distance = Vector3.Distance(target, transform.position);

        if (distance < attackDistance)
        {
            // Melee
            CurrBehavior = meleeAttackBehavior;
            rb.velocity = Vector3.zero;
        }
        else if (distance > attackDistance)
        {
            if (chaseTime > timeUntilRangedAttack)
            {
                CurrBehavior = rangedAttackBehavior;
                timeUntilRangedAttack = Random.Range(minProjectileInterval, maxProjectileInterval);
                rb.velocity = Vector3.zero;
                shouldRangAttack = false;
            }
            else
            {
                CurrBehavior = chaseBehavior;
            }
        }
    }

    protected IEnumerator RangedAttackTimer()
    {
        while (true)
        {
            timeUntilRangedAttack = Random.Range(minProjectileInterval, maxProjectileInterval);
            yield return new WaitForSeconds(timeUntilRangedAttack);
            shouldRangAttack = true;
        }
    }

    protected override void OnBehaviorComplete()
    {
        PickBehavior();

        base.OnBehaviorComplete();
    }

    public void FireProjectile()
    {
        Debug.Log(name + " fire projectile");
        Instantiate(projectilePrefab, projectileSpawnPoint.transform.position, mesh.transform.rotation);
    }
}
