using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SidescrollQuadScript : MonoBehaviour {

    /// <summary>
    /// Gets the render texture that is being displayed on the quad.
    /// </summary>
    /// <returns>The render texture.</returns>
    public RenderTexture GetRenderTexture()
    {
        Debug.Assert(m_renderTexture != null);
        return m_renderTexture;
    }

    private void Awake()
    {
        GameManager.Singleton.RegisterSidescrollQuad(this);
    }

    [SerializeField]
    private RenderTexture m_renderTexture = null;
}
