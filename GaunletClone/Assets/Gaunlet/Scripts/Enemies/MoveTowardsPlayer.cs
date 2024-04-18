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
    public List<SteeringStyle> styles = new List<SteeringStyle>();

    private ContextSolver solver;

    private SteeringData steeringData = new SteeringData();

    private void Start()
    {
        detectors.Add(gameObject.AddComponent<ObstacleDetector>());
        styles.Add(gameObject.AddComponent<ObstacleAvoidance>());
        solver = gameObject.AddComponent<ContextSolver>();
    }

    public void Execute(IEnemyInterface enemy, UnityAction onComplete)
    {
        steeringData.obstacles.Clear();
        steeringData.targets.Clear();
        foreach (var detector in detectors)
        {
            steeringData = detector.Detect(steeringData);
        }

        Vector3 input = solver.GetSolvedDirection(styles, steeringData);

        Debug.DrawLine(transform.position, transform.position + input, Color.yellow, .1f);
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
