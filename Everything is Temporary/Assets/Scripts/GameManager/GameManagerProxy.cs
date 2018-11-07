using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Lightweight component that contains methods mimicking some GameManager
/// methods. This exists to allow UI elements in different scenes to interact
/// with the GameManager.
/// </summary>
public class GameManagerProxy : MonoBehaviour
{
    public void DisplayBookPage(int pageNumber)
    {
#pragma warning disable 4014
        GameManager.Singleton.DisplayBookPage(pageNumber);
#pragma warning restore 4014
    }

    public void SlideBookDown()
    {
        GameManager.Singleton.SlideBookDown();
    }

    public void LoadTown(string townScene)
    {
#pragma warning disable 4014
        GameManager.Singleton.LoadTown(townScene);
#pragma warning restore 4014
    }
}
