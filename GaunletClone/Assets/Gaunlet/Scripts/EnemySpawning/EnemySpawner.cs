using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : EnemyPool
{
    public Transform spawnPoint;

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

    /** TESTONLY **/
    public void OnGUI()
    {
        if(GUILayout.Button("Spawn Enemy")){
            Spawn();
        }
    }

    /** ENDTEST **/
}
