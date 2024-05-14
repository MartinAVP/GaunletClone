using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class LobProjectile : MonoBehaviour
{
    [SerializeField] protected float verticalSpeed = 1;
    [SerializeField] protected float speed = 1;
    Vector3 targetPos;
    protected float baseHeight;

    protected Rigidbody rb;
    protected HealthComponent health;

    protected void Awake()
    {
        rb = gameObject.AddComponent<Rigidbody>();
        rb.useGravity = false;

        health = GetComponent<HealthComponent>();
        health.onHealthDepleted += Kill;

        //Launch(Vector3.zero);
    }

    public void Launch(Vector3 targetPos)
    {
        targetPos.y = baseHeight;
        this.targetPos = targetPos;

        baseHeight = transform.position.y;

        transform.LookAt(targetPos);
        rb.velocity = transform.forward * speed;

        StartCoroutine(Move());
    }

    protected IEnumerator Move()
    {
        float distance = Vector3.Distance(transform.position, targetPos);
        float distTraveled = 0;

        Debug.Log(name + " dist: " + distance);

        while(distTraveled < distance)
        {
            Vector3 flatDelta = rb.velocity * Time.fixedDeltaTime;
            flatDelta.y = 0;
            distTraveled += flatDelta.magnitude;

            Debug.Log(name + " dist traveled: " + distTraveled);

            float alpha = distTraveled / distance;

            Debug.Log(rb.velocity);

            if(alpha < .5f)
            {
                Vector3 newVel = rb.velocity;
                newVel.y = verticalSpeed * (.5f - alpha) * 2;
                rb.velocity = newVel;
            }
            else
            {
                Vector3 newVel = rb.velocity;
                newVel.y = -verticalSpeed * (alpha - .5f) * 2;
                rb.velocity = newVel;
            }

            yield return new WaitForFixedUpdate();
        }

        Kill();
    }

    protected void Kill()
    {
        Destroy(gameObject);
    }

    protected void OnDestroy()
    {
        health.onHealthDepleted -= Kill;
    }
}
