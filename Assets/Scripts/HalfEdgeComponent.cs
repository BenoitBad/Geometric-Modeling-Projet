using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class HalfEdgeComponent : MonoBehaviour
{
    [Header("Catmull Clark")]
    [SerializeField] int m_nbIterationCatmull;

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

    private Mesh m_Mesh;

    public HalfEdgeRepresentation m_HalfEdgeRepresentation;

    CatmullClark mCatmull;

    // Start is called before the first frame update
    void Start()
    {
        MeshGenerator meshGenerator = new MeshGenerator();
        MeshFilter m_Mf = GetComponent<MeshFilter>();

        //m_Mesh = meshGenerator.CreateRegularPolygon(5, 2f);
        /*m_Mesh = meshGenerator.WrapNormalizedPlaneQuads(20, 10,
            (kX, kZ) => {
                //Sphere
                float theta = kX * 2 * Mathf.PI;
                float phi = (1 - kZ) * Mathf.PI;
                float rho = 2;
                return new Vector3(rho * Mathf.Cos(theta) * Mathf.Sin(phi), rho * Mathf.Cos(phi), rho * Mathf.Sin(theta) * Mathf.Sin(phi));
            }
        );*/
        //m_Mesh = meshGenerator.createTruc();
        m_Mesh = meshGenerator.createCube(3);
        /*m_Mesh = meshGenerator.WrapNormalizedPlaneQuads(2, 2,
            (kX, kZ) => {
                return new Vector3(kX, 0, kZ);
            }
        );*/

        m_HalfEdgeRepresentation = new HalfEdgeRepresentation(m_Mesh);
        gameObject.AddComponent<MeshCollider>();
        mCatmull = new CatmullClark();

        // Boucle des itérations de Catmull Clark
        for(int i = 0; i < m_nbIterationCatmull; i++)
        {
            mCatmull.CatmullClarkAlgorithm(m_HalfEdgeRepresentation);
            m_Mesh = m_HalfEdgeRepresentation.getMeshVertexFaces();
            m_HalfEdgeRepresentation = new HalfEdgeRepresentation(m_Mesh);
        }
        m_Mf.sharedMesh = m_Mesh;
    }

    private void OnDrawGizmos()
    {
        GizmosHalfEdge();
        GizmosCatmullClark();
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

                    Vector3 sourceVertexRelative = he.sourceVertex.vertex + transform.position;
                    Vector3 nextVertexRelative = he.nextEdge.sourceVertex.vertex + transform.position;

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
        if(mCatmull != null)
        {
            // EdgePoints
            if (m_DisplayEdgePoints)
            {
                int nbEdgePoints = 0;
                foreach (EdgePoint EdP in mCatmull.EdgePoints)
                {
                    if (nbEdgePoints > m_NMaxEdgePoints) break;
                    Gizmos.color = Color.magenta;
                    Gizmos.DrawSphere(EdP.pt, .02f);
                    nbEdgePoints++;
                }
            }

            // FacePoints
            if (m_DisplayFacePoints)
            {
                int nbFacePoints = 0;
                foreach (Vector3 FaP in mCatmull.facePoints)
                {
                    if (nbFacePoints > m_NMaxFacePoints) break;
                    Gizmos.color = Color.blue;
                    Gizmos.DrawSphere(FaP, .01f);
                    nbFacePoints++;
                }
            }

            // MidPoints
            if (m_DisplayMidPoints)
            {
                int nbMidPoints = 0;
                foreach (Vector3 MidP in mCatmull.midPoints)
                {
                    if (nbMidPoints > m_NMaxMidPoints) break;
                    Gizmos.color = Color.green;
                    Gizmos.DrawSphere(MidP, .01f);
                    nbMidPoints++;
                }
            }
        }
    }
}
