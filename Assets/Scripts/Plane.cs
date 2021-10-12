using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Plane
{
    public Vector3 normal;
    public float d;

    public Plane(Vector3 normal, float d)
    {
        this.normal = normal;
        this.d = d;
    }

    public Plane(Vector3 pt, Vector3 v1, Vector3 v2)
    {
        normal = Vector3.Cross(v1, v2).normalized; // Produit vectoriel de deux vecteurs = normale au plan créé par ces vecteurs
        d = Vector3.Dot(pt, normal); // Distance à l'origine de la projection du point par rapport à la normale
    }
}
