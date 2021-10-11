using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereObject : MonoBehaviour
{
    Sphere s;

    // Start is called before the first frame update
    void Awake()
    {
        s = new Sphere();
        s.centre = transform.position;
        s.radius = transform.localScale.x / 2;
    }

    // Update is called once per frame
    void Update()
    {
        s.centre = transform.position;
        // En considérant que le scale en x/y/z sera toujours équivalent
        s.radius = transform.localScale.x / 2;
    }
}
