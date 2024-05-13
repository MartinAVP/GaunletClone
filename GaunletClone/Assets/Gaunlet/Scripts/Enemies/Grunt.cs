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

        CurrBehavior = moveTowardsPlayer;
        OnBehaviorComplete();

        StartCoroutine(CheckDistanceToTarget());
    }

    protected IEnumerator CheckDistanceToTarget()
    {
        while(true)
        {
            Vector3 pos = moveTowardsPlayer.Target;
            float distance = Vector3.Distance(transform.position, pos);

            //Debug.Log(name + " target " + pos + " dist " + distance);

            if(distance < attackDistance &&
                CurrBehavior != attackBehavior)
            {
                CurrBehavior.Cancel();
                CurrBehavior = attackBehavior;
                OnBehaviorComplete();
            }
            else if(distance > attackDistance &&
                CurrBehavior != moveTowardsPlayer)
            {
                CurrBehavior = moveTowardsPlayer;
            }

            if(rb.velocity.magnitude > 0)
            {
                mesh.transform.LookAt(transform.position + rb.velocity);
            }

            yield return new WaitForSeconds(.1f);
        }
    }

}
