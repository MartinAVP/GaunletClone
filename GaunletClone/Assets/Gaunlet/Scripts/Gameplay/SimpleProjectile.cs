using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleProjectile : MonoBehaviour
{
    [SerializeField] protected float speed;

    protected Rigidbody rb;

    protected virtual void Awake()
    {
        rb = gameObject.AddComponent<Rigidbody>();
        rb.useGravity = false;
    }

    protected void Start()
    {
        rb.velocity = transform.forward * speed;
    }

    protected void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
