using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Contains helper methods for game-specific actions and game data. This script
/// has the final word on what is displayed, on how transitions happen, and
/// on what input methods are available (e.g. whether the book can receive
/// input at any time). Most scripts should interact with each other indirectly
/// via this class---this will make code more modular and easier to debug and
/// modify.
/// </summary>
/// <remarks>See also the GameManagerRegistry.cs file.</remarks>
public partial class GameManager : MonoBehaviour
{
	public Inventory inventory;
	
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
        if (_singleton == null || _singleton == this)
        {
            _singleton = this;
            m_stateHandler = new ShowingBookHandler(this);
        }
        else
        {
            Debug.LogWarning("Duplicate GameManager detected.");

            // Stop self.
            Destroy(gameObject);
        }
		
		// Create an inventory system.
		inventory = new Inventory();
		
		// Add some random items to inventory for testing.
		Item item0 = new Item("item0", 1);
		Item item1 = new Item("item1", 1);
		Item item2 = new Item("item2", 20);
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
