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

    private void Awake()
    {
        detectors.Add(gameObject.AddComponent<ObstacleDetector>());
        detectors.Add(gameObject.AddComponent<TargetDetector>());
        styles.Add(gameObject.AddComponent<ObstacleAvoidance>());
        styles.Add(gameObject.AddComponent<TargetSeeking>());
        solver = gameObject.AddComponent<ContextSolver>();
    }

    public void Execute(IEnemyInterface enemy, UnityAction onComplete)
    {
        StartCoroutine(GetInput(onComplete));   
    }

    private IEnumerator GetInput(UnityAction onComplete)
    {
        steeringData.obstacles.Clear();
        steeringData.targets.Clear();
        foreach (var detector in detectors)
        {
            steeringData = detector.Detect(steeringData);
        }

        Vector3 input = solver.GetSolvedDirection(styles, steeringData);

        Debug.DrawLine(transform.position, transform.position + input, Color.yellow, .1f);

        yield return null;
        onComplete?.Invoke();
    }


}
