using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Class that helps with raycasting from the main view. Used by PageRaycaster
/// and SidescrollRaycaster.
/// </summary>
[RequireComponent(typeof(Camera))]
public class MainRaycastHelper : MonoBehaviour
{
    public static MainRaycastHelper Singleton
    {
        get
        {
            if (_singleton == null)
            {
                _singleton = FindObjectOfType<MainRaycastHelper>();

                if (_singleton == null)
                {
                    GameObject go = new GameObject();
                    _singleton = go.AddComponent<MainRaycastHelper>();
                    _singleton.m_camera = GameManager.Singleton.BookCamera;
                }
            }

            return _singleton;
        }
    }

    /// <summary>
    /// Blocks input to the book pages. Returns a token that represents the
    /// block request; call Unsubscribe() when blocking is no longer necessary.
    /// As long as there is at least one blocking token still in existence,
    /// mouse input to the book will be blocked.
    /// </summary>
    /// <returns>The page input.</returns>
    public IUnsubscriber BlockPageInput()
    {
        m_pageBlockerCount++;

        return new ActionUnsubscriber(() => m_pageBlockerCount--);
    }

    public PageCoordinates? RaycastBook(Vector2 mousePosition)
    {
        if (m_pageBlockerCount > 0)
            return null;

        if (m_prevBookCasts.ContainsKey(mousePosition))
            return m_prevBookCasts[mousePosition];

        PageCoordinates? result = null;

        RaycastHit hit;
        if (Physics.Raycast(m_camera.ScreenPointToRay(mousePosition), out hit))
        {
            if (hit.transform.CompareTag("Book Page"))
                result = ToPageCoordinates(hit.textureCoord);
        }

        m_prevBookCasts[mousePosition] = result;
        return result;
    }

    public PageCoordinates? RaycastLeftPage(Vector2 mousePosition)
    {
        var coords = RaycastBook(mousePosition);
        if (!coords.HasValue)
            return null;
        else if (coords.Value.side == PageCoordinates.PageSide.Left)
            return coords;
        else
            return null;
    }

    public PageCoordinates? RaycastRightPage(Vector2 mousePosition)
    {
        var coords = RaycastBook(mousePosition);
        if (!coords.HasValue)
            return null;
        else if (coords.Value.side == PageCoordinates.PageSide.Right)
            return coords;
        else
            return null;
    }

    public Vector2? RaycastScrollview(Vector2 mousePosition)
    {
        if (m_prevScrollviewCasts.ContainsKey(mousePosition))
            return m_prevScrollviewCasts[mousePosition];

        Vector2? result = null;

        RaycastHit hit;
        if (Physics.Raycast(m_camera.ScreenPointToRay(mousePosition), out hit))
        {
            if (hit.transform.CompareTag("Sidescroll View"))
                result = hit.textureCoord;
        }

        m_prevScrollviewCasts[mousePosition] = result;
        return result;
    }

    private PageCoordinates ToPageCoordinates(Vector2 uvBook)
    {
        Vector2 pageSpace = new Vector2(2 * uvBook.y, uvBook.x);

        if (pageSpace.x < 1)
            return new PageCoordinates(PageCoordinates.PageSide.Left, pageSpace);
        else
            return new PageCoordinates(PageCoordinates.PageSide.Right, pageSpace - Vector2.right);
    }

    private void Awake()
    {
        if (m_camera == null)
            m_camera = GetComponent<Camera>();

        m_prevBookCasts = new Dictionary<Vector2, PageCoordinates?>();
        m_prevScrollviewCasts = new Dictionary<Vector2, Vector2?>();
    }

    private void FixedUpdate()
    {
        m_prevBookCasts.Clear();
        m_prevScrollviewCasts.Clear();
    }

    private Camera m_camera = null;

    private static MainRaycastHelper _singleton;

    private Dictionary<Vector2, PageCoordinates?> m_prevBookCasts;
    private Dictionary<Vector2, Vector2?> m_prevScrollviewCasts;

    private int m_pageBlockerCount = 0;
}
