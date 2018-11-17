using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class to keep track of all interactive objects and to send out events
/// when they are created and destroyed. This class is not thread-safe and
/// should be accessed only on the main Unity thread (the one that calls
/// Update() and Start()). TODO
/// </summary>
public class InteractionSystem : MonoBehaviour
{

    public static InteractionSystem Singleton
    {
        get
        {
            if (_singleton == null)
            {
                var go = new GameObject("Interaction System");
                _singleton = go.AddComponent<InteractionSystem>();
            }

            return _singleton;
        }
    }

    public delegate void InteractiveObjectDelegate(IInteractiveObject obj);

    /// <summary>
    /// Occurs when an interactive object is registered.
    /// </summary>
    public event InteractiveObjectDelegate OnInteractiveObjectRegistered;

    /// <summary>
    /// Occurs when an interactive object is unregistered.
    /// </summary>
    public event InteractiveObjectDelegate OnInteractiveObjectUnregistered;

    /// <summary>
    /// Registers the interactive object and returns an IUnsubscriber that can
    /// be used to unregister the object. The unsubscriber will invoke Unsubscribe()
    /// in its finalizer, so a reference to the unsubscriber must be kept. When
    /// the interactive object is supposed to be destroyed, make sure to Unsubscribe()
    /// or else some code that captured it during OnInteractiveObjectRegistered
    /// might keep it alive indefinitely.
    /// </summary>
    /// <returns>The interactive object.</returns>
    /// <param name="obj">Object.</param>
    public IUnsubscriber RegisterInteractiveObject(IInteractiveObject obj)
    {
        m_currentInteractiveObjects.Add(obj);

        OnInteractiveObjectRegistered?.Invoke(obj);

        return new ListUnsubscriber<IInteractiveObject>(m_currentInteractiveObjects, obj,
                                                       (x) => OnInteractiveObjectUnregistered?.Invoke(x));
    }

    /// <summary>
    /// Returns an object that can be used to loop through all registered
    /// interactive objects.
    /// </summary>
    /// <returns>The objects.</returns>
    public IEnumerable<IInteractiveObject> InteractiveObjects()
    {
        return m_currentInteractiveObjects;
    }

    private void Awake()
    {
        if (_singleton == null)
        {
            _singleton = this;
        }
        else
        {
            Debug.LogWarning("Duplicate InteractionSystem script detected. " +
                             "You don't need to create your own game object with an " +
                             "InteractionSystem script.");
            Destroy(this);
        }
    }

    private static InteractionSystem _singleton;

    private List<IInteractiveObject> m_currentInteractiveObjects = new List<IInteractiveObject>();
}
