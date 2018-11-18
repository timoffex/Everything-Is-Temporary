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

        if (nearest.GetDistanceTo(this) > m_maximumInteractionDistance)
            nearest = null;

        if (nearest != m_previousNearest)
        {
            nearest?.ShowSelectionVisual();
            m_previousNearest?.HideSelectionVisual();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            nearest?.TryInteractWith(this);
        }

        m_previousNearest = nearest;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }


    [SerializeField]
    private float m_maximumInteractionDistance = 3;

    private InteractiveObjectTracker m_objectTracker;
    private IInteractiveObject m_previousNearest;
}
