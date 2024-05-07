using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyPool : MonoBehaviour
{
    [SerializeField] protected EnemySpawnerData spawnData;

    private IObjectPool<EnemyBase> _pool;
    public IObjectPool<EnemyBase> Pool
    {
        get
        {
            if(_pool == null)
            {
                _pool = new ObjectPool<EnemyBase>(OnCreate, OnGet, OnRelease, OnDestroyPooledObject, true, spawnData.Depth, spawnData.StackCap);
            }
            return _pool;
        }
    }

    private EnemyBase OnCreate()
    {
        EnemyBase newEnemy = Instantiate(spawnData.EnemyPrefab);
        newEnemy.Pool = _pool;

        return newEnemy;
    }

    private void OnGet(EnemyBase enemy)
    {
        enemy.gameObject.SetActive(true);
    }

    private void OnRelease(EnemyBase enemy)
    {
        enemy.gameObject.SetActive(false);
    }

    private void OnDestroyPooledObject(EnemyBase enemy)
    {
        Destroy(enemy.gameObject);
    }
}
