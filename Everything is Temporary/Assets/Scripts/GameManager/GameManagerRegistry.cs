/*
 * This file contains Register() methods and properties for the GameManager class.
 * 
 * Most objects don't have to be registered, although that will help performance
 * by preventing a FindObjectOfType() call. The registry also selects the objects
 * with which the GameManager script interacts.
 * 
 * The most important reason for the existence of this is to register objects
 * that cannot otherwise be singletons (e.g. existing classes, like Camera).
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
            Debug.LogWarning("Duplicate town camera detected.");
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

    private BookScript m_book;
    private Camera m_bookCamera;

    /// <summary>
    /// The main camera in the currently-loaded town scene.
    /// </summary>
    private Camera m_townCamera;
}
