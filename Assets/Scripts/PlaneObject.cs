using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneObject : MonoBehaviour
{
    public Plane p;

    // Start is called before the first frame update
    void Awake()
    {
        Vector3 normal = transform.up;
        p = new Plane(normal, Vector3.Dot(transform.position, normal));
    }

    // Update is called once per frame
    void Update()
    {
        p.normal = transform.up;
        p.d = Vector3.Dot(transform.position, p.normal); // AHAH CA FAIT PD
        //Debug.Log("Normale: " + p.normal + " d (distance signée): " + p.d);
    }
}
