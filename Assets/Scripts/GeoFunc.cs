using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeoFunc
{
    bool InterSegmentPlane(Segment seg, Plane plane, out Vector3 interPt , out Vector3 interNormal)
    {
        interPt = Vector3.zero;
        interNormal = Vector3.zero;
        Vector3 AB = seg.pt2 - seg.pt1;
        float dotABn = Vector3.Dot(AB, plane.normal);
        if (Mathf.Approximately(dotABn, 0)) 
        { 
            return false; 
        }

        float t = (plane.d - Vector3.Dot(seg.pt1, plane.normal)) / (dotABn);

        if (t < 0 || t > 1) 
        { 
            return false; 
        }

        interPt = seg.pt1 + t * AB;

        if (dotABn < 0) {
            interNormal = plane.normal; 
        }
        else 
        {
            interNormal = -plane.normal; 
        }

        return true;
    }

    /*bool InterSegmentSphere(Segment seg, Sphere sph, out Vector3 interPt, out Vector3 interNormal)
    {
        interPt = Vector3.zero;
        interNormal = Vector3.zero;
        Vector3 AB = seg.pt2 - seg.pt1;
    }*/
}