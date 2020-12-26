using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;

public class Wheels : MonoBehaviour
{
    public Robot robot;
    public float speed { get; private set; } = 50;
    public float acceleration { get; private set; } = 10;
    public float hp { get; private set; } = 50;
    public float stopDistance { get; private set; } = 5;
    void Start()
    {
        robot.wheelsInit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
