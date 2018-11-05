using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInput : MonoBehaviour
{

    public SkinnedMeshRenderer leftPage;
    public SkinnedMeshRenderer rightPage;

    public string bookPageTag = "Book Page";

    public delegate void PageClickHandler(PageCoordinates location);
    public event PageClickHandler onPageClick;

    // Use this for initialization
    void Start()
    {
        m_camera = GetComponent<Camera>();

        m_leftPageMesh = new Mesh();
        m_rightPageMesh = new Mesh();

        leftPage.BakeMesh(m_leftPageMesh);
        rightPage.BakeMesh(m_rightPageMesh);

        // Rescale meshes.
        RescaleMesh(m_leftPageMesh, leftPage.transform);
        RescaleMesh(m_rightPageMesh, rightPage.transform);

        var leftCollider = leftPage.gameObject.AddComponent<MeshCollider>();
        leftCollider.sharedMesh = m_leftPageMesh;

        var rightCollider = rightPage.gameObject.AddComponent<MeshCollider>();
        rightCollider.sharedMesh = m_rightPageMesh;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Input.GetMouseButton(0))
            return;

        RaycastHit hit;
        if (!Physics.Raycast(m_camera.ScreenPointToRay(Input.mousePosition), out hit))
            return;


        if (hit.transform.CompareTag(bookPageTag))
        {
            PageCoordinates coords = ToPageCoordinates(hit.textureCoord);
            onPageClick(coords);
        }
    }

    private void RescaleMesh(Mesh mesh, Transform objectScale)
    {
        Vector3 inverseScale = objectScale.localScale;
        inverseScale.x = 1.0f / inverseScale.x;
        inverseScale.y = 1.0f / inverseScale.y;
        inverseScale.z = 1.0f / inverseScale.z;

        Vector3[] verts = mesh.vertices;

        for (int i = 0; i < verts.Length; ++i)
        {
            verts[i].Scale(inverseScale);
        }

        mesh.vertices = verts;
    }

    private PageCoordinates ToPageCoordinates(Vector2 uvBook)
    {
        Vector2 pageSpace = new Vector2(2 * uvBook.y, uvBook.x);

        if (pageSpace.x < 1)
            return new PageCoordinates(PageCoordinates.PageSide.Left, pageSpace);
        else
            return new PageCoordinates(PageCoordinates.PageSide.Right, pageSpace - Vector2.right);
    }

    private Camera m_camera;

    private Mesh m_leftPageMesh;
    private Mesh m_rightPageMesh;
}
