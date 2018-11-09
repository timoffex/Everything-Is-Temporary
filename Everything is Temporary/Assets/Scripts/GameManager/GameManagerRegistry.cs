/*
 * This file contains Register() methods and properties for the GameManager class.
 * 
 * Most objects don't have to be registered, although that will help performance
 * by preventing a FindObjectOfType() call. The registry also selects the objects
 * with which the GameManager script interacts.
 * */

using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public partial class GameManager : MonoBehaviour
{
    public BookScript Book
    {
        get
        {
            if (m_book == null)
                m_book = FindOrDefault<BookScript>("No BookScript found. Creating default. Warning: this may cause errors.");

            return m_book;
        }
    }

    public Camera BookCamera
    {
        get
        {
            if (m_bookCamera == null)
                m_bookCamera = FindOrDefault<Camera>("Couldn't find a book camera.");
            return m_bookCamera;
        }
    }

    /// <summary>
    /// Registers the BookScript as the book object to be used in the game. If
    /// this returns false, that means that a book was already registered and
    /// was not changed.
    /// </summary>
    /// <returns><c>true</c>, if book was registered, <c>false</c> otherwise.</returns>
    /// <param name="book">The book.</param>
    public bool RegisterBook(BookScript book)
    {
        if (m_book == null || m_book == book)
        {
            m_book = book;
            return true;
        }
        else
        {
            Debug.LogWarning("Duplicate BookScript detected.");
            return false;
        }
    }

    /// <summary>
    /// Sets the book camera. This is the camera used to show the book.
    /// </summary>
    /// <returns><c>true</c>, if camera was registered, <c>false</c> otherwise.</returns>
    /// <param name="camera">The book camera.</param>
    public bool RegisterBookCamera(Camera camera)
    {
        if (m_bookCamera == null || m_bookCamera == camera)
        {
            m_bookCamera = camera;
            return true;
        }
        else
        {
            Debug.LogWarning("Duplicate book camera detected.");
            return false;
        }
    }

    /// <summary>
    /// Sets the MouseInput instance.
    /// </summary>
    /// <returns><c>true</c>, if mouse input was registered, <c>false</c> otherwise.</returns>
    /// <param name="mouseInput">Mouse input.</param>
    public bool RegisterMouseInput(MouseInput mouseInput)
    {
        if (m_mouseInput == null || m_mouseInput == mouseInput)
        {
            m_mouseInput = mouseInput;
            return true;
        }
        else
        {
            Debug.LogWarning("Duplicate MouseInput detected.");
            return false;
        }
    }

    /// <summary>
    /// Registers the town camera.
    /// </summary>
    /// <returns><c>true</c>, if town camera was registered, <c>false</c> otherwise.</returns>
    /// <param name="camera">Camera.</param>
    public bool RegisterTownCamera(Camera camera)
    {
        if (m_townCamera == null || m_townCamera == camera)
        {
            m_townCamera = camera;
            return true;
        }
        else
        {
            Debug.LogWarning("Duplicate 'additional camera' detected.");
            return false;
        }
    }

    /// <summary>
    /// Registers the town event system.
    /// </summary>
    /// <returns><c>true</c>, if town event system was registered, <c>false</c> otherwise.</returns>
    public bool RegisterTownEventSystem(EventSystem eventSystem)
    {
        if (m_townEventSystem == null || m_townEventSystem == eventSystem)
        {
            m_townEventSystem = eventSystem;
            return true;
        }
        else
        {
            Debug.LogWarning("Duplicate town event system detected.");
            return false;
        }

    }

    private T FindOrDefault<T>(string warningMessage) where T : Component
    {
        T obj = FindObjectOfType<T>();

        if (obj == null)
            return Default<T>(warningMessage);

        return obj;
    }

    private T Default<T>(string warningMessage) where T : Component
    {
        Debug.LogWarning(warningMessage);
        GameObject go = new GameObject();
        return go.AddComponent<T>();
    }

    private MouseInput MouseInputSingleton
    {
        get
        {
            if (m_mouseInput == null)
                m_mouseInput = FindOrDefault<MouseInput>("Couldn't find a MouseInput.");
            return m_mouseInput;
        }
    }

    private EventSystem TownEventSystem
    {
        get
        {
            if (m_townEventSystem == null)
                // Don't FindOrDefault() because that will pick up other event systems.
                m_townEventSystem = Default<EventSystem>("Couldn't find town event system.");
            return m_townEventSystem;
        }
    }

    private EventSystem BookEventSystem
    {
        get
        {
            if (m_bookEventSystem == null)
                m_bookEventSystem = FindOrDefault<EventSystem>("No BookEventSystem registered. Using default.");
            return m_bookEventSystem;
        }
    }

    private BookScript m_book;
    private Camera m_bookCamera;

    private MouseInput m_mouseInput;

    /// <summary>
    /// The main camera in the currently-loaded town scene.
    /// </summary>
    private Camera m_townCamera;

    /// <summary>
    /// The currently loaded town's input module.
    /// </summary>
    private EventSystem m_townEventSystem = null;

    private EventSystem m_bookEventSystem;
}
