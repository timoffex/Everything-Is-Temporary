using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Represents the main camera in a town scene. Notifies the GameManager
/// of its existence. Automatically disables the camera component. The
/// camera will be re-enabled by the GameManager as necessary.
/// </summary>
[RequireComponent(typeof(Camera))]
public class TownCamera : MonoBehaviour
{
    public GameObject player;

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

    /// <summary>
    /// Lets the TownCamera be implicitly converted to a Camera.
    /// </summary>
    public static implicit operator Camera(TownCamera cam)
    {
        return cam.m_camera;
    }

    /// <summary>
    /// Grabs the camera focus. The camera will follow the given transform.
    /// Call Unsubscribe() to release the focus.
    /// </summary>
    /// <returns>The camera focus.</returns>
    /// <param name="targetPos">Target position.</param>
    public IUnsubscriber GrabCameraFocus(Transform targetPos)
    {
        m_cameraFocusStack.Add(targetPos);

        return new ListUnsubscriber<Transform>(m_cameraFocusStack, targetPos);
    }

    private void Awake()
    {
        m_camera = GetComponent<Camera>();
        m_camera.enabled = false;

        m_cameraFocusStack = new List<Transform>();

        GameManager.Singleton.RegisterTownCamera(this);
    }

    private void Start()
    {
        // Center camera on player.
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, m_cameraZ);

        StartCoroutine(CatchUpWithPlayer());
    }

    private IEnumerator CatchUpWithPlayer()
    {
        while (true)
        {
            Vector3 targetPos = GetTargetPosition();

            // Don't change z coordinate.
            targetPos.z = transform.position.z;

            // Move toward targetPos smoothly.
            transform.position = Vector3.Lerp(transform.position, targetPos, 0.2f);

            yield return null;
        }
    }

    private Vector3 GetTargetPosition()
    {
        if (m_cameraFocusStack.Count > 0)
        {
            // When focusing on something that's not the player, try to center
            // over it perfectly.
            return m_cameraFocusStack[m_cameraFocusStack.Count - 1].position;
        }
        else
        {
            // In this case, we want to focus on the player. When we follow
            // the player, we want some wiggle room: if the player is near the
            // center of the screen and moves a little bit horizontally, the
            // camera should not move until the player passes some threshold.
            Vector3 pos = player.transform.position;

            float cameraX = transform.position.x;

            if (cameraX < pos.x - m_maxOffset)
            {
                pos.x -= m_maxOffset;
            }
            else if (cameraX > pos.x + m_maxOffset)
            {
                pos.x += m_maxOffset; 
            }
            else
            {
                pos.x = cameraX;
            }

            return pos;
        }
    }

    private Camera m_camera;

    private const float m_cameraZ = -1f;
    private const float m_maxOffset = 0.3f;

    /// <summary>
    /// The last entry in this list should be focused on. If the list
    /// is empty, the camera should focus on the player.
    /// </summary>
    private List<Transform> m_cameraFocusStack;
}
