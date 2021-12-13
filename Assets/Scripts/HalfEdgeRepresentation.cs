using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertex
{
    public int index;
    public Vector3 vertex;
    public List<HalfEdge> adjacentEdge;

    public Vertex(int index, Vector3 vertex)
    {
        this.index = index;
        this.vertex = vertex;
        this.adjacentEdge = new List<HalfEdge>();
    }

    public Vertex(int index, float x, float y, float z) : this(index, new Vector3(x, y, z)) { }

    public override string ToString()
    {
        return "Vertex {id: " + index + "/" + vertex.ToString() + "}";
    }
}

public class HalfEdge
{
    public int index;

    public Face face;
    public Vertex sourceVertex;
    
    public HalfEdge prevEdge;
    public HalfEdge nextEdge;
    public HalfEdge twinEdge;

    public HalfEdge(int index, Face face, Vertex sourceVertex, HalfEdge prevEdge, HalfEdge nextEdge, HalfEdge twinEdge)
    {
        this.index = index;
        this.face = face;
        this.sourceVertex = sourceVertex;
        this.prevEdge = prevEdge;
        this.nextEdge = nextEdge;
        this.twinEdge = twinEdge;
    }

    public HalfEdge(int index, Face face, Vertex sourceVertex)
    {
        this.index = index;
        this.face = face;
        this.sourceVertex = sourceVertex;
    }

    public override string ToString()
    {
        return $"HalfEdge{{ {sourceVertex} {nextEdge.sourceVertex} }}";
    }
}

public class Face
{
    public int index;
    public HalfEdge side;

    public Face(int index, HalfEdge side)
    {
        this.index = index;
        this.side = side;
    }

    public Face(int index)
    {
        this.index = index;
    }
}

public class HalfEdgeRepresentation
{
    private Mesh m_Mesh;

    public List<Face> m_Faces;
    public List<Vertex> m_Vertices;
    public List<HalfEdge> m_Edges;

    public HalfEdgeRepresentation()
    {
        this.m_Faces = new List<Face>();
        this.m_Vertices = new List<Vertex>();
        this.m_Edges = new List<HalfEdge>();
    }

    public HalfEdgeRepresentation(Mesh mesh)
    {
        this.m_Mesh = mesh; // Must be in quad topology
        this.m_Faces = new List<Face>();
        this.m_Vertices = new List<Vertex>();
        this.m_Edges = new List<HalfEdge>();
        InitializeFromMeshQuad();
    }

    private void InitializeFromMesh()
    {
        int nTriangles = m_Mesh.triangles.Length / 3;
        int indexV = 0;
        int indexEdge = 0;
        int i;
        for(i=0; i < nTriangles; i++) // Generate all Vertex, Faces & HalfEdges but without twinEdge
        {
            Vertex v1 = new Vertex(indexV, m_Mesh.vertices[m_Mesh.triangles[indexV++]]);
            Vertex v2 = new Vertex(indexV, m_Mesh.vertices[m_Mesh.triangles[indexV++]]);
            Vertex v3 = new Vertex(indexV, m_Mesh.vertices[m_Mesh.triangles[indexV++]]);

            m_Vertices.Add(v1);
            m_Vertices.Add(v2);
            m_Vertices.Add(v3);

            Face newFace = new Face(i);
            m_Faces.Add(newFace);
            HalfEdge edge1 = new HalfEdge(indexEdge++, newFace, v1);
            HalfEdge edge2 = new HalfEdge(indexEdge++, newFace, v2);
            HalfEdge edge3 = new HalfEdge(indexEdge++, newFace, v3);
            m_Edges.Add(edge1);
            m_Edges.Add(edge2);
            m_Edges.Add(edge3);

            newFace.side = edge1;

            edge1.nextEdge = edge2;
            edge1.prevEdge = edge3;

            edge2.nextEdge = edge3;
            edge2.prevEdge = edge1;

            edge3.nextEdge = edge1;
            edge3.prevEdge = edge2;
        }

        // Ajouter les twinEdge
        List<HalfEdge> edgesToFill = new List<HalfEdge>(m_Edges);
        i = 0;
        while(edgesToFill.Count != 0)
        {
            HalfEdge edgeToFill = m_Edges[i];
            HalfEdge twinEdge = null;
            foreach(HalfEdge edge in edgesToFill)
            {
                if(IsEdgeTwin(edgeToFill, edge))
                {
                    twinEdge = edge;
                    break;
                }
            }
            if(twinEdge != null)
            {
                edgeToFill.twinEdge = twinEdge;
                twinEdge.twinEdge = edgeToFill;
                edgesToFill.Remove(edgeToFill);
                edgesToFill.Remove(twinEdge);
            } else
            {
                i++;
            }
        }
    }

    private void InitializeFromMeshQuad()
    {
        int[] quads = m_Mesh.GetIndices(0);
        int nQuads = quads.Length / 4;
        int indexV = 0;
        foreach(Vector3 vertex in m_Mesh.vertices) // Generate all Vertex
        {
            m_Vertices.Add(new Vertex(indexV, vertex));
            indexV++;
        }
        int indexEdge = 0;
        indexV = 0;
        int i;
        for (i = 0; i < nQuads; i++) // Generate all Faces & HalfEdges but without twinEdge
        {
            Face newFace = new Face(i);
            m_Faces.Add(newFace);
            HalfEdge edge1 = new HalfEdge(indexEdge++, newFace, m_Vertices[quads[indexV]]);
            m_Vertices[quads[indexV++]].adjacentEdge.Add(edge1);
            HalfEdge edge2 = new HalfEdge(indexEdge++, newFace, m_Vertices[quads[indexV]]);
            m_Vertices[quads[indexV++]].adjacentEdge.Add(edge2);
            HalfEdge edge3 = new HalfEdge(indexEdge++, newFace, m_Vertices[quads[indexV]]);
            m_Vertices[quads[indexV++]].adjacentEdge.Add(edge3);
            HalfEdge edge4 = new HalfEdge(indexEdge++, newFace, m_Vertices[quads[indexV]]);
            m_Vertices[quads[indexV++]].adjacentEdge.Add(edge4);
            m_Edges.Add(edge1);
            m_Edges.Add(edge2);
            m_Edges.Add(edge3);
            m_Edges.Add(edge4);

            newFace.side = edge1;

            edge1.nextEdge = edge2;
            edge1.prevEdge = edge4;

            edge2.nextEdge = edge3;
            edge2.prevEdge = edge1;

            edge3.nextEdge = edge4;
            edge3.prevEdge = edge2;

            edge4.nextEdge = edge1;
            edge4.prevEdge = edge3;
        }

        // Ajouter les twinEdge
        List<HalfEdge> edgesToFill = new List<HalfEdge>(m_Edges);
        while (edgesToFill.Count != 0)
        {
            HalfEdge edgeToFill = edgesToFill[0];
            HalfEdge twinEdge = null;
            foreach (HalfEdge edge in edgesToFill)
            {
                if (IsEdgeTwin(edgeToFill, edge))
                {
                    twinEdge = edge;
                    break;
                }
            }
            edgeToFill.twinEdge = twinEdge;
            edgesToFill.Remove(edgeToFill);
            if (twinEdge != null) // Si un twinEdge a été trouvé on lui met le edge actuel en twinEdge et le supprime de la liste
            {
                twinEdge.twinEdge = edgeToFill;
                edgesToFill.Remove(twinEdge);
            }
        }

        // Parcoure tout les HalfEdge n'ayant pas de Twin pour les ajouter aux adjacentEdge du vertex suivant
        foreach (HalfEdge he in m_Edges)
        {
            if(he.twinEdge == null)
            {
                he.nextEdge.sourceVertex.adjacentEdge.Add(he);
            }
        }
    }

    private bool IsEdgeTwin(HalfEdge edge1, HalfEdge edge2)
    {
        return edge1.sourceVertex == edge2.nextEdge.sourceVertex && edge1.nextEdge.sourceVertex == edge2.sourceVertex;
    }

    public Mesh getMeshVertexFaces()
    {
        Mesh newMesh = new Mesh();

        Vector3[] vertices = new Vector3[m_Vertices.Count];
        int[] quads = new int[m_Faces.Count * 4];

        // Génère le tableau des vertices
        foreach(Vertex v in m_Vertices)
        {
            vertices[v.index] = v.vertex;
        }

        // Génère le tableau des quads
        int idQuad = 0;
        foreach (Face f in m_Faces)
        {
            int offset = 0;
            HalfEdge he = f.side.prevEdge; // Je pars ici du prevEdge car en partant du f.side la conversion en triangle crée des points vers chaque coin
            do
            {
                quads[idQuad * 4 + offset] = he.sourceVertex.index;
                offset++;
                he = he.nextEdge;
            }
            while (f.side.prevEdge != he);
            idQuad++;
        }

        // ### DEBUG
        //for(int i=0; i < quads.Length; i=i+4)
        //{
        //    Debug.Log("Quad: [" + quads[i] + ", " + quads[i+1] + ", " + quads[i+2] + ", " + quads[i+3] + "]");
        //}
        //for (int i = 0; i < vertices.Length; i++)
        //{
        //    Debug.Log(vertices[i]);
        //}

        newMesh.vertices = vertices;
        newMesh.SetIndices(quads, MeshTopology.Quads, 0);
        newMesh.RecalculateBounds();
        newMesh.RecalculateNormals();
        return newMesh;
    }
}
