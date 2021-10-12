using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Plane
{
    public Vector3 normal;
    public float d;

    Plane(Vector3 seg1, Vector3 seg2)
    {
        normal = Vector3.one; // Remplacer par produit vectoriel de seg1, seg2
        d = 0; // Replacer par ???
    }
}
