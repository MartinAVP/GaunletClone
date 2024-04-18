using UnityEngine;
using UnityEngine.Events;

public interface IEnemyBehaviorInterface
{
    /// <summary>
    /// Run a behavior on enemy.
    /// </summary>
    /// <param name="enemy"></param>
    /// <param name="onComplete">Called when behavior is finished.</param>
    public void Execute(IEnemyInterface enemy, UnityAction onComplete);
}
