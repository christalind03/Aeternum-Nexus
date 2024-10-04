using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [SerializeField]
    private float _angle;

    [SerializeField]
    private float _distance;

    [SerializeField]
    private float _height;

    [SerializeField]
    private int _subdivisionFactor;

    [SerializeField]
    private Color _meshColor;

    private Mesh _mesh;

    private void OnDrawGizmos()
    {
        if (_mesh)
        {
            Gizmos.color = _meshColor;
            Gizmos.DrawMesh(_mesh, transform.position, transform.rotation);
        }
    }

    private void OnValidate()
    {
        _mesh = CreateMesh();
    }

    /// <summary>
    /// Dynamically create a vision cone mesh, represented as a wedge, using the input parameters given from the inspector.
    /// </summary>
    private Mesh CreateMesh()
    {
        Mesh wedgeMesh = new Mesh();

        // To create a FOV mesh in the shape of a vision cone (aka wedge), we need a minimum of 8 triangles.
        // This can be derived from the 3D shape of a wedge where we require 2 triangles for the top and bottom, and 3 rectangles for the sides.
        // Since two triangles make up a rectangle, we're given with 2 + 2(3) = 8 triangles for a wedge-shaped mesh.

        // In order to create a custom, rounded edge at the far end of the vision cone we must subdivide by a given number, resulting in multiple, smaller wedges.
        // As we do not want to render the sides of each subwedge, we only account for the far end and the top and bottom faces.
        // Using the logic provided above, this results in 1(2) + 2 = 4.
        // Then, we must render the sides, which results in 2(2) = 4.
        int numTriangles = (_subdivisionFactor * 4) + 4;
        int numVertices = numTriangles * 3; // Since each triangle has 3 verticies, we multiply by 3.

        int[] meshTriangles = new int[numVertices];
        Vector3[] meshVertices = new Vector3[numVertices];
        
        Vector3 bottomCenter = Vector3.zero;
        Vector3 bottomRight = Quaternion.Euler(0f, _angle, 0f) * Vector3.forward * _distance;
        Vector3 bottomLeft = Quaternion.Euler(0f, -_angle, 0f) * Vector3.forward * _distance;

        Vector3 topCenter = bottomCenter + Vector3.up * _height;
        Vector3 topRight = bottomRight + Vector3.up * _height;
        Vector3 topLeft = bottomLeft + Vector3.up * _height;

        // Create the vertex buffer for the dynamic mesh.
        // Triangles are to be sorted in a clockwise direction where the next triangle starts off at the previous triangle's endpoint to ensure the normals are pointing in the correct direction.
        int vertexIndex = 0;

        // Left Side
        meshVertices[vertexIndex++] = bottomCenter;
        meshVertices[vertexIndex++] = bottomLeft;
        meshVertices[vertexIndex++] = topLeft;

        meshVertices[vertexIndex++] = topLeft;
        meshVertices[vertexIndex++] = topCenter;
        meshVertices[vertexIndex++] = bottomCenter;

        // Right Side
        meshVertices[vertexIndex++] = bottomCenter;
        meshVertices[vertexIndex++] = topCenter;
        meshVertices[vertexIndex++] = topRight;

        meshVertices[vertexIndex++] = topRight;
        meshVertices[vertexIndex++] = bottomRight;
        meshVertices[vertexIndex++] = bottomCenter;

        // Wedge Segments
        float currentAngle = -_angle;
        float deltaAngle = (_angle * 2) / _subdivisionFactor; // Since we have to account for both the the left and right sides of the wedge segments from 0 degrees, we multiply by 2.

        for (int i = 0; i < _subdivisionFactor; ++i)
        {
            bottomRight = Quaternion.Euler(0f, currentAngle + deltaAngle, 0f) * Vector3.forward * _distance;
            bottomLeft = Quaternion.Euler(0f, currentAngle, 0f) * Vector3.forward * _distance;

            topRight = bottomRight + Vector3.up * _height;
            topLeft = bottomLeft + Vector3.up * _height;

            // Far Side
            meshVertices[vertexIndex++] = bottomLeft;
            meshVertices[vertexIndex++] = bottomRight;
            meshVertices[vertexIndex++] = topRight;

            meshVertices[vertexIndex++] = topRight;
            meshVertices[vertexIndex++] = topLeft;
            meshVertices[vertexIndex++] = bottomLeft;

            // Top
            meshVertices[vertexIndex++] = topCenter;
            meshVertices[vertexIndex++] = topLeft;
            meshVertices[vertexIndex++] = topRight;

            // Bottom
            meshVertices[vertexIndex++] = bottomCenter;
            meshVertices[vertexIndex++] = bottomRight;
            meshVertices[vertexIndex++] = bottomLeft;

            currentAngle += deltaAngle;
        }

        // Setup an index buffer to map each vertex to its corresponding index.
        for (int i = 0; i < numVertices; ++i)
        {
            meshTriangles[i] = i;
        }

        wedgeMesh.vertices = meshVertices;
        wedgeMesh.triangles = meshTriangles;
        wedgeMesh.RecalculateNormals();

        return wedgeMesh;
    }
}
