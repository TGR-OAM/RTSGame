using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Game : MonoBehaviour
{

    public void ErrorInGame(string problem)
    {
        string filename = "crash" + System.DateTime.Now.ToString() + ".txt";
        filename = filename.Replace(':', '.');
        StreamWriter sw = File.CreateText(filename);
        sw.WriteLine(problem);
        sw.Close();
        Application.Quit();
    }
}
