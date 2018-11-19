using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Trigger : MonoBehaviour
{
    /// <summary>
    /// This event is fired when the trigger is triggered.
    /// </summary>
    public event System.Action OnTrigger;

    /// <summary>
    /// Call this from derived classes to invoke the OnTrigger event.
    /// </summary>
    protected void InvokeTrigger()
    {
        if (!m_triggerOnce || !m_triggered)
            OnTrigger?.Invoke();

        m_triggered = true;

        if (m_triggered)
            Destroy(this);
    }

    [SerializeField]
    [Tooltip("Should this trigger fire multiple times or just once?")]
    private bool m_triggerOnce = true;

    private bool m_triggered = false;
}
