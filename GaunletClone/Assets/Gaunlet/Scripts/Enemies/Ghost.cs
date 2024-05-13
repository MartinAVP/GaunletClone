using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ghost enemy.
/// Chases players and die when it collides with one.
/// </summary>
public class Ghost : EnemyBase
{
    public float playerDetectionRadius = 10;
    public float obstacleDetectionRadius = 4;

    MoveTowardsPlayer moveTowardsPlayer;

    protected override void Awake()
    {
        base.Awake();

        moveTowardsPlayer = gameObject.AddComponent<MoveTowardsPlayer>();
        moveTowardsPlayer.PlayerDetectionRadius = playerDetectionRadius;
        moveTowardsPlayer.ObstacleDetectionRadius = obstacleDetectionRadius;

        HealthComponent health = GetComponent<HealthComponent>();
        if(health != null)
        {
            health.onHealthDepleted += Kill;
            health.onTakeDamage += KillIfTouchPlayer;
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        CurrBehavior = moveTowardsPlayer;
        OnBehaviorComplete();
    }

    protected void KillIfTouchPlayer(DamageInfo info)
    {
        if(info.type == DamageType.Body)
        {
            Kill();
        }
    }
}
