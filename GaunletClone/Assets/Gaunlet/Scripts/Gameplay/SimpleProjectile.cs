using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SimpleProjectile : MonoBehaviour
{
    [SerializeField] protected float speed;

    protected Rigidbody rb;
    protected HealthComponent health;

    protected virtual void Awake()
    {
        rb = gameObject.AddComponent<Rigidbody>();
        rb.useGravity = false;
    }

    protected void Start()
    {
        rb.velocity = transform.forward * speed;
    }

    protected void Kill()
    {
        Destroy(gameObject);
    }

    protected void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Default") ||
            other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Kill();
        }
    }
}
