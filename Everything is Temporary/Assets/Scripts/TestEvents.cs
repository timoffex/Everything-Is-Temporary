using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Script to add to an object to test if it receives various EventSystem
/// events.
/// </summary>
public class TestEvents : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        Debug.LogFormat("It worked! I am {0}.", name);
    }
}
