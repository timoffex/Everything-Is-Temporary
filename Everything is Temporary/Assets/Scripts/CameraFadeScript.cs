using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFadeScript : MonoBehaviour
{
    /// <summary>
    /// Whether a fade effect is currently in progress.
    /// </summary>
    public bool IsFading
    {
        get
        {
            return m_isFading;
        }
    }

    /// <summary>
    /// The current fade value. Between 0 and 1, with 0 meaning "don't change
    /// image at all" and 1 meaning "replace image by fade color."
    /// </summary>
    /// <value>The fade value.</value>
    public float FadeValue
    {
        get
        {
            return m_fade;
        }
    }


    /// <summary>
    /// Begins the fading process if IsFading is false.
    /// </summary>
    /// <returns><c>true</c>, if fade was begun, <c>false</c> otherwise.</returns>
    /// <param name="initial">Initial fade value. Should be between 0 (no fade) and 1 (full fade).</param>
    /// <param name="final">Final fade value. Should be between 0 (no fade) and 1 (full fade).</param>
    /// <param name="color">Fade color.</param>
    /// <param name="time">Total time in seconds to fade.</param>
    /// <param name="endAction">Function to call once immediately when fade finishes.</param>
    /// <param name="cancelAction">Function to call if the fade is cancelled before
    /// it finishes. In this case, endAction is not invoked.</param>
    public bool BeginFade(float initial, float final, Color color, float time,
                          System.Action endAction = null,
                          System.Action cancelAction = null)
    {
        if (m_isFading)
            return false;
        else
        {
            m_initialFade = initial;
            m_fade = m_initialFade;
            m_finalFade = final;
            m_startTime = Time.fixedTime;
            m_fadeTime = time;
            m_fadeColor = color;

            m_endAction = endAction;
            m_cancelAction = cancelAction;

            m_material.SetColor("_Color", m_fadeColor);

            m_isFading = true;

            return true;
        }
    }

    /// <summary>
    /// Cancels the previous fading process, if any, and begins a new one.
    /// </summary>
    /// <param name="initial">Initial fade value. Should be between 0 (no fade) and 1 (full fade).</param>
    /// <param name="final">Final fade value. Should be between 0 (no fade) and 1 (full fade).</param>
    /// <param name="color">Fade color.</param>
    /// <param name="time">Total time in seconds to fade.</param>
    /// <param name="endAction">Function to call once immediately when fade finishes.</param>
    /// <param name="cancelAction">Function to call if the fade is cancelled before
    /// it finishes. In this case, endAction is not invoked.</param>
    public void CancelAndBeginNewFade(float initial, float final, Color color, float time,
                                      System.Action endAction = null,
                                      System.Action cancelAction = null)
    {
        if (m_isFading && m_cancelAction != null)
        {
            m_cancelAction();
            m_cancelAction = null;
        }

        m_isFading = false;

        bool success = BeginFade(initial, final, color, time, endAction, cancelAction);

        // The above must succeed.
        Debug.Assert(success);
    }

    private void Awake()
    {
        Shader shader = Shader.Find("Hidden/FadeShader");
        m_material = new Material(shader);

        m_isFading = false;
    }

    private void FixedUpdate()
    {
        if (!m_isFading)
            return;

        float deltaTime = Time.fixedTime - m_startTime;

        m_fade = Mathf.Lerp(m_initialFade, m_finalFade, deltaTime / m_fadeTime);

        if (deltaTime > m_fadeTime)
        {
            m_isFading = false;

            if (m_endAction != null)
            {
                m_endAction();
                m_endAction = null;
            }
        }

    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        m_material.SetFloat("_FadeValue", m_fade);
        Graphics.Blit(source, destination, m_material);
    }

    private bool m_isFading;

    private float m_fade;
    private float m_initialFade;
    private float m_finalFade;
    private float m_startTime;
    private float m_fadeTime;
    private Color m_fadeColor;
    private System.Action m_endAction;
    private System.Action m_cancelAction;

    private Material m_material;
}
