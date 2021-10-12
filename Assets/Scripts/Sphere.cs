using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Sphere
{
    public Vector3 center;
    public float radius;

    public Sphere(Vector3 center, float radius)
    {
        this.center = center;
        this.radius = radius;
    }

    public Sphere(Vector3 center, Vector3 pt)
    {
        this.center = center;
        this.radius = (center - pt).magnitude;
    }
}
