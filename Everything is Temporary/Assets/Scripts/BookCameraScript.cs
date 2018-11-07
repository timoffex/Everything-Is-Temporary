using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class BookCameraScript : MonoBehaviour
{
    private void Awake()
    {
        GameManager.Singleton.RegisterBookCamera(GetComponent<Camera>());
    }
}
