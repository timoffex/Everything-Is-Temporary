
using System.Collections.Generic;

/// <summary>
/// Interface implemented by everything that can be interacted with.
/// </summary>
public interface IInteractiveObject
{
    /// <summary>
    /// Determines whether this object can interact with the <paramref name="interactor"/>.
    /// </summary>
    bool CanInteractWith(IInteractor interactor);

    /// <summary>
    /// Tries to interact with <paramref name="interactor"/>.
    /// </summary>
    /// <returns><see langword="true"/> if the interaction happened,
    /// <see langword="false"/> otherwise.</returns>
    bool TryInteractWith(IInteractor interactor);

    /// <summary>
    /// Gets the distance to the <paramref name="interactor"/>.
    /// This is used to sort interactive objects to determine which is closest.
    /// Return a distance in the same units as the ones used in the Transform
    /// component (e.g. the distance between something whose transform.position
    /// is (0, 0, 0) and something at position (1, 1, 0) is sqrt(2)). Use
    /// Euclidean distance. This should perform efficiently.
    /// </summary>
    float GetDistanceTo(IInteractor interactor);

    /// <summary>
    /// Shows the selection visual.
    /// </summary>
    void ShowSelectionVisual();

    /// <summary>
    /// Hides the selection visual.
    /// </summary>
    void HideSelectionVisual();
}
