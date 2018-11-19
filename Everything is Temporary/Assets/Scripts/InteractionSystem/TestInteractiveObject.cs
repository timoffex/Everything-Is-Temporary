using UnityEngine;

/// <summary>
/// Simple example script for how to implement the IInteractiveObject interface.
/// </summary>
public class TestInteractiveObject : MonoBehaviour, IInteractiveObject
{
    public bool CanInteractWith(IInteractor interactor)
    {
        return enabled;
    }

    public bool TryInteractWith(IInteractor interactor)
    {
        return enabled;
    }

    public float GetDistanceTo(IInteractor interactor)
    {
        return Mathf.Abs(transform.position.x - interactor.GetPosition().x);
    }

    public void ShowSelectionVisual()
    {
        Debug.LogFormat("SHOW Visual for {0}.", gameObject.name);

        m_speechBubble?.SetActive(true);
    }

    public void HideSelectionVisual()
    {
        Debug.LogFormat("HIDE Visual for {0}.", gameObject.name);

        m_speechBubble?.SetActive(false);
    }

    private void Awake()
    {
        // Important to keep the return value alive!
        m_interactionSystemUnsubscriber = InteractionSystem.Singleton.RegisterInteractiveObject(this);

        m_speechBubble?.SetActive(false);
    }

    private IUnsubscriber m_interactionSystemUnsubscriber;

    [SerializeField]
    [Tooltip("An object that should be enabled/disabled on ShowSelectionVisual()" +
             " and ShowSelectionVisual().")]
    private GameObject m_speechBubble;
}
