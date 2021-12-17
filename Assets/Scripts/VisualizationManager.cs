using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VisualizationManager : MonoBehaviour
{
    // S'occupe de charger les scenes de visualisation (en additif)
    private string loadedScene = "Menu";

    public void loadCatmullclark()
    {
        deloadScene();
        SceneManager.LoadScene("CatmullClark", LoadSceneMode.Additive);
        loadedScene = "CatmullClark";
    }
    public void loadInterSegmentPlane()
    {
        deloadScene();
        SceneManager.LoadScene("InterSegmentPlane", LoadSceneMode.Additive);
        loadedScene = "InterSegmentPlane";
    }
    public void loadInterSegmentCylInf()
    {
        deloadScene();
        SceneManager.LoadScene("InterSegmentCylInf", LoadSceneMode.Additive);
        loadedScene = "InterSegmentCylInf";
    }

    public void loadInterSegmentSphere()
    {
        deloadScene();
        SceneManager.LoadScene("InterSegmentSphere", LoadSceneMode.Additive);
        loadedScene = "InterSegmentSphere";
    }
    public void loadDistancePoint()
    {
        deloadScene();
        SceneManager.LoadScene("DistancePoint", LoadSceneMode.Additive);
        loadedScene = "DistancePoint";
    }

    private void deloadScene()
    {
        if (loadedScene != "Menu")
            SceneManager.UnloadSceneAsync(loadedScene);
    }
}
