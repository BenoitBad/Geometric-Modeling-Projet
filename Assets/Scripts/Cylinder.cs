using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Cylinder
{
    public Vector3 pt1;
    public Vector3 pt2;
    public float radius;

    public Cylinder(Vector3 pt1, Vector3 pt2, float radius)
    {
        this.pt1 = pt1;
        this.pt2 = pt2;
        this.radius = radius;
    }
}
