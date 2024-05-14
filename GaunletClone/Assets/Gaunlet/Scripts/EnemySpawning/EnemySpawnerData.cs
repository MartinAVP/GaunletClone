using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemySpawnData", menuName = "EnemySpawnerData")]
public class EnemySpawnerData : ScriptableObject 
{
    [SerializeField] protected int depth, stackCap;
    public int Depth { get { return depth; } }
    public int StackCap { get {  return stackCap; } }
    [SerializeField] protected float minInterval, maxInterval;
    public float MinInterval { get { return minInterval; } }
    public float MaxInterval { get { return maxInterval; } }

    [SerializeField] protected EnemyBase enemyPrefab;
    public EnemyBase EnemyPrefab { get { return enemyPrefab; } }
}
