using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator
{
    public delegate Vector3 ComputeVector3FromKxKz(float kX, float kZ);
    //private MeshFilter m_Mf;

    void Awake() // N'a plus d'utilité après retrait de l'héritage de Monobehaviour, gardé pour les exemples de Mesh
    {
        //m_Mf.sharedMesh = CreateTriangle();
        //m_Mf.sharedMesh = CreateQuadXZ(new Vector3(4, 0, 2));
        //m_Mf.sharedMesh = CreateStripXZ(new Vector3(4, 0, 2), 8);
        //m_Mf.sharedMesh = CreatePlaneXZ(new Vector3(4, 0, 2), 8, 3);

        //m_Mf.sharedMesh = WrapNormalizedPlane(20, 10, (kX, kZ) => new Vector3(kX, 0, kZ));

        /*m_Mf.sharedMesh = WrapNormalizedPlane(20, 10,
            (kX, kZ) => {
                //Cylindre
                float theta = kX * 2 * Mathf.PI;
                float z = 4 * kZ;
                float rho = 2;
                return new Vector3(rho * Mathf.Cos(theta), z, rho * Mathf.Sin(theta));
            }
        );*/

        /*m_Mf.sharedMesh = WrapNormalizedPlane(20, 10,
            (kX, kZ) => {
                //Sphere
                float theta = kX * 2 * Mathf.PI;
                float phi = (1 - kZ) * Mathf.PI;
                float rho = 2;
                return new Vector3(rho * Mathf.Cos(theta) * Mathf.Sin(phi), rho * Mathf.Cos(phi), rho * Mathf.Sin(theta) * Mathf.Sin(phi));
            }
        );*/

        /*m_Mf.sharedMesh = WrapNormalizedPlaneQuads(20, 10,
            (kX, kZ) => {
                //Sphere quads
                float theta = kX * 2 * Mathf.PI;
                float phi = (1 - kZ) * Mathf.PI;
                float rho = 2;
                return new Vector3(rho * Mathf.Cos(theta) * Mathf.Sin(phi), rho * Mathf.Cos(phi), rho * Mathf.Sin(theta) * Mathf.Sin(phi));
            }
        );*/

        //m_Mf.sharedMesh = WrapNormalizedPlaneQuads(20, 10, (kX, kZ) => new Vector3(kX, 0, kZ));
        
        //m_Mf.sharedMesh = WrapNormalizedPlaneQuads(20, 10, (kX, kZ) => new Vector3(kX, Mathf.Sin(kZ), kZ));

        /*m_Mf.sharedMesh = WrapNormalizedPlane(20, 10,
            (kX, kZ) => {
                //Sphere
                float theta = kX * 2 * Mathf.PI;
                float phi = (1 - kZ) * Mathf.PI;
                float rho = 2;
                return new Vector3(rho * Mathf.Cos(theta) * Mathf.Sin(phi), rho * Mathf.Cos(phi), rho * Mathf.Sin(theta) * Mathf.Sin(phi));
            }
        );*/

        //m_Mf.sharedMesh = CreateRegularPolygon(5, 2f);

        //m_Mf.sharedMesh = createTruc();

        //gameObject.AddComponent<MeshCollider>();
    }

    public Mesh CreateTriangle()
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

    public Mesh CreateQuadXZ(Vector3 size)
    {
        // Crée un quad centré sur l'origine selon les axes X,Z
        Vector3 halfSize = size / 2;

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

    public Mesh CreateStripXZ(Vector3 size, int nSegments)
    {
        // Crée un quad centré sur l'origine selon les axes X,Z
        Vector3 halfSize = size / 2;
        float stepX = size.x / nSegments;

        Mesh newMesh = new Mesh();
        newMesh.name = "strip";

        int nVertices = (nSegments + 1) * 2;
        Vector3[] vertices = new Vector3[nVertices];
        int[] triangles = new int[nSegments * 2 * 3];

        //Remplissage vertices
        float posX = -halfSize.z;
        for(int i=0; i < nSegments+1; i++)
        {
            vertices[i] = new Vector3(posX + stepX * i, 0, halfSize.z);
            vertices[i + nSegments + 1] = new Vector3(posX + stepX * i, 0, -halfSize.z);
        }

        //Triangles
        //Debug.Log("Nb cases triangles: " + nSegments*3*2);
        for (int i = 0; i < nSegments; i++)
        {
            int posTriangle = i * 3;
            triangles[posTriangle] = i;
            triangles[posTriangle+1] = i+1;
            triangles[posTriangle+2] = nVertices / 2 + i;

            int posTriangleInverse = (nSegments + i) * 3;
            triangles[posTriangleInverse] = nSegments + 1 + i;
            triangles[posTriangleInverse + 1] = i + 1;
            triangles[posTriangleInverse + 2] = nSegments + 2 + i;
            //Debug.Log("i: " + i + " / " + (nSegments + i) + " " + i + " " + (nSegments + 1 + i));
        }

        newMesh.vertices = vertices;
        newMesh.triangles = triangles;
        newMesh.RecalculateBounds();

        return newMesh;
    }

    public Mesh CreatePlaneXZ(Vector3 size, int nSegmentsX, int nSegmentsZ)
    {
        // Crée un quad centré sur l'origine selon les axes X,Z
        Vector3 halfSize = size / 2;

        Mesh newMesh = new Mesh();
        newMesh.name = "plane";

        int nbVertices = (nSegmentsX + 1) * (nSegmentsZ + 1); 
        Vector3[] vertices = new Vector3[nbVertices];
        int[] triangles = new int[nSegmentsZ*nSegmentsX*2*3];

        //Remplissage vertices
        float startX = -halfSize.x;
        float startZ = -halfSize.z;
        float stepX = size.x / nSegmentsX;
        float stepZ = size.z / nSegmentsZ;
        for (int i = 0; i < nSegmentsZ + 1; i++)
        {
            for (int j = 0; j < nSegmentsX + 1; j++)
            {
                vertices[i*(nSegmentsX+1) + j] = new Vector3(startX + stepX * j, 0, startZ + stepZ * i);
                //Debug.Log("Indice: " + (i * (nSegmentsX + 1) + j) + "  Vertice: " + vertices[i * (nSegmentsX + 1) + j]);
            }
        }

        //Triangles
        for (int i = 0; i < nSegmentsZ; i++)
        {
            int indexOffset = i * (nSegmentsX + 1);
            for (int j = 0; j < nSegmentsX; j++)
            {
                int posTriangle = (i*nSegmentsX + j) * 3;
                triangles[posTriangle] = indexOffset + j;
                triangles[posTriangle + 1] = (i+1) * (nSegmentsX + 1) + j;//(i * (nSegmentsX + 1)) + (nbVertices / (nSegmentsZ+1) + j);
                triangles[posTriangle + 2] = indexOffset + j + 1;
                //Debug.Log("Indice: " + posTriangle + " / " + (i * (nSegmentsX + 1) + j) + " " + (i * (nSegmentsX + 1) + j + 1) + " " + ((i * (nSegmentsX + 1)) + (nbVertices / (nSegmentsZ + 1) + j)));

                int posTriangleInverse = (triangles.Length / 2) + (i * nSegmentsX + j) * 3;
                triangles[posTriangleInverse] = indexOffset + j + 1;
                triangles[posTriangleInverse + 1] = (i+1) * (nSegmentsX + 1) + j;
                triangles[posTriangleInverse + 2] = (i+1) * (nSegmentsX + 1) + j + 1;
                //Debug.Log("Indice: " + posTriangleInverse + " / " + (i * (nSegmentsX + 1) + j + 1) + " " + ((i + 1) * (nSegmentsX + 1) + j) + " " + ((i + 1) * (nSegmentsX + 1) + j + 1));
            }
        }

        newMesh.vertices = vertices;
        newMesh.triangles = triangles;
        newMesh.RecalculateBounds();

        return newMesh;
    }

    public Mesh WrapNormalizedPlane(int nSegmentsX, int nSegmentsZ, ComputeVector3FromKxKz computePosition)
    {
        Mesh newMesh = new Mesh();
        newMesh.name = "wrapNormalizedPlane";

        int nbVertices = (nSegmentsX + 1) * (nSegmentsZ + 1);
        Vector3[] vertices = new Vector3[nbVertices];
        int[] triangles = new int[nSegmentsZ * nSegmentsX * 2 * 3];

        int index = 0;
        for (int i = 0; i < nSegmentsZ + 1; i++)
        {
            float kZ = (float)i / nSegmentsZ;
            for (int j = 0; j < nSegmentsX + 1; j++)
            {
                float kX = (float)j / nSegmentsX;
                vertices[index++] = computePosition(kX, kZ);
            }
        }

        //Triangles
        index = 0;
        for (int i = 0; i < nSegmentsZ; i++)
        {
            for (int j = 0; j < nSegmentsX; j++)
            {
                triangles[index++] = i * (nSegmentsX + 1) + j;
                triangles[index++] = (i + 1) * (nSegmentsX + 1) + j + 1;
                triangles[index++] = i * (nSegmentsX + 1) + j + 1;

                triangles[index++] = i * (nSegmentsX + 1) + j;
                triangles[index++] = (i + 1) * (nSegmentsX + 1) + j;
                triangles[index++] = (i + 1) * (nSegmentsX + 1) + j + 1;
            }
        }

        newMesh.vertices = vertices;
        newMesh.triangles = triangles;
        newMesh.RecalculateBounds();
        newMesh.RecalculateNormals();
        return newMesh;
    }

    public Mesh WrapNormalizedPlaneQuads(int nSegmentsX, int nSegmentsZ, ComputeVector3FromKxKz computePosition)
    {
        Mesh newMesh = new Mesh();
        newMesh.name = "wrapNormalizedPlaneQuads";

        int nbVertices = (nSegmentsX + 1) * (nSegmentsZ + 1);
        Vector3[] vertices = new Vector3[nbVertices];
        int[] quads = new int[nSegmentsZ * nSegmentsX * 4];

        int index = 0;
        for (int i = 0; i < nSegmentsZ + 1; i++)
        {
            float kZ = (float)i / nSegmentsZ;
            for (int j = 0; j < nSegmentsX + 1; j++)
            {
                float kX = (float)j / nSegmentsX;
                vertices[index++] = computePosition(kX, kZ);
            }
        }

        //Triangles
        index = 0;
        for (int i = 0; i < nSegmentsZ; i++)
        {
            for (int j = 0; j < nSegmentsX; j++)
            {
                quads[index++] = i * (nSegmentsX + 1) + j;
                quads[index++] = (i + 1) * (nSegmentsX + 1) + j;
                quads[index++] = (i + 1) * (nSegmentsX + 1) + j + 1;
                quads[index++] = i * (nSegmentsX + 1) + j + 1;
            }
        }

        newMesh.vertices = vertices;
        newMesh.SetIndices(quads, MeshTopology.Quads, 0);
        newMesh.RecalculateBounds();
        newMesh.RecalculateNormals();
        return newMesh;
    }

    public Mesh CreateRegularPolygon(int size, float radius)
    {
        Mesh newMesh = new Mesh();
        int nbVertices = size * 2 + 1;
        Vector3[] vertices = new Vector3[nbVertices];
        int nbQuads = size;
        int[] quads = new int[nbQuads * 4];

        int index = 0;
        vertices[index++] = Vector3.zero;

        float stepDeg = 360 / size;
        for (int i = 0; i < nbQuads; i++)
        {
            Quaternion angleQuat = Quaternion.Euler(0, stepDeg * i, 0);
            Vector3 newVertice = angleQuat * Vector3.right * radius;
            if(i > 0)
            {
                vertices[index++] = Vector3.Lerp(newVertice, vertices[index - 2], .5f);
                //Debug.Log("Lerp index : " + index + " | " + newVertice + " " + vertices[index - 1]);
            }
            vertices[index++] = newVertice;
            //Debug.Log("Vertice index : " + index);
        }
        vertices[index++] = Vector3.Lerp(vertices[index-2], vertices[1], .5f);

        for (int i=0; i < nbQuads; i++)
        {
            int offset = i * 4;
            quads[offset] = 0;
            if (i == 0)
            {
                quads[offset + 1] = index - 1;
                quads[offset + 2] = 1;
                quads[offset + 3] = 2;
            }
            else
            {
                quads[offset + 1] = i * 2;
                quads[offset + 2] = i * 2 + 1;
                quads[offset + 3] = i * 2 + 2;
            }
            
        }

        newMesh.vertices = vertices;
        newMesh.SetIndices(quads, MeshTopology.Quads, 0);
        newMesh.RecalculateBounds();
        newMesh.RecalculateNormals();
        return newMesh;
    }

    public Mesh createTruc()
    {
        Mesh newMesh = new Mesh();
        int n = 20;
        Vector3[] vertices = new Vector3[n * n];
        for(int i=0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                vertices[i*n + j] = new Vector3((float)i, 0, (float)j);
            }
        }

        int nbQuads = 3;
        int[] quads = new int[nbQuads * 4];
        quads[0] = 0;
        quads[1] = 2;
        quads[2] = n * 4 + 6;
        quads[3] = n * 4 + 0;

        quads[4] = 2;
        quads[5] = 3;
        quads[6] = n * 4 + 7;
        quads[7] = n * 4 + 6;

        quads[8] = 3;
        quads[9] = 8;
        quads[10] = n * 5 + 8;
        quads[11] = n * 4 + 7;

        newMesh.vertices = vertices;
        newMesh.SetIndices(quads, MeshTopology.Quads, 0);
        newMesh.RecalculateBounds();
        newMesh.RecalculateNormals();
        return newMesh;
    }

    public Mesh createCube(int size)
    {
        Mesh newMesh = new Mesh();
        Vector3[] vertices = new Vector3[8];
        vertices[0] = Vector3.zero;
        vertices[1] = new Vector3(size, 0, 0);
        vertices[2] = new Vector3(size, 0, size);
        vertices[3] = new Vector3(0, 0, size);
        vertices[4] = new Vector3(0, size, 0);
        vertices[5] = new Vector3(size, size, 0);
        vertices[6] = new Vector3(size, size, size);
        vertices[7] = new Vector3(0, size, size);

        int nbQuads = 6;
        int[] quads = new int[nbQuads * 4];
        quads[0] = 0;
        quads[1] = 1;
        quads[2] = 2;
        quads[3] = 3;

        quads[4] = 4;
        quads[5] = 5;
        quads[6] = 1;
        quads[7] = 0;

        quads[8] = 5;
        quads[9] = 6;
        quads[10] = 2;
        quads[11] = 1;

        quads[12] = 6;
        quads[13] = 7;
        quads[14] = 3;
        quads[15] = 2;

        quads[16] = 7;
        quads[17] = 4;
        quads[18] = 0;
        quads[19] = 3;

        quads[20] = 7;
        quads[21] = 6;
        quads[22] = 5;
        quads[23] = 4;

        newMesh.vertices = vertices;
        newMesh.SetIndices(quads, MeshTopology.Quads, 0);
        newMesh.RecalculateBounds();
        newMesh.RecalculateNormals();
        return newMesh;
    }

    public static string ExportMeshCSV(Mesh mesh)
        {
            if (!mesh) return "";

            List<string> strings = new List<string>();
            strings.Add("VertexIndex    VertexPosX  VertexPosY  VertexPosZ  QuadIndex   QuadVertexIndex1    QuadVertexIndex2    QuadVertexIndex3    QuadVertexIndex4");

            Vector3[] vertices = mesh.vertices;
            int[] quads = mesh.GetIndices(0);

            for(int i=0; i < vertices.Length; i++)
            {
                Vector3 pos = vertices[i];
                strings.Add($"{i}\t{pos.x.ToString("N02")}\t{pos.y.ToString("N02")}\t{pos.z.ToString("N02")}\t");
            }

            int index = 0;
            for (int i = 0; i < quads.Length/4; i++)
            {
                string tmpStr = $"{i}\t{quads[index++]}\t{quads[index++]}\t{quads[index++]}\t{quads[index++]}";
                if (i + 1 < strings.Count) strings[i + 1] += tmpStr;
                else strings.Add("\t\t\t\t" + tmpStr);
            }

                return string.Join("\n", strings);
        }
}
