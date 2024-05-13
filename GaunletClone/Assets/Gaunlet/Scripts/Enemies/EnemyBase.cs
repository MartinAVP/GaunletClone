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

    /// <summary>
    /// Override to add functionality that should happen once, only the first time the enemy spawns.
    /// </summary>
    protected virtual void Awake()
    {
        // Set constraints on the rigidbody.
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezePositionY;
        rb.freezeRotation = true;
        rb.useGravity = false;
    }

    /// <summary>
    /// Override to add custom functionality when this enemy spawns.
    /// </summary>
    protected virtual void OnEnable()
    {
        rb.velocity = Vector3.zero;
    }

    /// <summary>
    /// Functionality to run when current behvior completes. By default loops the current behavior.
    /// </summary>
    protected virtual void OnBehaviorComplete()
    {
        //Debug.Log(name + " on complete");
        currBehavior.Execute(this, OnBehaviorComplete);
    }

    /// <summary>
    /// Logic to clean up a dead enemy.
    /// </summary>
    protected virtual void Kill()
    {
        Pool.Release(this);
    }
}
