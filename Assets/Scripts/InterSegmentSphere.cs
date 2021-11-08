using UnityEngine;

public class InterSegmentSphere : MonoBehaviour
{
    [SerializeField] GameObject segmentObject;
    [SerializeField] GameObject sphereObject;
    [SerializeField] GameObject intersection;

    // Update is called once per frame
    void Update()
    {

        Vector3 intersectionPos = checkIntersection();
        if (intersectionPos != Vector3.zero)
        {
            intersection.SetActive(true);
            intersection.transform.position = intersectionPos;
            Debug.Log(intersectionPos.x + " " + intersectionPos.y + " " + intersectionPos.z);
        } else
        {
            intersection.SetActive(false);
        }
    }

    Vector3 checkIntersection()
    {
        Segment segment = segmentObject.GetComponent<SegmentObject>().seg;
        Sphere sphere = sphereObject.GetComponent<SphereObject>().s;
        Vector3 interPt;
        Vector3 interNormal;
        GeoFunc.InterSegmentSphere(segment, sphere, out interPt, out interNormal);
        return interPt;
    }
}
