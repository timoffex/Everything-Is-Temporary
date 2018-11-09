using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Animator))]
public class BookScript : MonoBehaviour
{
    [SerializeField]
    private AnimatedPageScript animatedPage = null;

    [SerializeField]
    private RenderTexture sampleRenderTexture = null;

    [SerializeField]
    private Material staticPageMaterial = null;

    [SerializeField]
    private Material animatedPageMaterial = null;

    [SerializeField]
    private Transform allPages = null;

    [SerializeField]
    private int initialPageIndex = 0;

    [SerializeField]
    private string bookDisplayedAnimBool = "Book Displayed";

    /// <summary>
    /// Whether the book is currently on the screen or is slid down. Setting this
    /// will animate the book sliding down or sliding up.
    /// </summary>
    public bool IsDisplayed
    {
        get
        {
            return m_isDisplayed;
        }

        set
        {
            m_animator.SetBool(bookDisplayedAnimBool, value);
            m_isDisplayed = value;

            if (!value)
                // Block mouse input to the book while it's down.
                m_isDisplayedInputBlocker = MainRaycastHelper.Singleton.BlockPageInput();
            else
            {
                if (m_isDisplayedInputBlocker != null)
                    // If the book is back up, allow mouse input again.
                    m_isDisplayedInputBlocker.Unsubscribe();
                m_isDisplayedInputBlocker = null;
            }
        }
    }

    private void Awake()
    {
        GameManager.Singleton.RegisterBook(this);
    }

    private void Start()
    {
        m_animator = GetComponent<Animator>();

        IsDisplayed = true;

        m_currentLeft = new RenderTexture(sampleRenderTexture);
        m_currentRight = new RenderTexture(sampleRenderTexture);
        m_bufferLeft = new RenderTexture(sampleRenderTexture);
        m_bufferRight = new RenderTexture(sampleRenderTexture);

        m_pageIndex = initialPageIndex;
        m_isFlipping = false;

        ParsePages();

        DisableAllPages();

        // Set up the initial page.
        m_pagePairs[m_pageIndex].SetCameraRenderTargets(m_currentLeft, m_currentRight);

        staticPageMaterial.SetTexture("_LeftPageImage", m_currentLeft);
        staticPageMaterial.SetTexture("_RightPageImage", m_currentRight);

        m_pagePairs[m_pageIndex].EnableCameras();
        m_pagePairs[m_pageIndex].EnableRaycasters();
    }


    /// <summary>
    /// Invokes TryFlipToPage() and ignores return value. This exists so that
    /// it can be invoked with UI elements.
    /// </summary>
    /// <param name="pageIndex">Page index.</param>
    public void FlipToPage(int pageIndex)
    {
        TryFlipToPage(pageIndex);
    }

    /// <summary>
    /// Attempts to flip to the given group of pages.
    /// </summary>
    /// <returns><c>true</c>, if flip will happen or if the book is already
    /// on the correct page, <c>false</c> otherwise.</returns>
    /// <param name="pageIndex">Page index.</param>
    public bool TryFlipToPage(int pageIndex)
    {
        // Can't flip to pages that don't exist.
        if (pageIndex < 0 || pageIndex > m_pagePairs.Count)
            return false;

        if (pageIndex > m_pageIndex)
            return FlipRightToLeft(pageIndex);
        else if (pageIndex < m_pageIndex)
            return FlipLeftToRight(pageIndex);
        else
            // True because the book is on the correct page.
            return true;
    }

    private bool FlipLeftToRight(int newIndex)
    {
        if (m_isFlipping)
            return false;

        m_isFlipping = true;
        IUnsubscriber inputBlocker = MainRaycastHelper.Singleton.BlockPageInput();

        m_pagePairs[m_pageIndex].DisableRaycasters();

        m_pagePairs[newIndex].SetCameraRenderTargets(m_bufferLeft, m_bufferRight);
        m_pagePairs[newIndex].EnableCameras();
        m_pagePairs[newIndex].EnableRaycasters();

        animatedPageMaterial.SetTexture("_LeftPageImage", m_currentLeft);
        animatedPageMaterial.SetTexture("_RightPageImage", m_bufferRight);

        // TODO: This should always return true. Check to make sure.
        animatedPage.FlipLeftToRight(() =>
        {
            FinishLeftToRightFlip(newIndex);
            inputBlocker.Unsubscribe();
        });

        // Do this after the animated page is made active.
        staticPageMaterial.SetTexture("_LeftPageImage", m_bufferLeft);

        return true;
    }

    private bool FlipRightToLeft(int newIndex)
    {
        if (m_isFlipping)
            return false;

        m_isFlipping = true;
        IUnsubscriber inputBlocker = MainRaycastHelper.Singleton.BlockPageInput();

        m_pagePairs[m_pageIndex].DisableRaycasters();

        m_pagePairs[newIndex].SetCameraRenderTargets(m_bufferLeft, m_bufferRight);
        m_pagePairs[newIndex].EnableCameras();
        m_pagePairs[newIndex].EnableRaycasters();

        animatedPageMaterial.SetTexture("_LeftPageImage", m_bufferLeft);
        animatedPageMaterial.SetTexture("_RightPageImage", m_currentRight);

        // TODO: This should always return true. Check to make sure.
        animatedPage.FlipRightToLeft(() =>
        {
            FinishRightToLeftFlip(newIndex);
            inputBlocker.Unsubscribe();
        });

        // Do this after the animated page is made active.
        staticPageMaterial.SetTexture("_RightPageImage", m_bufferRight);

        return true;
    }

    private void FinishLeftToRightFlip(int newIndex)
    {
        int oldIndex = m_pageIndex;
        m_pageIndex = newIndex;

        m_pagePairs[oldIndex].DisableCameras();

        staticPageMaterial.SetTexture("_RightPageImage", m_bufferRight);

        SwapRenderTextures();

        m_isFlipping = false;
    }

    private void FinishRightToLeftFlip(int newIndex)
    {
        int oldIndex = m_pageIndex;
        m_pageIndex = newIndex;

        m_pagePairs[oldIndex].DisableCameras();

        staticPageMaterial.SetTexture("_LeftPageImage", m_bufferLeft);

        SwapRenderTextures();

        m_isFlipping = false;
    }

    private void DisableAllPages()
    {
        for (int i = 0; i < m_pagePairs.Count; ++i)
        {
            m_pagePairs[i].DisableCameras();
            m_pagePairs[i].DisableRaycasters();
        }
    }

    /// <summary>
    /// Swaps m_current* with m_buffer*. All this does is swaps references.
    /// At the end of this method, the m_buffer* members refer to where
    /// m_current* members did previously, and vice versa.
    /// </summary>
    private void SwapRenderTextures()
    {
        RenderTexture temp;

        temp = m_currentLeft;
        m_currentLeft = m_bufferLeft;
        m_bufferLeft = temp;

        temp = m_currentRight;
        m_currentRight = m_bufferRight;
        m_bufferRight = temp;
    }

    /// <summary>
    /// Extracts cameras and canvases from the allPages object.
    /// </summary>
    private void ParsePages()
    {
        m_pagePairs = new List<PageScript>();

        foreach (Transform pageGroup in allPages)
        {
            var pageScript = pageGroup.GetComponent<PageScript>();

            if (pageScript != null)
                m_pagePairs.Add(pageScript);
            else
                Debug.LogWarningFormat("Object {0} (child of {1}) does not have a PageScript.",
                                       pageGroup.name, allPages.name);
        }
    }

    private RenderTexture m_currentLeft;
    private RenderTexture m_currentRight;
    private RenderTexture m_bufferLeft;
    private RenderTexture m_bufferRight;

    private bool m_isFlipping;

    private int m_pageIndex;

    private List<PageScript> m_pagePairs;

    private Animator m_animator;
    private bool m_isDisplayed;

    /// <summary>
    /// When m_isDisplayed is set to true, Unsubscribe() should be called
    /// on this to allow the book to receive mouse input again.
    /// </summary>
    private IUnsubscriber m_isDisplayedInputBlocker;
}
