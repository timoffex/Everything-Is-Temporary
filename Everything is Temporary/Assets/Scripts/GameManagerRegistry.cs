/*
 * This file contains Register() methods and properties for the GameManager class.
 * */

using UnityEngine;
using System.Collections;

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

    /// <summary>
    /// Registers the BookScript as the book object to be used in the game. If
    /// this returns false, that means that a book was already registered and
    /// was not changed.
    /// </summary>
    /// <returns><c>true</c>, if book was registered, <c>false</c> otherwise.</returns>
    /// <param name="book">The book.</param>
    public bool RegisterBook(BookScript book)
    {
        if (m_book == null)
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

    private T FindOrDefault<T>(string warningMessage) where T : MonoBehaviour
    {
        T obj = FindObjectOfType<T>();

        if (obj == null)
        {
            Debug.LogWarning(warningMessage);
            GameObject go = new GameObject();
            return go.AddComponent<T>();
        }

        return obj;
    }

    private BookScript m_book;
}
