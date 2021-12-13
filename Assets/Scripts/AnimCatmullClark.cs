using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimCatmullClark : MonoBehaviour
{
    [SerializeField] GameObject iteration_0;
    [SerializeField] GameObject iteration_1;
    [SerializeField] GameObject iteration_2;
    int state = 1;

    bool animStarted = false;

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
            switch (state)
            {
                case 0:
                    iteration_2.SetActive(false);
                    iteration_0.SetActive(true);
                    break;
                case 1:
                    iteration_0.SetActive(false);
                    iteration_1.SetActive(true);
                    break;
                case 2:
                    iteration_1.SetActive(false);
                    iteration_2.SetActive(true);
                    break;
            }
            if(state < 2)
            {
                state++;
            } else
            {
                state = 0;
            }
        }
    }
}
