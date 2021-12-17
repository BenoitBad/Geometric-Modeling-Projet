using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class HalfEdgeComponent : MonoBehaviour
{
    [Header("HalfEdges")]
    [SerializeField] bool m_DisplayHalfEdges;
    [SerializeField] bool m_DisplayHalfEdgeNumber;
    [SerializeField] int m_NMaxHalfEdges;

    [Header("CatmullClark")]
    [SerializeField] bool m_DisplayMidPoints;
    [SerializeField] int m_NMaxMidPoints;
    [SerializeField] bool m_DisplayEdgePoints;
    [SerializeField] int m_NMaxEdgePoints;
    [SerializeField] bool m_DisplayFacePoints;
    [SerializeField] int m_NMaxFacePoints;

    private MeshFilter m_Mf;
    private Mesh m_BaseMesh;
    private Mesh m_Mesh;

    public HalfEdgeRepresentation m_HalfEdgeRepresentation;
    public CatmullClark m_Catmull;
    private int m_CatmullState;

    // Start is called before the first frame update
    void Start()
    {
        MeshGenerator meshGenerator = new MeshGenerator();
        m_Mf = GetComponentInChildren<MeshFilter>();

        m_BaseMesh = m_Mf.sharedMesh;
        m_Mesh = Mesh.Instantiate(m_BaseMesh);
        m_Mf.sharedMesh = m_BaseMesh;

        m_HalfEdgeRepresentation = new HalfEdgeRepresentation(m_Mesh);
        gameObject.AddComponent<MeshCollider>();
        m_Catmull = new CatmullClark();
        m_CatmullState = 0;
    }

    public void IterateCatmull(int nbIteration)
    {
        if(m_CatmullState > nbIteration)
        {
            m_HalfEdgeRepresentation = new HalfEdgeRepresentation(m_BaseMesh);
            m_CatmullState = 0;
        }
        while(m_CatmullState < nbIteration)
        {
            m_Mesh = m_HalfEdgeRepresentation.getMeshVertexFaces();
            m_HalfEdgeRepresentation = new HalfEdgeRepresentation(m_Mesh);
            m_Catmull.CatmullClarkAlgorithm(m_HalfEdgeRepresentation);
            m_CatmullState++;
        }
        m_Mesh = m_HalfEdgeRepresentation.getMeshVertexFaces();
        m_Mf.sharedMesh = m_Mesh;
    }

    private void OnDrawGizmos()
    {
        GizmosHalfEdge();
        GizmosCatmullClark();
    }

    private Vector3 positionAndScale(Vector3 v)
    {
        return transform.rotation * Vector3.Scale(v, transform.localScale) + transform.position;
    }

    private void GizmosHalfEdge()
    {
        if (!m_DisplayHalfEdges) return; // Ne pas afficher si m_DisplayHalfEdge non coché
        if (m_HalfEdgeRepresentation != null)
        {
            int iFace = 0;
            int iHe = 0;
            foreach (Face f in m_HalfEdgeRepresentation.m_Faces)
            {
                if (iFace % 2 == 0)
                    Handles.color = Color.red;
                else
                    Handles.color = Color.blue;
                iFace++;

                HalfEdge he = f.side;
                do
                {
                    if (iHe > m_NMaxHalfEdges - 1) return; // Stop si on a dessiné plus que défini par l'utilisateur

                    Vector3 sourceVertexRelative = positionAndScale(he.sourceVertex.vertex);
                    Vector3 nextVertexRelative = positionAndScale(he.nextEdge.sourceVertex.vertex);

                    Vector3 direction = nextVertexRelative - sourceVertexRelative;
                    Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
                    Handles.ArrowHandleCap(0, sourceVertexRelative, rotation, direction.magnitude * 0.8f, EventType.Repaint);

                    Gizmos.color = Color.red;
                    Gizmos.DrawSphere(sourceVertexRelative, .01f);

                    if (m_DisplayHalfEdgeNumber)
                    {
                        GUIStyle myStyle = new GUIStyle();
                        myStyle.fontSize = 16;
                        myStyle.normal.textColor = Color.red;
                        Handles.Label(sourceVertexRelative, he.sourceVertex.index.ToString(), myStyle);
                    }

                    iHe++;
                    he = he.nextEdge;
                }
                while (f.side != he);
            }
        }
    }

    private void GizmosCatmullClark()
    {
        if(m_Catmull != null)
        {
            // EdgePoints
            if (m_DisplayEdgePoints)
            {
                int nbEdgePoints = 0;
                foreach (EdgePoint EdP in m_Catmull.EdgePoints)
                {
                    if (nbEdgePoints > m_NMaxEdgePoints) break;
                    Gizmos.color = Color.magenta;
                    Gizmos.DrawSphere(positionAndScale(EdP.pt), .02f);
                    nbEdgePoints++;
                }
            }

            // FacePoints
            if (m_DisplayFacePoints)
            {
                int nbFacePoints = 0;
                foreach (Vector3 FaP in m_Catmull.facePoints)
                {
                    if (nbFacePoints > m_NMaxFacePoints) break;
                    Gizmos.color = Color.blue;
                    Gizmos.DrawSphere(positionAndScale(FaP), .01f);
                    nbFacePoints++;
                }
            }

            // MidPoints
            if (m_DisplayMidPoints)
            {
                int nbMidPoints = 0;
                foreach (Vector3 MidP in m_Catmull.midPoints)
                {
                    if (nbMidPoints > m_NMaxMidPoints) break;
                    Gizmos.color = Color.green;
                    Gizmos.DrawSphere(positionAndScale(MidP), .01f);
                    nbMidPoints++;
                }
            }
        }
    }
}
