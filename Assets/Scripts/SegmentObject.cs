using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegmentObject : MonoBehaviour
{
    public Segment seg;

    private LineRenderer line;

    // Start is called before the first frame update
    void Awake()
    {
        line = this.GetComponent<LineRenderer>();
        seg = new Segment(line.GetPosition(0), line.GetPosition(1));
    }

    // Update is called once per frame
    void Update()
    {
        seg.pt1 = line.GetPosition(0);
        seg.pt2 = line.GetPosition(1);
    }
}
