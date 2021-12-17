using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimCatmullClark : MonoBehaviour
{
    [SerializeField] List<HalfEdgeComponent> HeMesh;
    [SerializeField] int nbIteration;

    private int state;
    private bool animStarted = false;

    //private List<HalfEdgeComponent> mHalfEdgeComponents;

    private void Start()
    {
        state = 1;
    }

    void Update()
    {
        if (!animStarted)
        {
            StartCoroutine(AnimationCatmullClark());
            animStarted = true;
        }
    }

    IEnumerator AnimationCatmullClark()
    {
        while (true)
        {
            yield return new WaitForSeconds(2.0f);
            foreach(HalfEdgeComponent heComp in HeMesh)
            {
                heComp.IterateCatmull(state);
            }
            state++;
            Debug.Log("State: " + state);
            if (state >= nbIteration+1) state = 0;
        }
    }
}
