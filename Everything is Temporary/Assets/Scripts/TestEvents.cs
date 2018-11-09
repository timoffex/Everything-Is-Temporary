using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Script to add to an object to test if it receives various EventSystem
/// events.
/// </summary>
public class TestEvents : MonoBehaviour,
    IPointerClickHandler,
    ISelectHandler,
    IDragHandler
{
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        Debug.LogFormat("I am {0} and I am clicked.", name);
    }

    public void OnSelect(BaseEventData data)
    {
        Debug.LogFormat("I am {0} and I am selected.", name);
    }

    public void OnDrag(PointerEventData data)
    {
        Debug.LogFormat("I am {0} and I am being dragged.", name);
    }
}
