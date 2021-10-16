using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InterSegmentPlane : MonoBehaviour
{
    [SerializeField] GameObject segmentObject;
    [SerializeField] GameObject planeObject;
    [SerializeField] GameObject intersection;

    // Update is called once per frame
    void Update()
    {

        Vector3 intersectionPos = checkIntersection();
        if (intersectionPos != null)
        {
            intersection.SetActive(true);
            intersection.transform.position = intersectionPos;
        } else
        {
            intersection.SetActive(false);
        }
    }

    Vector3 checkIntersection()
    {
        Segment segment = segmentObject.GetComponent<SegmentObject>().seg;
        Plane plane = planeObject.GetComponent<PlaneObject>().p;
        Vector3 interPt;
        Vector3 interNormal;
        GeoFunc.InterSegmentPlane(segment, plane, out interPt, out interNormal);
        return interPt;
    }
}
