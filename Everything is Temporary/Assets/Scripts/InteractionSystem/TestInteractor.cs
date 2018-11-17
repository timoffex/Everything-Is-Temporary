using UnityEngine;

/// <summary>
/// Simple script that calls ShowSelectionVisual() on the closest interactive
/// object, and prints the name of the closest object when the Space key
/// is pressed.
/// </summary>
public class TestInteractor : MonoBehaviour, IInteractor
{
    // Use this for initialization
    private void Awake()
    {
        m_objectTracker = new InteractiveObjectTracker(this);
    }

    // Update is called once per frame
    private void Update()
    {
        m_objectTracker.Update();

        IInteractiveObject nearest = m_objectTracker.GetClosest();

        if (nearest != m_previousNearest)
        {
            nearest?.ShowSelectionVisual();
            m_previousNearest?.HideSelectionVisual();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (nearest == null)
                Debug.Log("No closest interactive object.");
            else if (nearest is MonoBehaviour)
            {
                var nearestMono = nearest as MonoBehaviour;

                Debug.LogFormat("Closest interactive object is {0} at distance {1}.",
                                nearestMono.gameObject.name, nearest.GetDistanceTo(this));
            }
            else
            {
                Debug.LogFormat("Closest interactive object is not a MonoBehaviour, but its" +
                                " distance is {0}", nearest.GetDistanceTo(this));
            }
        }

        m_previousNearest = nearest;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    private InteractiveObjectTracker m_objectTracker;
    private IInteractiveObject m_previousNearest;
}
