using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// Base class for all enemies. 
/// Starts executing behavior, looping when that behvior is complete.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class EnemyBase : MonoBehaviour, IEnemyInterface
{
    [Tooltip("Currently running behavior.")]
    private IEnemyBehaviorInterface currBehavior;
    [Tooltip("Currently running behavior.")]
    public IEnemyBehaviorInterface CurrBehavior
    {
        get { return currBehavior; }
        set { currBehavior = value; }
    }

    [Tooltip("Rigidbody of this enemy. Authoriative source of physics driven movement.")]
    public Rigidbody Rigidbody
    {
        get { return rb; }
    }

    protected Rigidbody rb;
    protected Vector3 accumulateMoveInput = Vector3.zero;

    public IObjectPool<EnemyBase> Pool { get; set; }

    protected virtual void Awake()
    {
        // Set constraints on the rigidbody.
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezePositionY;
        rb.freezeRotation = true;
        rb.useGravity = false;
    }

    private void OnEnable()
    {
        Debug.Log(name + " enemy enabled.");
        ResetEnemy();
    }

    protected virtual void Kill()
    {
        Debug.Log(name + " enemy killed.");
        Pool.Release(this);
    }

    protected virtual void ResetEnemy()
    {
        Debug.Log(name + " enemy reset.");
    }

    /// <summary>
    /// Functionality to run when current behvior completes. By default loops the current behavior.
    /// </summary>
    protected virtual void OnBehaviorComplete()
    {
        currBehavior.Execute(this, OnBehaviorComplete);
    }

    private void OnDisable()
    {
        Debug.Log(name + " enemy disabled.");
    }
}
