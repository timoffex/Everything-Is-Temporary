using UnityEngine;

/// <summary>
/// Simple example script for how to implement the IInteractiveObject interface.
/// </summary>
public class TestInteractiveObject : MonoBehaviour, IInteractiveObject
{
    public bool CanInteractWith(IInteractor interactor) => true;
    public bool TryInteractWith(IInteractor interactor) => true;

    public float GetDistanceTo(IInteractor interactor)
    {
        return Mathf.Abs(transform.position.x - interactor.GetPosition().x);
    }

    public void ShowSelectionVisual()
    {
        Debug.LogFormat("SHOW Visual for {0}.", gameObject.name);
    }

    public void HideSelectionVisual()
    {
        Debug.LogFormat("HIDE Visual for {0}.", gameObject.name);
    }

    private void Awake()
    {
        // Important to keep the return value alive!
        m_unsubscriber = InteractionSystem.Singleton.RegisterInteractiveObject(this);
    }

    private IUnsubscriber m_unsubscriber;
}
