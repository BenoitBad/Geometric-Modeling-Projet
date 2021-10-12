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

    bool InterSegmentSphere(Segment seg, Sphere sph, out Vector3 interPt, out Vector3 interNormal)
    {
        interPt = Vector3.zero;
        interNormal = Vector3.zero;
        Vector3 AB = seg.pt2 - seg.pt1;
        Vector3 cA = seg.pt1 - sph.center;
        float a, b, c, t, delta;
        a = Vector3.Dot(AB, AB) * Vector3.Dot(AB, AB);
        b = 2 * Vector3.Dot(cA, AB);
        c = Vector3.Dot(cA, cA) * Vector3.Dot(cA, cA) - sph.radius * sph.radius;
        delta = b * b - 4 * a * c;
        if (delta < 0)
        {
            return false;
        }
        else if(Mathf.Approximately(delta, 0))
        {
            t = -b / (2 * a);
        }
        else
        {
            t = (-b + Mathf.Sqrt(delta)) / (2 * a);
        }
        interPt = seg.pt1 + t * AB;
        return true;
    }

    bool InterSegCylInf(Segment seg, Cylinder cyl, out Vector3 interPr, out Vector3 interNormal)
    {
        interPr = Vector3.zero; // A definir
        interNormal = Vector3.zero; // A definir
        return true;
    }
}