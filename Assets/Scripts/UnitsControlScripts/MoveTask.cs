using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTask : GameOrder
{
    public Vector3 destination { get; private set; }
    public MoveTask (Vector3 d)
    {
        destination = d;
    }
}
