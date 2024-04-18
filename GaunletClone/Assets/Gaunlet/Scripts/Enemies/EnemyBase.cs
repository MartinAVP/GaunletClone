using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour, IEnemyInterface
{
    private IEnemyBehaviorInterface t_behavior;

    private void Start()
    {
        t_behavior = gameObject.AddComponent<MoveTowardsPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        t_behavior.Execute(this, OnBehaviorComplete);
    }

    private void OnBehaviorComplete()
    {

    }

    public void Move(Vector3 delta)
    {
        transform.Translate(delta);
    }
}
