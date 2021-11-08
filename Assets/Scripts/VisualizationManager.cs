using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VisualizationManager : MonoBehaviour
{
    // S'occupe de charger les scenes de visualisation (en additif), récupère les objets et lance une coroutine pour l'animation.
    // Voir si besoin d'un menuManager

    void Start()
    {
        loadInterSegmentPlane();
    }

    void Update()
    {
        
    }

    void loadInterSegmentPlane()
    {
        SceneManager.LoadScene("InterSegmentSphere", LoadSceneMode.Additive);
    }
}
