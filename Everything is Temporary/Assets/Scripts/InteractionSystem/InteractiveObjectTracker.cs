using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This helper class tracks IInteractiveObjects and helps efficiently
/// determine questions about which one is closest to a particular IInteractor.
/// This class is not thread-safe, and depends on InteractionSystem events
/// to happen synchronously with the usage of this class.
/// </summary>
public class InteractiveObjectTracker
{

    public InteractiveObjectTracker(IInteractor interactor)
    {
        m_interactor = interactor;
    }

    /// <summary>
    /// Compute the closest interactive object that can be interacted with
    /// by this interactor.
    /// </summary>
    public void Update()
    {
        double closestDistance = double.PositiveInfinity;
        m_closest = null;

        foreach (var obj in InteractionSystem.Singleton.InteractiveObjects())
        {
            if (obj.CanInteractWith(m_interactor))
            {
                double distance = obj.GetDistanceTo(m_interactor);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    m_closest = obj;
                }
            }
        }
    }

    /// <summary>
    /// Gets the closest interactive object that can interact with the interactor.
    /// Can be null if no such object exists. Use after calling Update().
    /// </summary>
    /// <returns>The closest.</returns>
    public IInteractiveObject GetClosest() => m_closest;

    /// <summary>
    /// The closest object that can interact with the interactor.
    /// </summary>
    private IInteractiveObject m_closest;
    private IInteractor m_interactor;
}
