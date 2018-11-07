using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Put this script on any EventSystem in a town. This will automatically
/// disable the EventSystem when the town is loaded, and it will also
/// register the event system as the town event system with the GameManager.
/// </summary>
[RequireComponent(typeof(EventSystem))]
public class TownEventSystem : MonoBehaviour
{
    private void Awake()
    {
        EventSystem eventSystem = GetComponent<EventSystem>();

        // Disable by default.
        eventSystem.enabled = false;

        GameManager.Singleton.RegisterTownEventSystem(eventSystem);
    }
}
