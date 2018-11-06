using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains helper methods for game-specific actions and also contains game
/// data (e.g. a Player instance; TODO).
/// 
/// See also the GameManagerRegistry.cs file.
/// </summary>
public partial class GameManager : MonoBehaviour
{
    /// <summary>
    /// The GameManager instance for the current game.
    /// </summary>
    /// <value>The GameManager instance.</value>
    public static GameManager Singleton
    {
        get
        {
            if (_singleton == null)
            {
                _singleton = FindObjectOfType<GameManager>();

                if (_singleton == null)
                {
                    Debug.LogWarning("GameManager singleton not found. The GameManager scene might be missing.");

                    // Create a default GameManager to avoid null reference errors.
                    GameObject go = new GameObject();
                    _singleton = go.AddComponent<GameManager>();
                }
            }

            return _singleton;
        }
    }

    void Awake()
    {
        if (_singleton == null)
        {
            _singleton = this;
        }
        else if (_singleton != this)
        {
            Debug.LogWarning("Duplicate GameManager detected.");

            // Stop self.
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Displays the given pair of pages in the book. This will slide the book up
    /// if it was previously down.
    /// </summary>
    /// <param name="pageNumber">Page number.</param>
    public void DisplayBookPage(int pageNumber)
    {
        if (!Book.IsDisplayed)
            Book.IsDisplayed = true;

        Book.FlipToPage(pageNumber);
    }

    /// <summary>
    /// Slides the book down.
    /// </summary>
    public void SlideBookDown()
    {
        Book.IsDisplayed = false;
    }

    /// <summary>
    /// Slides the book up.
    /// </summary>
    public void SlideBookUp()
    {
        Book.IsDisplayed = true;
    }

    private static GameManager _singleton;
}
