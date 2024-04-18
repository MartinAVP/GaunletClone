using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MoveTowardsPlayer : MonoBehaviour, IEnemyBehaviorInterface
{
    public float sightDistance;
    public float attackRange;

    public List<Detector> detectors = new List<Detector>();

    private SteeringData steeringData = new SteeringData();

    private void Start()
    {
        detectors.Add(gameObject.AddComponent<ObstacleDetector>());
    }

    public void Execute(IEnemyInterface enemy, UnityAction onComplete)
    {
        steeringData.obstacles.Clear();
        steeringData.targets.Clear();
        foreach (var detector in detectors)
        {
            detector.Detect(steeringData);
        }
    }

    /* TESTONLY */

    private void OnGUI()
    {
        if(GUILayout.Button("Print obstacles"))
        {
            foreach(var obstacle in steeringData.obstacles)
            {
                Debug.Log(obstacle);
            }
        }
    }
}
