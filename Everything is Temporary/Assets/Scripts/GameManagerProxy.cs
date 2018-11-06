using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Lightweight component that contains methods mimicking the GameManager
/// methods. This exists to allow UI elements in different scenes to interact
/// with the GameManager.
/// </summary>
public class GameManagerProxy : MonoBehaviour
{
    public void DisplayBookPage(int pageNumber)
    {
        GameManager.Singleton.DisplayBookPage(pageNumber);
    }

    public void SlideBookDown()
    {
        GameManager.Singleton.SlideBookDown();
    }
}
