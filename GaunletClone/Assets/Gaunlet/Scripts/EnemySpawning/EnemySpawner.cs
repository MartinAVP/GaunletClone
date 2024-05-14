using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : EnemyPool
{
    public Transform spawnPoint;

    protected void Awake()
    {
        HealthComponent health = GetComponent<HealthComponent>();
        if(health != null )
        {
            health.onHealthDepleted += Kill;
        }
    }

    protected void OnEnable()
    {
        StartCoroutine(SpawnRandomInterval());
    }

    protected  IEnumerator SpawnRandomInterval()
    {
        while (true)
        {
            float spawnInterval = Random.Range(spawnData.MinInterval, spawnData.MaxInterval);
            yield return new WaitForSeconds(spawnInterval);
            Spawn();
        }
    }

    /// <summary>
    /// Place a fresh enemy at spawn point.
    /// </summary>
    /// <returns></returns>
    public EnemyBase Spawn()
    {
        EnemyBase newEnemy = Pool.Get();
        newEnemy.transform.position = spawnPoint.position;
        newEnemy.transform.rotation = spawnPoint.rotation;

        return newEnemy;
    }

    protected void Kill()
    {
        StopAllCoroutines();
        gameObject.SetActive(false);
    }
}
