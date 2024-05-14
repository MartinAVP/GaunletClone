using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : EnemyBase
{
    public float playerDetectionRadius,
        obstacleDetectionRadius;

    [SerializeField] protected GameObject mesh;

    protected MoveTowardsPlayer chaseBehavior;

    public DamageInfo damage;
    protected float overlapTime;

    protected List<HurtBox> hurtBoxes = new List<HurtBox>();

    protected override void Awake()
    {
        base.Awake();

        chaseBehavior = gameObject.AddComponent<MoveTowardsPlayer>();
        chaseBehavior.PlayerDetectionRadius = playerDetectionRadius;
        chaseBehavior.ObstacleDetectionRadius = obstacleDetectionRadius;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        CurrBehavior = chaseBehavior;

        StartBehavior();
    }

    protected void FixedUpdate()
    {
        overlapTime += Time.fixedDeltaTime;
        if (overlapTime >= 1)
        {
            foreach(HurtBox box in hurtBoxes)
            {
                box.TakeDamage(damage);
            }

            overlapTime = 0;
        }
    }

    protected void OnTriggerEnter(Collider other)
    {
        HurtBox hurtBox = other.GetComponent<HurtBox>();
        if(hurtBox != null)
        {
            hurtBoxes.Add(hurtBox);
        }
    }

    protected void OnTriggerExit(Collider other)
    {
        HurtBox hurtBox = other.GetComponent<HurtBox>();
        if (hurtBox != null)
        {
            hurtBoxes.Remove(hurtBox);
        }
    }

    protected override void Kill()
    {
        Destroy(gameObject);
    }
}
