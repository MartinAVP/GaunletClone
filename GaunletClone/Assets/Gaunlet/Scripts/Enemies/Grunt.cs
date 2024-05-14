using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Grunt aproaches the player until they are close and facing the player.
/// Then should repeatedly swing its club at the player until the player moves away.
/// </summary>
public class Grunt : EnemyBase
{
    public float playerDetectionRadius = 10;
    public float obstacleDetectionRadius = 4;
    public float attackDistance = 1f;

    [SerializeField] protected GameObject mesh;

    [SerializeField] protected AnimationClip atttackAnimation;

    [SerializeField] protected GameObject meleeWeapon;
    protected bool meleeWeaponDefaultActive = false;

    protected PlayClip attackBehavior;
    protected MoveTowardsPlayer moveTowardsPlayer;

    protected override void Awake()
    {
        base.Awake();

        attackBehavior = gameObject.AddComponent<PlayClip>();   
        attackBehavior.SetAnimation(atttackAnimation);

        moveTowardsPlayer = gameObject.AddComponent<MoveTowardsPlayer>();
        moveTowardsPlayer.PlayerDetectionRadius = playerDetectionRadius;
        moveTowardsPlayer.ObstacleDetectionRadius = obstacleDetectionRadius;

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

        CurrBehavior = moveTowardsPlayer;
        StartBehavior();
    }

    protected void FixedUpdate()
    {
        if (rb.velocity.magnitude > 0)
        {
            mesh.transform.LookAt(transform.position + rb.velocity);
        }
    }

    protected void PickBehavior()
    {
        Vector3 pos = moveTowardsPlayer.Target;
        float distance = Vector3.Distance(transform.position, pos);

        //Debug.Log(name + " target " + pos + " dist " + distance);

        if (distance < attackDistance)
        {
            CurrBehavior = attackBehavior;
            rb.velocity = Vector3.zero;
        }
        else if (distance > attackDistance)
        {
            CurrBehavior = moveTowardsPlayer;
        }
    }

    protected override void OnBehaviorComplete()
    {
        PickBehavior();
        base.OnBehaviorComplete();
    }

}
