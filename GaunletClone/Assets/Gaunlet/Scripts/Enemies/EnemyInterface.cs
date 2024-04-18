using UnityEngine;

public interface IEnemyInterface
{
    /// <summary>
    /// Translate the enemy by delta.
    /// </summary>
    /// <param name="delta"></param>
    public void Move(Vector3 delta);
}
