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

    [SerializeField] protected Animation meleeAttackAnim, rangedAttackAnim;

    [SerializeField] protected GameObject projectilePrefab, projectileSpawnPoint;

    protected PlayClip meleeAttackBehavior;
    protected PlayClip rangedAttackBehavior;    // Range attack will play a clip that calls an animation event on this to fire the projectile.
    protected MoveTowardsPlayer chaseBehavior;

    protected override void Awake()
    {
        base.Awake();

        meleeAttackBehavior = gameObject.AddComponent<PlayClip>();
        //meleeAttackBehavior.SetAnimation(meleeAttackAnim);

        rangedAttackBehavior = gameObject.AddComponent<PlayClip>();
        //rangedAttackBehavior.SetAnimation(rangedAttackAnim);

        chaseBehavior = gameObject.AddComponent<MoveTowardsPlayer>();
        chaseBehavior.PlayerDetectionRadius = playerDetectionRadius;
        chaseBehavior.ObstacleDetectionRadius = obstacleDetectionRadius;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        if(CurrBehavior != null)
            CurrBehavior.Cancel();
        CurrBehavior = chaseBehavior;
        OnBehaviorComplete();

        StartCoroutine(PickBehavior());
    }

    protected IEnumerator PickBehavior()
    {
        while (true)
        {
            yield return new WaitForSeconds(.1f);
        }
    }

    public void FireProjectile()
    {
        Instantiate(projectilePrefab, projectileSpawnPoint.transform.position, mesh.transform.rotation);
    }
}
