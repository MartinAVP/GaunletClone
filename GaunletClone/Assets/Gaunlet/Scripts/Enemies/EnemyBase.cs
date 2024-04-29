using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Awake()
    {
        // Set constraints on the rigidbody.
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezePositionY;
        rb.freezeRotation = true;
        rb.useGravity = false;
    }

    /// <summary>
    /// Functionality to run when current behvior completes. By default loops the current behavior.
    /// </summary>
    protected virtual void OnBehaviorComplete()
    {
        currBehavior.Execute(this, OnBehaviorComplete);
    }
}
