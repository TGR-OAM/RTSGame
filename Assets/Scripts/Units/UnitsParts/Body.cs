using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;

public class Body : MonoBehaviour
{
    public Robot robot;
    public float reachDistance { get; private set; } = 10;
    public float hp { get; private set; } = 75;
    public float visionDistance { get; private set; } = 30;
    void Start()
    {
        
        robot.bodyInit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
