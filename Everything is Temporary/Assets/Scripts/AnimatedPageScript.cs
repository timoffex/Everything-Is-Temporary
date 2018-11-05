using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedPageScript : MonoBehaviour
{
    public string triggerLeftToRight = "FlipL2R";
    public string triggerRightToLeft = "FlipR2L";

    // Use this for initialization
    void Start()
    {
        m_animator = GetComponent<Animator>();

        m_triggerL2R = Animator.StringToHash(triggerLeftToRight);
        m_triggerR2L = Animator.StringToHash(triggerRightToLeft);

        m_pageReady = true;

        gameObject.SetActive(false);
    }

    public bool FlipLeftToRight(System.Action onFinish)
    {
        if (m_pageReady)
        {
            m_pageReady = false;
            m_onFinish = onFinish;
            gameObject.SetActive(true);
            m_animator.SetTrigger(m_triggerL2R);
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool FlipRightToLeft(System.Action onFinish)
    {
        if (m_pageReady)
        {
            m_pageReady = false;
            m_onFinish = onFinish;
            gameObject.SetActive(true);
            m_animator.SetTrigger(m_triggerR2L);
            return true;
        }
        else
        {
            return false;
        }
    }

    private void PageReady()
    {
        if (m_onFinish != null)
            m_onFinish();

        m_pageReady = true;
        gameObject.SetActive(false);
    }

    private Animator m_animator;
    private int m_triggerL2R;
    private int m_triggerR2L;
    private bool m_pageReady;
    private System.Action m_onFinish;
}
