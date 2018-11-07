using UnityEngine;
using System.Collections;

/// <summary>
/// Represents the main camera in a town scene. Notifies the GameManager
/// of its existence. Automatically disables the camera component.
/// </summary>
[RequireComponent(typeof(Camera))]
public class TownCamera : MonoBehaviour
{
    private void Awake()
    {
        var cam = GetComponent<Camera>();
        GameManager.Singleton.RegisterTownCamera(cam);
        cam.enabled = false;
    }
}
