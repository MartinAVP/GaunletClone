using UnityEngine;

/// <summary>
/// Interface defining how other objects should interact with an enemy.
/// </summary>
public interface IEnemyInterface
{
    /// <summary>
    /// "Rigidbody of this enemy. Authoriative source of physics driven movement."
    /// </summary>
    public Rigidbody Rigidbody { get; }
}
