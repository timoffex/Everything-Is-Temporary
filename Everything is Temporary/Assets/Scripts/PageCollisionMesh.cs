using UnityEngine;
using System.Collections;

/// <summary>
/// Adds a collision mesh to a SkinnedMeshRenderer that is in a static pose.
/// </summary>
[RequireComponent(typeof(SkinnedMeshRenderer))]
public class PageCollisionMesh : MonoBehaviour
{
    private void Start()
    {
        SkinnedMeshRenderer pageRenderer = GetComponent<SkinnedMeshRenderer>();

        // Create meshes to check for collisions with static pages.
        Mesh pageMesh = new Mesh();

        pageRenderer.BakeMesh(pageMesh);

        // Rescale mesh so that it is properly positioned.
        RescaleMesh(pageMesh, transform);

        var meshCollider = gameObject.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = pageMesh;
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
}
