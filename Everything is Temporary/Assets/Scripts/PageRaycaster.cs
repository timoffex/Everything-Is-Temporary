using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Canvas))]
public class PageRaycaster : GraphicRaycaster
{
    public PageCoordinates.PageSide side = PageCoordinates.PageSide.Left;

    protected override void Awake()
    {
        m_canvas = GetComponent<Canvas>();
    }

    public override void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList)
    {
        PageCoordinates? coords = null;

        switch (side)
        {
            case PageCoordinates.PageSide.Left:
                coords = MainRaycastHelper.Singleton.RaycastLeftPage(eventData.position);
                break;
            case PageCoordinates.PageSide.Right:
                coords = MainRaycastHelper.Singleton.RaycastRightPage(eventData.position);
                break;
        }

        if (coords.HasValue)
        {
            Vector2 oldPosition = eventData.position;

            eventData.position = Vector2.Scale(coords.Value.coords, m_canvas.pixelRect.size);
            base.Raycast(eventData, resultAppendList);

            eventData.position = oldPosition;
        }
    }

    private Canvas m_canvas;
}
