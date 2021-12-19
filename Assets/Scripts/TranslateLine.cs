using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslateLine : MonoBehaviour
{
    private LineRenderer line;

    [SerializeField] float m_TranslationSpeed;
    [SerializeField] float m_TranslationDuration;
    [SerializeField] Vector3 m_TranslateVector;
    // Start is called before the first frame update
    void Awake()
    {
        line = this.GetComponent<LineRenderer>();
        StartCoroutine(Start());



    }

    // Use this for initialization
    IEnumerator Start()
    {
        bool translateRightToLeft = true;
        while (true)
        {
            yield return StartCoroutine(TranslatePoint(m_TranslationSpeed, m_TranslationDuration, m_TranslateVector,translateRightToLeft));
            translateRightToLeft = !translateRightToLeft;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator TranslatePoint(float translationSpeed,  float translationDuration, Vector3 end,bool translateRightToLeft)
    {
        if (!translateRightToLeft) end *= -1;
        Vector3 startPos1 = line.GetPosition(0);
        Vector3 startPos2 = line.GetPosition(1);

        Vector3 endPosPt1 = line.GetPosition(0) + end;
        Vector3 endPosPt2 = line.GetPosition(1) + end;
        float elapsedTime = 0;
        while (elapsedTime < translationDuration)
        {

            float k = elapsedTime / m_TranslationDuration;
            line.SetPosition(0, Vector3.Lerp(startPos1, endPosPt1, k));
            line.SetPosition(1, Vector3.Lerp(startPos2, endPosPt2, k));

            elapsedTime += Time.deltaTime;

            yield return null;
        }
        
    }
}
