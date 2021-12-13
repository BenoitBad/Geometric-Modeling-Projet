using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VisualizationManager : MonoBehaviour
{
    // S'occupe de charger les scenes de visualisation (en additif), récupère les objets et lance une coroutine pour l'animation.
    // Voir si besoin d'un menuManager
    private int scene = 0;

    public void loadInterSegmentPlane()
    {
        deloadScene(scene);
        SceneManager.LoadScene("InterSegmentPlane", LoadSceneMode.Additive);
        scene = 3;
    }
    public void loadInterSegmentCylInf()
    {
        deloadScene(scene);
        SceneManager.LoadScene("InterSegmentCylInf", LoadSceneMode.Additive);
        scene = 2;
    }

    public void loadInterSegmentSphere()
    {
        deloadScene(scene);
        SceneManager.LoadScene("InterSegmentSphere", LoadSceneMode.Additive);
        scene = 1;
    }

    private void deloadScene(int idScene)
    {
        if (scene != 0)
            SceneManager.UnloadSceneAsync(scene);
    }
}
