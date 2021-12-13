using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertex
{
    int index;
    Vector3 vertex;
}
public class HalfEdge
{
    int index;
    Vertex sourceVertex;
    HalfEdge prevEdge;
    HalfEdge nextEdge;
    HalfEdge twinEdge;
    Face face;
}
public class Face
{
    int index;
    HalfEdge initVertex;
}

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    delegate Vector3 ComputePositionFromKxKz(float kX, float kZ);

    MeshFilter m_Mf;

    private void Awake()
    {
        m_Mf = GetComponent<MeshFilter>();
        //m_Mf.sharedMesh = CreateTriangle();
        //m_Mf.sharedMesh = CreateQuadXZ(new Vector3(4,0,2));
        //m_Mf.sharedMesh = CreateStripXZ(new Vector3(4, 0, 2),200);

        //m_Mf.sharedMesh = CreatePlaneXZ(new Vector3(4, 0, 2), 20,10);
        //m_Mf.sharedMesh = WrapNormalizePlane( 20, 10, (kX,kZ)=>new Vector3((kX-0.5f)*4,0,(kZ-0.5f)*2) );
        /* m_Mf.sharedMesh = WrapNormalizePlane(20, 100,
        (kX, kZ) =>{
        float rho = 2*(1+.25f*Mathf.Sin(kZ*Mathf.PI*2*4));
        float theta = kX * 2 * Mathf.PI;
        float y = kZ * 4;
        return new Vector3(rho*Mathf.Cos(theta),y, rho * Mathf.Sin(theta));
        }
        );
        */
        m_Mf.sharedMesh = WrapNormalizePlane(200, 100,
        (kX, kZ) => {
            float rho = 2 * (1 + .25f * Mathf.Sin(kZ * Mathf.PI * 2 * 4));
            float theta = kX * 2 * Mathf.PI;
            float phi = (1 - kZ) * Mathf.PI;
            return rho * new Vector3(Mathf.Cos(theta) * Mathf.Sin(phi), Mathf.Cos(phi), Mathf.Sin(theta) * Mathf.Sin(phi));
        }
        );


        gameObject.AddComponent<MeshCollider>();
    }

    Mesh CreateTriangle()
    {
        Mesh newMesh = new Mesh();
        newMesh.name = "triangle";

        Vector3[] vertices = new Vector3[3];
        int[] triangles = new int[1 * 3];

        vertices[0] = Vector3.right;
        vertices[1] = Vector3.up;
        vertices[2] = Vector3.forward;

        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;

        newMesh.vertices = vertices;
        newMesh.triangles = triangles;
        newMesh.RecalculateBounds();
        return newMesh;
    }
    Mesh CreateQuadXZ(Vector3 size)
    {
        Vector3 halfSize = size * .5f;

        Mesh newMesh = new Mesh();
        newMesh.name = "quad";

        Vector3[] vertices = new Vector3[4];
        int[] triangles = new int[2 * 3];

        vertices[0] = new Vector3(-halfSize.x, 0, -halfSize.z);
        vertices[1] = new Vector3(-halfSize.x, 0, halfSize.z);
        vertices[2] = new Vector3(halfSize.x, 0, halfSize.z);
        vertices[3] = new Vector3(halfSize.x, 0, -halfSize.z);

        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;

        triangles[3] = 0;
        triangles[4] = 2;
        triangles[5] = 3;

        newMesh.vertices = vertices;
        newMesh.triangles = triangles;
        newMesh.RecalculateBounds();
        return newMesh;
    }

    Mesh CreateStripXZ(Vector3 size, int nSegments)
    {
        Vector3 halfSize = size * .5f;

        Mesh newMesh = new Mesh();
        newMesh.name = "strip";

        Vector3[] vertices = new Vector3[(nSegments + 1) * 2];
        int[] triangles = new int[nSegments * 2 * 3];

        //Vertices
        for (int i = 0; i < nSegments + 1; i++)
        {
            float k = (float)i / nSegments;
            float y = .25f * Mathf.Sin(k * Mathf.PI * 2 * 3);
            vertices[i] = new Vector3(Mathf.Lerp(-halfSize.x, halfSize.x, k), y, -halfSize.z);
            vertices[nSegments + 1 + i] = new Vector3(Mathf.Lerp(-halfSize.x, halfSize.x, k), y, halfSize.z);
        }

        //Triangles
        int index = 0;
        for (int i = 0; i < nSegments; i++)
        {
            triangles[index++] = i;
            triangles[index++] = i + nSegments + 1;
            triangles[index++] = i + nSegments + 2;

            triangles[index++] = i;
            triangles[index++] = i + nSegments + 2;
            triangles[index++] = i + 1;
        }

        newMesh.vertices = vertices;
        newMesh.triangles = triangles;
        newMesh.RecalculateBounds();
        return newMesh;
    }

    Mesh CreatePlaneXZ(Vector3 size, int nSegmentsX, int nSegmentsZ)
    {
        Vector3 halfSize = size * .5f;

        Mesh newMesh = new Mesh();
        newMesh.name = "plane";

        Vector3[] vertices = new Vector3[(nSegmentsX + 1) * (nSegmentsZ + 1)];
        int[] triangles = new int[nSegmentsX * nSegmentsZ * 2 * 3];

        //Vertices
        int index = 0;
        for (int i = 0; i < nSegmentsX + 1; i++)
        {
            float kX = (float)i / nSegmentsX;
            float x = Mathf.Lerp(-halfSize.x, halfSize.x, kX);
            for (int j = 0; j < nSegmentsZ + 1; j++)
            {
                float kZ = (float)j / nSegmentsZ;
                vertices[index++] = new Vector3(x, 0, Mathf.Lerp(-halfSize.z, halfSize.z, kZ));
            }
        }

        //Triangles
        index = 0;
        //double boucle également
        for (int i = 0; i < nSegmentsX; i++)
        {
            for (int j = 0; j < nSegmentsZ; j++)
            {
                triangles[index++] = i * (nSegmentsZ + 1) + j;
                triangles[index++] = i * (nSegmentsZ + 1) + j + 1;
                triangles[index++] = (i + 1) * (nSegmentsZ + 1) + j + 1;

                triangles[index++] = i * (nSegmentsZ + 1) + j;
                triangles[index++] = (i + 1) * (nSegmentsZ + 1) + j + 1;
                triangles[index++] = (i + 1) * (nSegmentsZ + 1) + j;
            }
        }

        newMesh.vertices = vertices;
        newMesh.triangles = triangles;
        newMesh.RecalculateBounds();
        newMesh.RecalculateNormals();
        return newMesh;
    }

    Mesh WrapNormalizePlane(int nSegmentsX, int nSegmentsZ, ComputePositionFromKxKz computePosition)
    {
        Mesh newMesh = new Mesh();
        newMesh.name = "plane";
        Vector3[] vertices = new Vector3[(nSegmentsX + 1) * (nSegmentsZ + 1)];
        int[] triangles = new int[nSegmentsX * nSegmentsZ * 2 * 3];
        //Vertices
        int index = 0;
        for (int i = 0; i < nSegmentsX + 1; i++)
        {
            float kX = (float)i / nSegmentsX;
            for (int j = 0; j < nSegmentsZ + 1; j++)
            {
                float kZ = (float)j / nSegmentsZ;
                vertices[index++] = computePosition(kX, kZ);
            }
        }

        //Triangles
        index = 0;
        //double boucle également
        for (int i = 0; i < nSegmentsX; i++)
        {
            for (int j = 0; j < nSegmentsZ; j++)
            {
                triangles[index++] = i * (nSegmentsZ + 1) + j;
                triangles[index++] = i * (nSegmentsZ + 1) + j + 1;
                triangles[index++] = (i + 1) * (nSegmentsZ + 1) + j + 1;

                triangles[index++] = i * (nSegmentsZ + 1) + j;
                triangles[index++] = (i + 1) * (nSegmentsZ + 1) + j + 1;
                triangles[index++] = (i + 1) * (nSegmentsZ + 1) + j;


            }
        }

        newMesh.vertices = vertices;
        newMesh.triangles = triangles;
        newMesh.RecalculateBounds();
        newMesh.RecalculateNormals();
        return newMesh;
    }
}