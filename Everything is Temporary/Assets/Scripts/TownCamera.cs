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

    public static implicit operator Camera(TownCamera cam)
    {
        return cam.m_camera;
    }

    private void Awake()
    {
        m_camera = GetComponent<Camera>();
        m_camera.enabled = false;

        GameManager.Singleton.RegisterTownCamera(this);
		
		// Center camera on player.
		transform.position = new Vector3(player.transform.position.x, player.transform.position.y, m_cameraZ);
		
		StartCoroutine("CatchUpWithPlayer");
    }
	
	private void Update()
	{

	}
	
	private IEnumerator CatchUpWithPlayer()
	{
		while (true)
		{
			Vector3 playerPos = player.transform.position;
			Vector3 cameraPos = transform.position;
			
			float xOffset = playerPos.x - cameraPos.x;
			
			if (xOffset > m_maxOffset)
			{
				transform.position = new Vector3(playerPos.x - m_maxOffset, cameraPos.y, cameraPos.z);
			}
			else if (-xOffset > m_maxOffset)
			{
				transform.position = new Vector3(playerPos.x + m_maxOffset, cameraPos.y, cameraPos.z);
			}
			
			yield return null;
		}
	}

    private Camera m_camera;
	
	private const float m_cameraZ = -1f;
	private const float m_maxOffset = 0.3f;
}
