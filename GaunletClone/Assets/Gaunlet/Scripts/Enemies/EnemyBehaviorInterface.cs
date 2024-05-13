using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Interface defining how enemies should interact with their behaviors.
/// </summary>
public interface IEnemyBehaviorInterface
{
    /// <summary>
    /// Run a behavior on enemy.
    /// </summary>
    /// <param name="enemy"></param>
    /// <param name="onComplete">Called when behavior is finished.</param>
    public void Execute(IEnemyInterface enemy, UnityAction onComplete);

    /// <summary>
    /// End this behavior immedietely.
    /// </summary>
    public void Cancel();
}
