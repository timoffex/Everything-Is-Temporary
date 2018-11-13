using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SidescrollRaycaster : BaseRaycaster
{
    public override Camera eventCamera => m_delegateRaycaster.eventCamera;

    protected override void Awake()
    {
        m_delegateRaycaster.enabled = false;
    }

    public override void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList)
    {
        Vector2? normalizedCoords = MainRaycastHelper.Singleton.RaycastScrollview(eventData.position);

        if (!normalizedCoords.HasValue)
            return;

        Vector2 oldPosition = eventData.position;

        eventData.position = Vector2.Scale(normalizedCoords.Value, eventCamera.pixelRect.size);
        m_delegateRaycaster.Raycast(eventData, resultAppendList);
        eventData.position = oldPosition;
    }

#if UNITY_EDITOR
    public BaseRaycaster DelegateRaycaster
    {
        get { return m_delegateRaycaster; }
        set { m_delegateRaycaster = value; }
    }
#endif

    [SerializeField]
    [Tooltip("The sidescroll raycaster will forward mouse inputs from the" +
             " sidescrolling quad to this raycaster.")]
    private BaseRaycaster m_delegateRaycaster = null;
}
