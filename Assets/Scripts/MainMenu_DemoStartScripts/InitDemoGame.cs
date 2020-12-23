using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class InitDemoGame : MonoBehaviour
{
    public int DemoSceneIndex;
    
    public void StartDemoGame()
    {
        SceneManager.LoadScene(DemoSceneIndex);
    }
}
