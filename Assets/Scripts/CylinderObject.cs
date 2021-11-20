using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderObject : MonoBehaviour
{
    public Cylinder c;

    // Start is called before the first frame update
    void Awake()
    {
        c = new Cylinder();
        c.pt1 = transform.position + -transform.up * transform.localScale.y/2;
        c.pt2 = transform.position + transform.up * transform.localScale.y / 2;
        // En considérant que le scale en x/z sera toujours équivalent
        c.radius = transform.localScale.x / 2;
    }

    // Update is called once per frame
    void Update()
    {
        c.pt1 = transform.position + -transform.up * transform.localScale.y / 2;
        c.pt2 = transform.position + transform.up * transform.localScale.y / 2;
        // En considérant que le scale en x/z sera toujours équivalent
        c.radius = transform.localScale.x / 2;
    }
}
