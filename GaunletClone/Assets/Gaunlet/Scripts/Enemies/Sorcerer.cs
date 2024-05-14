using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sorcerer can:
/// Chase player
/// Randomly stop and shoot a fireball
/// Melee attack player when close
/// Randomly disappear for a few seconds (continues to move, doesn't collide with objects)
/// </summary>
public class Sorcerer : EnemyBase
{
    public float playerDetectionRadius,
        obstacleDetectionRadius,
        attackDistance,
        minProjectileInterval, maxProjectileInterval;

    [SerializeField] protected GameObject mesh;

    [SerializeField] protected AnimationClip meleeAttackAnim, rangedAttackAnim;

    [SerializeField] protected GameObject projectilePrefab, projectileSpawnPoint;

    [SerializeField] protected GameObject meleeWeapon;
    protected bool meleeWeaponDefaultActive = false;

    [SerializeField, Tooltip("Parent of everything that can turn invisible")] protected GameObject invisibleBody;
    [SerializeField] float minVisibilityTime, maxVisibilityTime, minInvisibilityTime, maxInvisibilityTime;

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

        meleeWeaponDefaultActive = meleeWeapon.activeInHierarchy;

        HealthComponent health = GetComponent<HealthComponent>();
        if (health != null)
        {
            health.onHealthDepleted += Kill;
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        meleeWeapon.SetActive(meleeWeaponDefaultActive);

        invisibleBody.SetActive(true);
        StartCoroutine(RandomlyInvisible());

        CurrBehavior = chaseBehavior;
        StartBehavior();
    }

    protected void FixedUpdate()
    {
        if (rb.velocity.magnitude > 0)
        {
            mesh.transform.LookAt(transform.position + rb.velocity);
        }

        if (CurrBehavior == chaseBehavior && invisibleBody.activeInHierarchy)
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
            invisibleBody.SetActive(true);
        }
        else if (distance > attackDistance)
        {
            if (target != Vector3.zero && distance <= playerDetectionRadius && chaseTime > timeUntilRangedAttack && invisibleBody.activeInHierarchy)
            {
                CurrBehavior = rangedAttackBehavior;
                timeUntilRangedAttack = Random.Range(minProjectileInterval, maxProjectileInterval);
                rb.velocity = Vector3.zero;
                shouldRangAttack = false;
                chaseTime = 0;
            }
            else
            {
                CurrBehavior = chaseBehavior;
            }
        }

        Debug.Log(name + " " + CurrBehavior);
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

    protected IEnumerator RandomlyInvisible()
    {
        float time = 0;
        float timeUntilSwap = invisibleBody.activeInHierarchy ? Random.Range(minVisibilityTime, maxVisibilityTime) : Random.Range(minInvisibilityTime, maxInvisibilityTime);

        while(true)
        {
            time += Time.fixedDeltaTime;
            if(time >= timeUntilSwap  && CurrBehavior == chaseBehavior)
            {
                invisibleBody.SetActive(!invisibleBody.activeInHierarchy);
                timeUntilSwap = invisibleBody.activeInHierarchy ? Random.Range(minVisibilityTime, maxVisibilityTime) : Random.Range(minInvisibilityTime, maxInvisibilityTime);
                time = 0;
            }

            yield return new WaitForFixedUpdate();
        }
    }

    protected override void OnBehaviorComplete()
    {
        PickBehavior();

        base.OnBehaviorComplete();
    }

    public void FireProjectile()
    {
        //Debug.Log(name + " fire projectile");
        Instantiate(projectilePrefab, projectileSpawnPoint.transform.position, mesh.transform.rotation);
    }
}
