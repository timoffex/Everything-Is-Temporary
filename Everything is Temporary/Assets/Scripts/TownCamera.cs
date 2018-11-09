using UnityEngine;
using System.Collections;

/// <summary>
/// Represents the main camera in a town scene. Notifies the GameManager
/// of its existence. Automatically disables the camera component (if the
/// registering went through correctly).
/// </summary>
[RequireComponent(typeof(Camera))]
public class TownCamera : MonoBehaviour
{
    public void SetRenderTarget(RenderTexture renderTexture)
    {
        m_camera.targetTexture = renderTexture;
    }

    public void EnableCamera()
    {
        m_camera.enabled = true;
    }

    public void DisableCamera()
    {
        m_camera.enabled = false;
    }

    public static implicit operator Camera(TownCamera cam)
    {
        return cam.m_camera;
    }

    private void Awake()
    {
        m_camera = GetComponent<Camera>();
        m_camera.enabled = false;

        GameManager.Singleton.RegisterTownCamera(this);
    }

    private Camera m_camera;
}
