﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// InputModule for book pages. This class is responsible for sending
/// mouse-related events to page canvases (this is what causes buttons' OnClick()
/// methods to fire, for example).
/// </summary>
/// <remarks>
/// There can only ever be one InputModule active. This means that other
/// UI systems will not work simultaneously with pages. When the need arises
/// for out-of-page UI elements that are usable simultaneously with the book,
/// we can write a more complex InputModule.
/// </remarks>
public class PageInputModule : BaseInputModule
{
    public MouseInput mouseInput;

    public Canvas leftCanvas;
    public Canvas rightCanvas;

    /// <summary>
    /// After this method is called, this input module is guaranteed to
    /// ignore all input until at least after Unsubscribe() is invoked
    /// on the return value. REMEMBER TO CALL Unsubscribe().
    /// </summary>
    public IUnsubscriber IgnoreInput ()
    {
        ++m_inputBlockCount;
        return new ActionUnsubscriber(() => --m_inputBlockCount);
    }

    public void SetLeftCanvas(Canvas left)
    {
        leftCanvas = left;
        m_leftRaycaster = leftCanvas.GetComponent<GraphicRaycaster>();
    }

    public void SetRightCanvas(Canvas right)
    {
        rightCanvas = right;
        m_rightRaycaster = rightCanvas.GetComponent<GraphicRaycaster>();
    }

    public override void ActivateModule()
    {
        base.ActivateModule();

        GameManager.Singleton.RegisterPageInputModule(this);

        m_clickHappened = false;
        m_eventSystem = GetComponent<EventSystem>();

        mouseInput.OnPageClick += OnPageClick;

        m_leftRaycaster = leftCanvas.GetComponent<GraphicRaycaster>();
        m_rightRaycaster = rightCanvas.GetComponent<GraphicRaycaster>();
    }

    public override void Process()
    {
        if (m_inputBlockCount > 0)
            return;

        if (m_clickHappened)
        {
            if (m_clickLocation.side == PageCoordinates.PageSide.Left)
                ProcessCanvas(leftCanvas, m_leftRaycaster);
            else
                ProcessCanvas(rightCanvas, m_rightRaycaster);
        }

        m_clickHappened = false;
    }

    private void ProcessCanvas(Canvas canvas, GraphicRaycaster graphicsRaycaster)
    {
        var rect = canvas.pixelRect;

        var canvasCoordinates = rect.position + Vector2.Scale(rect.size, m_clickLocation.coords);

        var pointerEventData = new PointerEventData(m_eventSystem);
        pointerEventData.position = canvasCoordinates;

        List<RaycastResult> hits = new List<RaycastResult>();
        graphicsRaycaster.Raycast(pointerEventData, hits);

        foreach (RaycastResult hit in hits)
        {
            ExecuteEvents.Execute(hit.gameObject, pointerEventData, ExecuteEvents.pointerClickHandler);
        }
    }

    private void OnPageClick(PageCoordinates location)
    {
        m_clickLocation = location;
        m_clickHappened = true;
    }

    private bool m_clickHappened;
    private PageCoordinates m_clickLocation;
    private EventSystem m_eventSystem;

    private GraphicRaycaster m_leftRaycaster;
    private GraphicRaycaster m_rightRaycaster;

    /// <summary>
    /// If this is positive, then at least some code requires that this class
    /// ignores inputs.
    /// </summary>
    private int m_inputBlockCount = 0;
}
