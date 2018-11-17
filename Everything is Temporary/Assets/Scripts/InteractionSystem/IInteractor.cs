using UnityEngine;

public interface IInteractor
{
    /// <summary>
    /// The position of this object, in the same units as transform.position.
    /// This is used by interactive objects to determine how far they are from
    /// this.
    /// </summary>
    /// <returns>The position.</returns>
    Vector3 GetPosition();
}