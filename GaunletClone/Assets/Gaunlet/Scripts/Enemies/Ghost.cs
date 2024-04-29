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

    protected void Start()
    {
        moveTowardsPlayer = gameObject.AddComponent<MoveTowardsPlayer>();
        moveTowardsPlayer.PlayerDetectionRadius = playerDetectionRadius;
        moveTowardsPlayer.ObstacleDetectionRadius = obstacleDetectionRadius;
        CurrBehavior = moveTowardsPlayer;
        OnBehaviorComplete();
    }

    protected void OnTriggerEnter(Collider other)
    {
        // TODO: Ghost deals damage & dies when it hits a player.
    }
}
