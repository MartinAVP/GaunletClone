using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour, IEnemyInterface
{
    private IEnemyBehaviorInterface t_behavior;

    private void Start()
    {
        t_behavior = gameObject.AddComponent<MoveTowardsPlayer>();
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
