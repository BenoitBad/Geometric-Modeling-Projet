using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistancePoint : MonoBehaviour
{
    [SerializeField] GameObject point;
    [SerializeField] GameObject droiteObject;
    [SerializeField] GameObject planeObject;

    [Header("Textes")]
    [SerializeField] GameObject textDroite;
    [SerializeField] GameObject textPlane;

    private void Update()
    {
        RefreshDistanceDroite();
        RefreshDistancePlan();
    }

    private void RefreshDistanceDroite()
    {
        Segment droite = droiteObject.GetComponent<SegmentObject>().seg;
        float distancePointDroite = GeoFunc.DistancePointDroite(point.transform.position, droite);

        TextMesh texte = textDroite.GetComponent<TextMesh>();
        texte.text = "Distance point/droite : " + distancePointDroite;
    }

    private void RefreshDistancePlan()
    {
        Plane plan = planeObject.GetComponent<PlaneObject>().p;
        float distancePointPlan = GeoFunc.DistancePointPlane(point.transform.position, plan);

        TextMesh texte = textPlane.GetComponent<TextMesh>();
        texte.text = "Distance point/plan : " + distancePointPlan;
    }
}
