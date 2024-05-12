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

    [SerializeField] protected Animation atttackAnimation;

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
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        /* TESTONLY - testing animation */

        CurrBehavior = attackBehavior;
        OnBehaviorComplete();

        /* ENDTEST */
    }

}
