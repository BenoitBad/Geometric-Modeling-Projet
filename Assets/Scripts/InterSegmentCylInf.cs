using System.Collections.Generic;
using UnityEngine;

public class InterSegmentCylInf : MonoBehaviour
{
    [SerializeField] GameObject segmentObject;
    [SerializeField] GameObject cylObject;
    [SerializeField] GameObject intersection1;
    [SerializeField] GameObject intersection2;

    // Update is called once per frame
    void Update()
    {

        List<Vector3> intersectionPos = checkIntersection();
        if (intersectionPos[0] != Vector3.zero)
        {
            intersection1.SetActive(true);
            intersection1.transform.position = intersectionPos[0];
            //Debug.Log("InterPt1: " + intersectionPos[0].x + " " + intersectionPos[0].y + " " + intersectionPos[0].z);
        }
        else
        {
            intersection1.SetActive(false);
        }

        if (intersectionPos.Count == 2 && intersectionPos[1] != Vector3.zero)
        {
            intersection2.SetActive(true);
            intersection2.transform.position = intersectionPos[1];
            //Debug.Log("InterPt2: " + intersectionPos[1].x + " " + intersectionPos[1].y + " " + intersectionPos[1].z);
        }
        else
        {
            intersection2.SetActive(false);
        }
    }

    List<Vector3> checkIntersection()
    {
        Segment segment = segmentObject.GetComponent<SegmentObject>().seg;
        Cylinder cyl = cylObject.GetComponent<CylinderObject>().c;
        List<Vector3> listInter = new List<Vector3>();
        Vector3 interPt1, interPt2;
        Vector3 interNormal1, interNormal2;
        GeoFunc.InterSegmentCylInf(segment, cyl, out interPt1, out interPt2, out interNormal1, out interNormal2);
        listInter.Add(interPt1);
        if(interPt2 != Vector3.zero)
        {
            listInter.Add(interPt2);
        }
        return listInter;
    }
}
