using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgePoint
{
    public int index;
    public Vector3 pt;
    public HalfEdge edge;
    public HalfEdge twinEdge;

    public EdgePoint(int index, Vector3 pt, HalfEdge edge, HalfEdge twinEdge)
    {
        this.index = index;
        this.pt = pt;
        this.edge = edge;
        this.twinEdge = twinEdge;
    }

    public override string ToString()
    {
        string s = "EdgePoint: " + index + "[";
        s += pt + ", ";
        s += edge + ", ";
        s += twinEdge + "]";
        return s;
    }
}

public class CatmullClark
{
    public List<Vector3> facePoints = new List<Vector3>(); // Id = Id face
    public List<Vector3> midPoints = new List<Vector3>(); // Id = Id halfEdge
    public List<EdgePoint> EdgePoints = new List<EdgePoint>();

    public CatmullClark() { }

    private int valence(Vertex v)
    {
        return v.adjacentEdge.Count;
    }

    private Vector3 facePointsMoy(Vertex v)
    {
        Vector3 res = Vector3.zero;
        foreach(HalfEdge he in v.adjacentEdge)
        {
            if(he.sourceVertex == v) // Si le HalfEdge n'est pas sortant, il n'est pas dans une nouvelle face
            {
                res += facePoints[he.face.index];
            }
        }
        return res / nbAdjacentFaces(v);
    }

    private Vector3 midPointsMoy(Vertex v)
    {
        Vector3 res = Vector3.zero;
        foreach (HalfEdge he in v.adjacentEdge)
        {
            res += midPoints[he.index];
        }
        return res / v.adjacentEdge.Count;
    }

    private int nbAdjacentFaces(Vertex v)
    {
        int res = 0;
        foreach (HalfEdge he in v.adjacentEdge)
        {
            if (he.sourceVertex == v) // Si le HalfEdge n'est pas sortant, il n'est pas dans une nouvelle face
            {
                res++;
            }
        }
        return res;
    }

    private void repositionVertex(Vertex v)
    {
        float n = valence(v);
        // TODO : Check si en bordure et voir que faire
        if(nbAdjacentFaces(v) == n)
        {
            Vector3 Q = facePointsMoy(v);
            Vector3 R = midPointsMoy(v);
            Vector3 newPos = (Q / n) + ((2 * R) / n) + ((n - 3) / n) * v.vertex;
            //Debug.Log("Id: " + v.index + " " + v.vertex + " Valence: " + n);
            //Debug.Log("Q: " + Q);
            //Debug.Log("R: " + R);
            //Debug.Log("newPos: " + newPos);
            //Debug.Log("(Q / n) = x:" + (Q / n).x + " z:" +  (Q / n).z);
            //Debug.Log("((2 * R) / n) = x:" + ((2 * R) / n).x + " z:" + ((2 * R) / n).z);
            //Debug.Log("((n - 3) / n) * v.vertex = x:" + (((n - 3) / n) * v.vertex).x + " z:" + (((n - 3) / n) * v.vertex).z);
            v.vertex = newPos;
        }
    }

    private void splitEdgePoint(HalfEdgeRepresentation input, EdgePoint ep)
    {
        Vertex epVertex = new Vertex(input.m_Vertices.Count, ep.pt);
        input.m_Vertices.Add(epVertex);

        int idHe = input.m_Edges.Count;
        if(ep.twinEdge != null)
        {
            HalfEdge he1 = new HalfEdge(idHe++, ep.edge.face, epVertex);
            HalfEdge he2 = new HalfEdge(idHe++, ep.twinEdge.face, epVertex);
            input.m_Edges.Add(he1);
            input.m_Edges.Add(he2);
            epVertex.adjacentEdge.Add(he1);
            epVertex.adjacentEdge.Add(he2);

            he1.nextEdge = ep.edge.nextEdge;
            he1.prevEdge = ep.edge;
            he1.twinEdge = ep.twinEdge;

            he2.nextEdge = ep.twinEdge.nextEdge;
            he2.prevEdge = ep.twinEdge;
            he2.twinEdge = ep.edge;

            ep.twinEdge.nextEdge = he2;
            ep.twinEdge.twinEdge = he1;

            ep.edge.nextEdge = he1;
            ep.twinEdge = he2;
        } else // Si le EdgePoint n'a pas de twinEdge
        {
            HalfEdge he1 = new HalfEdge(idHe++, ep.edge.face, epVertex);
            input.m_Edges.Add(he1);
            epVertex.adjacentEdge.Add(he1);
            epVertex.adjacentEdge.Add(ep.edge);

            he1.nextEdge = ep.edge.nextEdge;
            he1.prevEdge = ep.edge;

            ep.edge.nextEdge = he1;
        }
    }

    private int CountVerticesInFace(Face f)
    {
        int nbVerticesInFace = 0;
        HalfEdge he = f.side;
        do
        {
            nbVerticesInFace++;
            he = he.nextEdge;
        }
        while (f.side != he);
        return nbVerticesInFace;
    }

    private void SplitFace(HalfEdgeRepresentation input, Face f, Vector3 facePoint)
    {
        int nbverticeInFace = CountVerticesInFace(f);
        if (nbverticeInFace % 2 == 0) // On split la face seulement si le nombre de vertices de la face est pair
        {
            int idFace = input.m_Faces.Count;
            int idHalfEdge = input.m_Edges.Count;

            Vertex fpVertex = new Vertex(input.m_Vertices.Count, facePoint);
            input.m_Vertices.Add(fpVertex);

            HalfEdge firstHe = f.side.nextEdge; // On sauvegarde le premier halfEdge de la première face à générer pour boucler dessus
            HalfEdge heNext = firstHe.nextEdge.nextEdge; // On garde en mémoire le premier edge de la prochaine face avant de changer les nextEdge
            // Réorganisation de la face actuelle
            f.side = firstHe;
            HalfEdge prevEdge = new HalfEdge(idHalfEdge++, f, fpVertex);
            fpVertex.adjacentEdge.Add(prevEdge);
            HalfEdge nextEdge = new HalfEdge(idHalfEdge++, f, firstHe.nextEdge.nextEdge.sourceVertex);
            input.m_Edges.Add(prevEdge);
            input.m_Edges.Add(nextEdge);

            prevEdge.prevEdge = nextEdge;
            prevEdge.nextEdge = firstHe;

            nextEdge.prevEdge = firstHe.nextEdge;
            nextEdge.nextEdge = prevEdge;

            firstHe.prevEdge = prevEdge;
            firstHe.nextEdge.nextEdge = nextEdge;

            // Création de nouvelles faces
            int countNewFace = 0;
            do
            {
                firstHe = heNext; // Prends le premier Edge de la nouvelle face sauvegardé auparavant
                heNext = firstHe.nextEdge.nextEdge; // Sauvegarde le premier edge de la prochaine face
                // Crée la nouvelle face et l'assigne aux edges existants
                Face newFace = new Face(idFace, firstHe);
                input.m_Faces.Add(newFace);
                firstHe.face = newFace;
                firstHe.nextEdge.face = newFace;

                // Crée les 2 nouveaux edges reliant au facePoint
                prevEdge = new HalfEdge(idHalfEdge++, newFace, fpVertex);
                fpVertex.adjacentEdge.Add(prevEdge);
                nextEdge = new HalfEdge(idHalfEdge++, newFace, firstHe.nextEdge.nextEdge.sourceVertex);
                input.m_Edges.Add(prevEdge);
                input.m_Edges.Add(nextEdge);

                // Fait les liaisons entre les edges existants et les nouveaux edges
                prevEdge.prevEdge = nextEdge;
                prevEdge.nextEdge = firstHe;

                nextEdge.prevEdge = firstHe.nextEdge;
                nextEdge.nextEdge = prevEdge;

                firstHe.prevEdge = prevEdge;
                firstHe.nextEdge.nextEdge = nextEdge;

                if(countNewFace == 0) // Si première face générée twinEdge de notre prevEdge est le nextEdge de la face de base
                {
                    prevEdge.twinEdge = f.side.nextEdge.nextEdge;
                    f.side.nextEdge.nextEdge.twinEdge = prevEdge;
                }
                else // Sinon le twinEdge de notre prevEdge est le nextEdge de la face d'avant
                {
                    prevEdge.twinEdge = input.m_Faces[idFace - countNewFace].side.nextEdge.nextEdge;
                    input.m_Faces[idFace - countNewFace].side.nextEdge.nextEdge.twinEdge = prevEdge;
                }

                // nbverticeInFace/2 - 2 = Nombre de nouvelles faces - la face de base qu'on a gardé - 1 car le compteur commence à 0
                if (nbverticeInFace/2 - 2 == countNewFace) // Si dernière face, le twinEdge de son nextEdge est le prevEdge de la face de base
                {
                    nextEdge.twinEdge = f.side.prevEdge;
                    f.side.prevEdge.twinEdge = nextEdge;
                }

                countNewFace++;
                idFace++;
            }
            while (f.side != heNext);
        }
    }

    public HalfEdgeRepresentation CatmullClarkAlgorithm(HalfEdgeRepresentation input)
    {
        facePoints.Clear();
        midPoints.Clear();
        EdgePoints.Clear();

        // Initialise un facePoint par face
        foreach (Face f in input.m_Faces)
        {
            Vector3 facePoint = Vector3.zero;
            HalfEdge he = f.side;
            do
            {
                midPoints.Add(Vector3.Lerp(he.sourceVertex.vertex, he.nextEdge.sourceVertex.vertex, 0.5f));
                facePoint += he.sourceVertex.vertex;
                he = he.nextEdge;
            }
            while (f.side != he);
            facePoint = facePoint / 4;
            facePoints.Add(facePoint);
        }

        // # Initialise les EdgePoints
        HashSet<HalfEdge> heDone = new HashSet<HalfEdge>();
        int idEdgePoint = 0;
        foreach (Face f in input.m_Faces)
        {
            HalfEdge he = f.side;
            do
            {
                if (!heDone.Contains(he))
                {
                    // Calcul EdgePoint -> Moyenne des extremités de l'edge et des facepoints des faces adjacentes
                    Vector3 edgePt;
                    if (he.twinEdge != null)
                    {
                        edgePt = (he.sourceVertex.vertex + he.nextEdge.sourceVertex.vertex + facePoints[f.index] + facePoints[he.twinEdge.face.index]) / 4;
                        heDone.Add(he.twinEdge);
                    }
                    else
                    {
                        edgePt = midPoints[he.index];
                    }
                    heDone.Add(he); // Note que les HalfEdge ont déja été traités
                    EdgePoints.Add(new EdgePoint(idEdgePoint++, edgePt, he, he.twinEdge));
                }
                he = he.nextEdge;
            }
            while (f.side != he);
        }

        // # Repositionne les vertices
        foreach (Vertex v in input.m_Vertices)
        {
            // DEBUG
            //string s = "";
            //foreach (HalfEdge he in v.adjacentEdge)
            //{
            //    s += he + " | ";
            //}
            //Debug.Log(s);
            // ###
            repositionVertex(v);
        }

        // # Split des edges en ajoutant les edgePoints
        foreach(EdgePoint ep in EdgePoints)
        {
            splitEdgePoint(input, ep);
        }

        // Split des faces en ajoutant les facePoints
        List<Face> actualFaceList = new List<Face>(input.m_Faces);
        foreach(Face f in actualFaceList)
        {
            Vector3 fp = facePoints[f.index];
            SplitFace(input, f, fp);
        }

        return input;
    }

}