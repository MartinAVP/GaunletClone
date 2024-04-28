using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour, IEnemyInterface
{
    public float playerDetectionRadius = 10;
    public float obstacleDetectionRadius = 4;

    private IEnemyBehaviorInterface t_behavior;

    private void Start()
    {
        MoveTowardsPlayer moveTowardsPlayer = gameObject.AddComponent<MoveTowardsPlayer>();
        moveTowardsPlayer.PlayerDetectionRadius = playerDetectionRadius;
        moveTowardsPlayer.ObstacleDetectionRadius = obstacleDetectionRadius;
        t_behavior = moveTowardsPlayer;

        t_behavior.Execute(this, OnBehaviorComplete);
    }

    private void OnBehaviorComplete()
    {
        //Debug.Log(name + " behavior complete.");
        t_behavior.Execute(this, OnBehaviorComplete);
    }

    public void Move(Vector3 delta)
    {
        transform.Translate(delta);
    }
}
